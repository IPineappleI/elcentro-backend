using TangoSchool.DataAccess.Entities;

namespace TangoSchool.DataAccess.Repositories.Interfaces;

public interface IGroupsRepository : IRepositoryBase<Group>
{
    public void AddStudentGroup(StudentGroup studentGroup);

    public void RemoveStudentGroup(StudentGroup studentGroup);
}
