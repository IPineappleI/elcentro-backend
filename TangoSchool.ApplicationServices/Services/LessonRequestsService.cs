using Microsoft.EntityFrameworkCore;
using TangoSchool.ApplicationServices.Constants;
using TangoSchool.ApplicationServices.Extensions;
using TangoSchool.ApplicationServices.Mappers;
using TangoSchool.ApplicationServices.Models.LessonRequests;
using TangoSchool.ApplicationServices.Services.Interfaces;
using TangoSchool.DataAccess.DatabaseContexts.Interfaces;
using TangoSchool.DataAccess.Entities;
using TangoSchool.DataAccess.Enums;
using TangoSchool.DataAccess.Repositories.Interfaces;

namespace TangoSchool.ApplicationServices.Services;

internal class LessonRequestsService : ILessonRequestsService
{
    private readonly IReadOnlyTangoSchoolDbContext _readOnlyTangoSchoolDbContext;
    private readonly ILessonRequestsRepository _lessonRequestsRepository;
    private readonly ILessonsService _lessonsService;

    public LessonRequestsService
    (
        IReadOnlyTangoSchoolDbContext readOnlyTangoSchoolDbContext,
        ILessonRequestsRepository lessonRequestsRepository,
        ILessonsService lessonsService
    )
    {
        _readOnlyTangoSchoolDbContext = readOnlyTangoSchoolDbContext;
        _lessonRequestsRepository = lessonRequestsRepository;
        _lessonsService = lessonsService;
    }

    public async Task<Guid> CreateLessonRequest
    (
        Guid userId,
        CreateLessonRequestPayload payload,
        CancellationToken cancellationToken
    )
    {
        var studentData = await _readOnlyTangoSchoolDbContext
            .Students
            .FilterActive()
            .Where(x => x.ApplicationUserId == userId)
            .Select(x => new
            {
                x.Id,
                HasActiveIndividualSubscription = x.Subscriptions.Any(y =>
                    y.ExpirationDate > DateTimeOffset.UtcNow
                    && y.LessonType == LessonType.Individual
                    && y.AttendedLessons.Count < y.LessonCount)
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (studentData is null)
        {
            throw new ApplicationException(GeneralErrorMessages.StudentWasNotFound);
        }

        if (!studentData.HasActiveIndividualSubscription)
        {
            throw new ApplicationException(GeneralErrorMessages.YouHasNoActiveIndividualSubscription);
        }

        var request = payload.MapToDatabaseLesson();
        request.StudentId = studentData.Id;

        var newLessonRequest = _lessonRequestsRepository.Add(request);

        await _lessonRequestsRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return newLessonRequest.Id;
    }

    public async Task<GetLessonRequestByTeacherResponse> GetLessonRequestsByTeacher
    (
        Guid userId,
        GetLessonRequestByTeacherPayload payload,
        CancellationToken cancellationToken
    )
    {
        var teacherData = await _readOnlyTangoSchoolDbContext
            .Students
            .FilterActive()
            .Where(x => x.ApplicationUserId == userId)
            .Select(x => new {x.Id})
            .SingleOrDefaultAsync(cancellationToken);

        if (teacherData is null)
        {
            throw new ApplicationException(GeneralErrorMessages.TeacherWasNotFound);
        }

        IQueryable<LessonRequest> query = _readOnlyTangoSchoolDbContext.LessonRequests;

        query = query
            .FilterActive()
            .Where(x => x.TeacherId == teacherData.Id);

        var totalCount = await query.CountAsync(cancellationToken);

        var result = await query
            .Paginate(payload.ItemsPerPage, payload.Page)
            .Select(x => new GetLessonRequestByTeacherResponseItem(
                x.Description,
                x.StartTime,
                x.FinishTime,
                x.StudentId,
                x.TeacherId
            ))
            .ToListAsync(cancellationToken);

        return new(result, totalCount);
    }

    public async Task<GetLessonRequestByTeacherResponse> GetLessonRequestsByStudent
    (
        Guid userId,
        GetLessonRequestByTeacherPayload payload,
        CancellationToken cancellationToken
    )
    {
        var studentData = await _readOnlyTangoSchoolDbContext
            .Students
            .FilterActive()
            .Where(x => x.ApplicationUserId == userId)
            .Select(x => new {x.Id})
            .SingleOrDefaultAsync(cancellationToken);

        if (studentData is null)
        {
            throw new ApplicationException(GeneralErrorMessages.StudentWasNotFound);
        }

        IQueryable<LessonRequest> query = _readOnlyTangoSchoolDbContext.LessonRequests;

        query = query
            .FilterActive()
            .Where(x => x.StudentId == studentData.Id);

        var totalCount = await query.CountAsync(cancellationToken);

        var result = await query
            .Paginate(payload.ItemsPerPage, payload.Page)
            .Select(x => new GetLessonRequestByTeacherResponseItem
            (
                x.Description,
                x.StartTime,
                x.FinishTime,
                x.StudentId,
                x.TeacherId
            ))
            .ToListAsync(cancellationToken);

        return new(result, totalCount);
    }

    public async Task RejectLessonRequest(Guid userId, Guid id, CancellationToken cancellationToken)
    {
        var lessonRequest = await _readOnlyTangoSchoolDbContext.LessonRequests
            .FilterActive()
            .Where(x => x.Id == id)
            .SingleOrDefaultAsync(cancellationToken);

        if (lessonRequest is null)
        {
            throw new ApplicationException(GeneralErrorMessages.LessonRequestWasNotFound);
        }

        lessonRequest.Terminated = true;

        _lessonRequestsRepository.Update(lessonRequest);

        await _lessonRequestsRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task ApproveLessonRequest
    (
        Guid userId,
        Guid lessonRequestId,
        ApproveLessonRequestPayload payload,
        CancellationToken cancellationToken
    )
    {
        var lessonRequest = await _readOnlyTangoSchoolDbContext.LessonRequests
            .FilterActive()
            .Where(x => x.Id == lessonRequestId)
            .SingleOrDefaultAsync(cancellationToken);

        if (lessonRequest is null)
        {
            throw new ApplicationException(GeneralErrorMessages.LessonRequestWasNotFound);
        }

        var teacherData = await _readOnlyTangoSchoolDbContext
            .Students
            .FilterActive()
            .Where(x => x.ApplicationUserId == userId)
            .Select(x => new {x.Id})
            .SingleOrDefaultAsync(cancellationToken);

        if (teacherData is null
            || lessonRequest.TeacherId == teacherData.Id)
        {
            throw new ApplicationException(GeneralErrorMessages.TeacherWasNotFound);
        }

        await _lessonRequestsRepository.UnitOfWork.BeginTransactionAsync(cancellationToken);

        await _lessonsService.CreateLesson(new
        (
            payload.Name,
            payload.Description,
            LessonType.Individual,
            payload.StartTime,
            payload.FinishTime,
            payload.ClassroomId,
            lessonRequest.TeacherId,
            lessonRequest.StudentId,
            null
        ), cancellationToken);

        lessonRequest.Terminated = true;

        _lessonRequestsRepository.Update(lessonRequest);

        await _lessonRequestsRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        await _lessonRequestsRepository.UnitOfWork.CommitTransactionAsync(cancellationToken);
    }
}
