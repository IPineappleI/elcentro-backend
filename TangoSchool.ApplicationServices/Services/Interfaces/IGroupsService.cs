using TangoSchool.ApplicationServices.Models.Groups;

namespace TangoSchool.ApplicationServices.Services.Interfaces;

public interface IGroupsService
{
    Task<GroupsMetadata> GetGroupsMetadata
    (
        CancellationToken cancellationToken
    );
    
    Task<Guid> CreateGroup
    (
        CreateGroupPayload payload,
        CancellationToken cancellationToken
    );

    Task UpdateGroup
    (
        UpdateGroupPayload payload,
        CancellationToken cancellationToken
    );

    Task<GetGroupResponse> GetGroup
    (
        Guid id,
        CancellationToken cancellationToken
    );

    Task<GetAllGroupsResponse> GetAllGroups
    (
        GetAllGroupsPayload payload,
        CancellationToken cancellationToken
    );

    Task<List<GroupHeader>> GetGroupHeaders
    (
        CancellationToken cancellationToken
    );
    
    Task TerminateGroup
    (
        Guid id,
        CancellationToken cancellationToken
    );

    Task RestoreGroup
    (
        Guid id,
        CancellationToken cancellationToken
    );

    Task AddStudentToGroup(Guid groupId, Guid studentId, CancellationToken cancellationToken);
    
    Task RemoveStudentFromGroup(Guid groupId, Guid studentId, CancellationToken cancellationToken);
}
