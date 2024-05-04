using Microsoft.EntityFrameworkCore;
using TangoSchool.ApplicationServices.Constants;
using TangoSchool.ApplicationServices.Extensions;
using TangoSchool.ApplicationServices.Mappers;
using TangoSchool.ApplicationServices.Models.Classrooms;
using TangoSchool.ApplicationServices.Services.Interfaces;
using TangoSchool.DataAccess.DatabaseContexts.Interfaces;
using TangoSchool.DataAccess.Entities;
using TangoSchool.DataAccess.Repositories.Interfaces;

namespace TangoSchool.ApplicationServices.Services;

internal class ClassroomsService : IClassroomsService
{
    private readonly IReadOnlyTangoSchoolDbContext _readOnlyTangoSchoolDbContext;
    private readonly IClassroomsRepository _classroomsRepository;

    public ClassroomsService
    (
        IReadOnlyTangoSchoolDbContext readOnlyTangoSchoolDbContext,
        IClassroomsRepository classroomsRepository
    )
    {
        _readOnlyTangoSchoolDbContext = readOnlyTangoSchoolDbContext;
        _classroomsRepository = classroomsRepository;
    }

    public async Task<Guid> CreateClassroom(CreateClassroomPayload payload, CancellationToken cancellationToken)
    {
        var newClassroom = _classroomsRepository.Add(payload.MapToDatabaseClassroom());

        await _classroomsRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return newClassroom.Id;
    }

    public async Task UpdateClassroom(UpdateClassroom payload, CancellationToken cancellationToken)
    {
        var classroom = await _readOnlyTangoSchoolDbContext
            .Classrooms
            .Where(x => x.Id == payload.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (classroom is null)
        {
            throw new ApplicationException(GeneralErrorMessages.ClassroomWasNotFound);
        }

        classroom.Name = payload.Name;
        classroom.Description = payload.Description;

        _classroomsRepository.Update(classroom);

        await _classroomsRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<GetClassroomResponse> GetClassroom(Guid id, CancellationToken cancellationToken)
    {
        var classroom = await _readOnlyTangoSchoolDbContext
            .Classrooms
            .Where(x => x.Id == id)
            .Select(x => new GetClassroomResponse(x.Name, x.Description, x.Terminated))
            .SingleOrDefaultAsync(cancellationToken);

        if (classroom is null)
        {
            throw new ApplicationException(GeneralErrorMessages.ClassroomWasNotFound);
        }

        return classroom;
    }

    public async Task TerminateClassroom(Guid id, CancellationToken cancellationToken)
    {
        var classroom = await _readOnlyTangoSchoolDbContext
            .Classrooms
            .Where(x => x.Id == id)
            .SingleOrDefaultAsync(cancellationToken);

        if (classroom is null)
        {
            throw new ApplicationException(GeneralErrorMessages.ClassroomWasNotFound);
        }

        classroom.Terminated = true;

        _classroomsRepository.Update(classroom);

        await _classroomsRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task RestoreClassroom(Guid id, CancellationToken cancellationToken)
    {
        var classroom = await _readOnlyTangoSchoolDbContext
            .Classrooms
            .Where(x => x.Id == id)
            .SingleOrDefaultAsync(cancellationToken);

        if (classroom is null)
        {
            throw new ApplicationException(GeneralErrorMessages.ClassroomWasNotFound);
        }

        classroom.Terminated = false;

        _classroomsRepository.Update(classroom);

        await _classroomsRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<GetAllClassroomsResponse> GetAllClassrooms
    (
        GetAllClassroomsPayload payload,
        CancellationToken cancellationToken
    )
    {
        IQueryable<Classroom> query = _readOnlyTangoSchoolDbContext.Classrooms;

        if (!payload.IncludeTerminated)
        {
            query = query.FilterActive();
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var result = await query
            .Paginate(payload.ItemsPerPage, payload.Page)
            .Select(x => new GetAllClassroomsResponseItem(x.Id, x.Name, x.Description, x.Terminated))
            .ToListAsync(cancellationToken);

        return new(result, totalCount);
    }

    public async Task<List<ClassroomHeader>> GetAvailableClassrooms
    (
        GetAvailableClassroomsPayload payload,
        CancellationToken cancellationToken
    )
    {
        if (payload.StartTime >= payload.FinishTime)
        {
            throw new ApplicationException(GeneralErrorMessages.StartTimeMustBeLessThanFinishTime);
        }

        var notAvailableClassroomsQuery = _readOnlyTangoSchoolDbContext
            .Lessons
            .Where(x => (x.StartTime >= payload.StartTime && x.StartTime <= payload.FinishTime)
                || (x.FinishTime >= payload.StartTime && x.FinishTime <= payload.FinishTime))
            .Select(x => x.ClassroomId);

        return await _readOnlyTangoSchoolDbContext
            .Classrooms
            .FilterActive()
            .Where(x => !notAvailableClassroomsQuery.Contains(x.Id))
            .Select(x => new ClassroomHeader(x.Id, x.Name))
            .ToListAsync(cancellationToken);
    }

    public async Task<List<ClassroomHeader>> GetClassroomHeaders
    (
        CancellationToken cancellationToken
    )
    {
        return await _readOnlyTangoSchoolDbContext
            .Classrooms
            .FilterActive()
            .Select(x => new ClassroomHeader(x.Id, x.Name))
            .ToListAsync(cancellationToken);
    }
}
