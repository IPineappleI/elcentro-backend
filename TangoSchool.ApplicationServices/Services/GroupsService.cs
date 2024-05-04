using Microsoft.EntityFrameworkCore;
using TangoSchool.ApplicationServices.Constants;
using TangoSchool.ApplicationServices.Extensions;
using TangoSchool.ApplicationServices.Mappers;
using TangoSchool.ApplicationServices.Models.Groups;
using TangoSchool.ApplicationServices.Models.Students;
using TangoSchool.ApplicationServices.Models.Teachers;
using TangoSchool.ApplicationServices.Services.Interfaces;
using TangoSchool.DataAccess.DatabaseContexts.Interfaces;
using TangoSchool.DataAccess.Entities;
using TangoSchool.DataAccess.Repositories.Interfaces;

namespace TangoSchool.ApplicationServices.Services;

internal class GroupsService : IGroupsService
{
    private readonly IReadOnlyTangoSchoolDbContext _readOnlyTangoSchoolDbContext;
    private readonly IGroupsRepository _groupsRepository;

    public GroupsService
    (
        IGroupsRepository groupsRepository,
        IReadOnlyTangoSchoolDbContext readOnlyTangoSchoolDbContext
    )
    {
        _groupsRepository = groupsRepository;
        _readOnlyTangoSchoolDbContext = readOnlyTangoSchoolDbContext;
    }

    public async Task<GroupsMetadata> GetGroupsMetadata
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

        return new(teachers, students);
    }

    public async Task<Guid> CreateGroup(CreateGroupPayload payload, CancellationToken cancellationToken)
    {
        var newGroup = _groupsRepository.Add(payload.MapToDatabaseGroup());

        if (newGroup.JoinedStudentGroups.Count > newGroup.MaxStudentCapacity)
        {
            throw new ApplicationException(GeneralErrorMessages.MaxStudentCapacityReached);
        }

        await _groupsRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return newGroup.Id;
    }

    public async Task UpdateGroup(UpdateGroupPayload payload, CancellationToken cancellationToken)
    {
        var group = await _readOnlyTangoSchoolDbContext
            .Groups
            .Where(x => x.Id == payload.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (group is null)
        {
            throw new ApplicationException(GeneralErrorMessages.GroupWasNotFound);
        }

        group.Name = payload.Name;
        group.Description = payload.Description;
        group.Level = payload.Level;
        group.MaxStudentCapacity = payload.MaxStudentCapacity;
        group.TeacherId = payload.TeacherId;

        _groupsRepository.Update(group);

        await _groupsRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<GetGroupResponse> GetGroup(Guid id, CancellationToken cancellationToken)
    {
        var group = await _readOnlyTangoSchoolDbContext
            .Groups
            .Where(x => x.Id == id)
            .Select(x => new GetGroupResponse
            (
                x.Name,
                x.Description,
                x.Level,
                x.MaxStudentCapacity,
                new
                (
                    x.TeacherId,
                    x.Teacher.ApplicationUser.FirstName,
                    x.Teacher.ApplicationUser.LastName,
                    x.Teacher.ApplicationUser.MiddleName
                ),
                x.JoinedStudentGroups.Select(y => new StudentHeader
                    (
                        y.StudentId,
                        y.Student.ApplicationUser.FirstName,
                        y.Student.ApplicationUser.LastName,
                        y.Student.ApplicationUser.MiddleName
                    ))
                    .ToList()
            ))
            .AsSplitQuery()
            .SingleOrDefaultAsync(cancellationToken);

        if (group is null)
        {
            throw new ApplicationException(GeneralErrorMessages.GroupWasNotFound);
        }

        return group;
    }

    public async Task<GetAllGroupsResponse> GetAllGroups
    (
        GetAllGroupsPayload payload,
        CancellationToken cancellationToken
    )
    {
        IQueryable<Group> query = _readOnlyTangoSchoolDbContext.Groups;

        if (!payload.IncludeTerminated)
        {
            query = query.Where(x => !x.Terminated);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var result = await query
            .Paginate(payload.ItemsPerPage, payload.Page)
            .Select(x => new GetAllGroupsResponseItem
            (
                x.Id,
                x.Name,
                x.Description,
                x.Level,
                x.JoinedStudentGroups.Count,
                x.MaxStudentCapacity,
                new
                (
                    x.TeacherId,
                    x.Teacher.ApplicationUser.FirstName,
                    x.Teacher.ApplicationUser.LastName,
                    x.Teacher.ApplicationUser.MiddleName
                ),
                x.Terminated
            ))
            .ToListAsync(cancellationToken);

        return new(result, totalCount);
    }

    public async Task<List<GroupHeader>> GetGroupHeaders
    (
        CancellationToken cancellationToken
    )
    {
        return await _readOnlyTangoSchoolDbContext
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
    }

    public async Task TerminateGroup(Guid id, CancellationToken cancellationToken)
    {
        var group = await _readOnlyTangoSchoolDbContext
            .Groups
            .Where(x => x.Id == id)
            .SingleOrDefaultAsync(cancellationToken);

        if (group is null)
        {
            throw new ApplicationException(GeneralErrorMessages.ClassroomWasNotFound);
        }

        group.Terminated = true;

        _groupsRepository.Update(group);

        await _groupsRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task RestoreGroup(Guid id, CancellationToken cancellationToken)
    {
        var group = await _readOnlyTangoSchoolDbContext
            .Groups
            .Where(x => x.Id == id)
            .SingleOrDefaultAsync(cancellationToken);

        if (group is null)
        {
            throw new ApplicationException(GeneralErrorMessages.ClassroomWasNotFound);
        }

        group.Terminated = false;

        _groupsRepository.Update(group);

        await _groupsRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task AddStudentToGroup(Guid groupId, Guid studentId, CancellationToken cancellationToken)
    {
        var group = await _readOnlyTangoSchoolDbContext
            .Groups
            .Include(x => x.JoinedStudentGroups)
            .Where(x => x.Id == groupId)
            .SingleOrDefaultAsync(cancellationToken);

        if (group is null)
        {
            throw new ApplicationException(GeneralErrorMessages.GroupWasNotFound);
        }

        if (group.JoinedStudentGroups.Count >= group.MaxStudentCapacity)
        {
            throw new ApplicationException(GeneralErrorMessages.MaxStudentCapacityReached);
        }

        if (group.JoinedStudentGroups
            .Any(x => x.StudentId == studentId))
        {
            throw new ApplicationException(GeneralErrorMessages.StudentHasAlreadyAddedToGroup);
        }

        _groupsRepository.AddStudentGroup(new()
        {
            GroupId = groupId,
            StudentId = studentId,
            JoinDate = DateTimeOffset.UtcNow
        });

        await _groupsRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveStudentFromGroup(Guid groupId, Guid studentId, CancellationToken cancellationToken)
    {
        var group = await _readOnlyTangoSchoolDbContext
            .Groups
            .Include(x => x.JoinedStudentGroups)
            .Where(x => x.Id == groupId)
            .SingleOrDefaultAsync(cancellationToken);

        if (group is null)
        {
            throw new ApplicationException(GeneralErrorMessages.GroupWasNotFound);
        }

        var studentGroup = group.JoinedStudentGroups
            .SingleOrDefault(x => x.StudentId == studentId);

        if (studentGroup is null)
        {
            throw new ApplicationException(GeneralErrorMessages.StudentGroupWasNotFound);
        }

        _groupsRepository.RemoveStudentGroup(studentGroup);

        await _groupsRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
    }
}
