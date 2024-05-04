using System.Text;
using Microsoft.EntityFrameworkCore;
using TangoSchool.ApplicationServices.Constants;
using TangoSchool.ApplicationServices.Extensions;
using TangoSchool.ApplicationServices.Mappers;
using TangoSchool.ApplicationServices.Models.Classrooms;
using TangoSchool.ApplicationServices.Models.Groups;
using TangoSchool.ApplicationServices.Models.Lessons;
using TangoSchool.ApplicationServices.Models.Students;
using TangoSchool.ApplicationServices.Models.Teachers;
using TangoSchool.ApplicationServices.Services.Interfaces;
using TangoSchool.DataAccess.DatabaseContexts.Interfaces;
using TangoSchool.DataAccess.Entities;
using TangoSchool.DataAccess.Enums;
using TangoSchool.DataAccess.Repositories.Interfaces;

namespace TangoSchool.ApplicationServices.Services;

internal class LessonsService : ILessonsService
{
    private readonly IReadOnlyTangoSchoolDbContext _readOnlyTangoSchoolDbContext;
    private readonly ILessonsRepository _lessonsRepository;

    public LessonsService
    (
        IReadOnlyTangoSchoolDbContext readOnlyTangoSchoolDbContext,
        ILessonsRepository lessonsRepository
    )
    {
        _readOnlyTangoSchoolDbContext = readOnlyTangoSchoolDbContext;
        _lessonsRepository = lessonsRepository;
    }

    public async Task<LessonsMetadata> GetLessonsMetadata
    (
        CancellationToken cancellationToken
    )
    {
        var teachers = await _readOnlyTangoSchoolDbContext
            .Teachers
            .FilterActive()
            .Select(x => new TeacherHeader
            (
                x.Id,
                x.ApplicationUser.FirstName,
                x.ApplicationUser.LastName,
                x.ApplicationUser.MiddleName
            ))
            .ToListAsync(cancellationToken);

        var students = await _readOnlyTangoSchoolDbContext
            .Students
            .FilterActive()
            .Select(x => new StudentHeader
            (
                x.Id,
                x.ApplicationUser.FirstName,
                x.ApplicationUser.LastName,
                x.ApplicationUser.MiddleName
            ))
            .ToListAsync(cancellationToken);

        var classrooms = await _readOnlyTangoSchoolDbContext
            .Classrooms
            .FilterActive()
            .Select(x => new ClassroomHeader
            (
                x.Id,
                x.Name
            ))
            .ToListAsync(cancellationToken);

        var groups = await _readOnlyTangoSchoolDbContext
            .Groups
            .FilterActive()
            .Select(x => new GroupHeader
            (
                x.Id,
                x.Name,
                x.JoinedStudentGroups.Count,
                x.Level
            ))
            .ToListAsync(cancellationToken);

        return new(teachers, students, classrooms, groups);
    }

    public async Task<Guid> CreateLesson(CreateLessonPayload payload, CancellationToken cancellationToken)
    {
        switch (payload.LessonType)
        {
            case LessonType.Individual when !payload.StudentId.HasValue || payload.GroupId.HasValue:
            {
                throw new ApplicationException(GeneralErrorMessages.InvalidLessonData);
            }
            case LessonType.Group when payload.StudentId.HasValue || !payload.GroupId.HasValue:
            {
                throw new ApplicationException(GeneralErrorMessages.InvalidLessonData);
            }
            case LessonType.Seminar when payload is {StudentId: not null, GroupId: not null}:
            {
                throw new ApplicationException(GeneralErrorMessages.InvalidLessonData);
            }
            default:
            {
                break;
            }
        }

        if (payload.StartTime >= payload.FinishTime)
        {
            throw new ApplicationException(GeneralErrorMessages.StartTimeMustBeLessThanFinishTime);
        }

        var timePeriodAvailable = await _readOnlyTangoSchoolDbContext
            .Lessons
            .Where(x => x.ClassroomId == payload.ClassroomId)
            .AllAsync(x => (x.StartTime <= payload.StartTime && x.StartTime <= payload.FinishTime)
                    || (x.StartTime >= payload.StartTime && x.FinishTime >= payload.FinishTime),
                cancellationToken);

        if (!timePeriodAvailable)
        {
            throw new ApplicationException(GeneralErrorMessages.TimePeriodIsNotAvailable);
        }

        var newLesson = _lessonsRepository.Add(payload.MapToDatabaseLesson());

        await _lessonsRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return newLesson.Id;
    }

    public async Task UpdateLesson(UpdateLesson payload, CancellationToken cancellationToken)
    {
        switch (payload.LessonType)
        {
            case LessonType.Individual when !payload.StudentId.HasValue || payload.GroupId.HasValue:
            {
                throw new ApplicationException(GeneralErrorMessages.InvalidLessonData);
            }
            case LessonType.Group when payload.StudentId.HasValue || !payload.GroupId.HasValue:
            {
                throw new ApplicationException(GeneralErrorMessages.InvalidLessonData);
            }
            case LessonType.Seminar when payload is {StudentId: not null, GroupId: not null}:
            {
                throw new ApplicationException(GeneralErrorMessages.InvalidLessonData);
            }
            default:
            {
                break;
            }
        }

        if (payload.StartTime >= payload.FinishTime)
        {
            throw new ApplicationException(GeneralErrorMessages.StartTimeMustBeLessThanFinishTime);
        }

        var timePeriodAvailable = await _readOnlyTangoSchoolDbContext
            .Lessons
            .Where(x => x.Id != payload.Id)
            .Where(x => x.ClassroomId == payload.ClassroomId)
            .AllAsync(x => (x.StartTime <= payload.StartTime && x.StartTime <= payload.FinishTime)
                    || (x.StartTime >= payload.StartTime && x.FinishTime >= payload.FinishTime),
                cancellationToken);

        if (!timePeriodAvailable)
        {
            throw new ApplicationException(GeneralErrorMessages.TimePeriodIsNotAvailable);
        }

        var lesson = await _readOnlyTangoSchoolDbContext
            .Lessons
            .Where(x => x.Id == payload.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (lesson is null)
        {
            throw new ApplicationException(GeneralErrorMessages.LessonWasNotFound);
        }

        lesson.Name = payload.Name;
        lesson.Description = payload.Description;
        lesson.LessonType = payload.LessonType;
        lesson.StartTime = payload.StartTime;
        lesson.FinishTime = payload.FinishTime;
        lesson.ClassroomId = payload.ClassroomId;
        lesson.TeacherId = payload.TeacherId;
        lesson.StudentId = payload.StudentId;
        lesson.GroupId = payload.GroupId;

        _lessonsRepository.Update(lesson);

        await _lessonsRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<GetLessonResponse> GetLesson(Guid id, CancellationToken cancellationToken)
    {
        var lesson = await _readOnlyTangoSchoolDbContext
            .Lessons
            .Where(x => x.Id == id)
            .Select(x => new GetLessonResponse
            (
                x.Id,
                x.Name,
                x.Description,
                x.LessonType,
                x.StartTime,
                x.FinishTime,
                new
                (
                    x.ClassroomId,
                    x.Classroom.Name
                ),
                new
                (
                    x.Teacher.Id,
                    x.Teacher.ApplicationUser.FirstName,
                    x.Teacher.ApplicationUser.LastName,
                    x.Teacher.ApplicationUser.MiddleName
                ),
                x.Student != null
                    ? new StudentHeader
                    (
                        x.Teacher.Id,
                        x.Teacher.ApplicationUser.FirstName,
                        x.Teacher.ApplicationUser.LastName,
                        x.Teacher.ApplicationUser.MiddleName
                    )
                    : null,
                x.Group != null
                    ? new GroupHeader
                    (
                        x.Group.Id,
                        x.Group.Name,
                        x.Group.JoinedStudentGroups.Count,
                        x.Group.Level
                    )
                    : null
            ))
            .AsSplitQuery()
            .SingleOrDefaultAsync(cancellationToken);

        if (lesson is null)
        {
            throw new ApplicationException(GeneralErrorMessages.LessonWasNotFound);
        }

        return lesson;
    }

    public async Task<GetAllLessonsResponse> GetAllLessons
    (
        GetAllLessonsPayload payload,
        CancellationToken cancellationToken
    )
    {
        var query = _readOnlyTangoSchoolDbContext.Lessons.AsQueryable();

        return await GetAllLessonsResponse(query, payload, cancellationToken);
    }

    private static async Task<GetAllLessonsResponse> GetAllLessonsResponse
    (
        IQueryable<Lesson> baseQuery,
        GetAllLessonsPayload payload,
        CancellationToken cancellationToken
    )
    {
        if (!payload.IncludeTerminated)
        {
            baseQuery = baseQuery.FilterActive();
        }

        var result = await baseQuery
            .Where(x => x.StartTime > payload.From || x.FinishTime >= payload.To)
            .Select(x => new GetAllLessonsResponseItem
            (
                new
                (
                    x.Id,
                    x.Name,
                    x.LessonType,
                    x.StartTime,
                    x.FinishTime
                ),
                new
                (
                    x.Teacher.Id,
                    x.Teacher.ApplicationUser.FirstName,
                    x.Teacher.ApplicationUser.LastName,
                    x.Teacher.ApplicationUser.MiddleName
                ),
                new
                (
                    x.Classroom.Id,
                    x.Classroom.Name
                )
            ))
            .ToListAsync(cancellationToken);

        return new(result);
    }

    public async Task<GetAllLessonsResponse> GetAllLessonsByStudent
    (
        Guid userId,
        GetAllLessonsPayload payload,
        CancellationToken cancellationToken
    )
    {
        var query = _readOnlyTangoSchoolDbContext
            .Lessons
            .Where(x => x.Teacher.ApplicationUserId == userId);

        return await GetAllLessonsResponse(query, payload, cancellationToken);
    }

    public async Task<GetAllLessonsResponse> GetAllLessonsByTeacher
    (
        Guid userId,
        GetAllLessonsPayload payload,
        CancellationToken cancellationToken
    )
    {
        var query = _readOnlyTangoSchoolDbContext
            .Lessons
            .Where(x => (x.Student != null && x.Student.ApplicationUserId == userId)
                || (x.Group != null
                    && x.Group.JoinedStudentGroups
                        .Any(y => y.Student.ApplicationUserId == userId)));

        return await GetAllLessonsResponse(query, payload, cancellationToken);
    }

    public async Task SetLessonAttendance
    (
        Guid id,
        SetLessonAttendancePayload payload,
        CancellationToken cancellationToken
    )
    {
        var lesson = await _readOnlyTangoSchoolDbContext
            .Lessons
            .Where(x => x.Id == id)
            .SingleOrDefaultAsync(cancellationToken);

        if (lesson is null)
        {
            throw new ApplicationException(GeneralErrorMessages.LessonWasNotFound);
        }

        var studentsSubscriptions = await _readOnlyTangoSchoolDbContext
            .Students
            .FilterActive()
            .Where(x => payload.StudentIds.Contains(x.Id))
            .Select(x => new
            {
                Student = new StudentHeader
                (
                    x.Id,
                    x.ApplicationUser.FirstName,
                    x.ApplicationUser.LastName,
                    x.ApplicationUser.MiddleName
                ),
                Subscription = x.Subscriptions
                    .Where(y => y.ExpirationDate >= DateTimeOffset.UtcNow
                        && y.LessonType == lesson.LessonType
                        && y.AttendedLessons.Count < y.LessonCount)
                    .OrderBy(y => y.ExpirationDate)
                    .FirstOrDefault()
            })
            .AsSplitQuery()
            .ToListAsync(cancellationToken);

        var studentsWithoutSubscription = studentsSubscriptions
            .Where(x => x.Subscription is null)
            .Select(x => x.Student)
            .ToList();

        if (studentsWithoutSubscription.Any())
        {
            var stringBuilder = new StringBuilder(GeneralErrorMessages.SomeStudentsDoNotHaveSubscription);

            foreach (var student in studentsWithoutSubscription)
            {
                stringBuilder.Append(student.FirstName);
                stringBuilder.Append(' ');
                stringBuilder.Append(student.LastName);
                stringBuilder.Append(',');
            }

            stringBuilder.Remove(stringBuilder.Length - 1, 1);

            throw new ApplicationException(stringBuilder.ToString());
        }

        var attendedLessonsSubscriptions = studentsSubscriptions
            .Select(studentsSubscription => new LessonSubscription
            {
                LessonId = lesson.Id,
                SubscriptionId = studentsSubscription.Subscription!.Id
            })
            .ToList();

        _lessonsRepository.AddLessonSubscriptions(attendedLessonsSubscriptions);
        await _lessonsRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task TerminateLesson(Guid id, CancellationToken cancellationToken)
    {
        var lesson = await _readOnlyTangoSchoolDbContext
            .Lessons
            .Where(x => x.Id == id)
            .SingleOrDefaultAsync(cancellationToken);

        if (lesson is null)
        {
            throw new ApplicationException(GeneralErrorMessages.LessonWasNotFound);
        }

        lesson.Terminated = true;

        _lessonsRepository.Update(lesson);

        await _lessonsRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task RestoreLesson(Guid id, CancellationToken cancellationToken)
    {
        var lesson = await _readOnlyTangoSchoolDbContext
            .Lessons
            .Where(x => x.Id == id)
            .SingleOrDefaultAsync(cancellationToken);

        if (lesson is null)
        {
            throw new ApplicationException(GeneralErrorMessages.LessonWasNotFound);
        }

        lesson.Terminated = false;

        _lessonsRepository.Update(lesson);

        await _lessonsRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
    }
}
