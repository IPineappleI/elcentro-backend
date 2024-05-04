using TangoSchool.DataAccess.DatabaseContexts;
using TangoSchool.DataAccess.Entities;
using TangoSchool.DataAccess.Repositories.Interfaces;

namespace TangoSchool.DataAccess.Repositories;

internal class GroupsRepository : BaseRepository<Group>, IGroupsRepository
{
    public GroupsRepository(TangoSchoolDbContext tangoSchoolDbContext) : base(tangoSchoolDbContext)
    {
    }

    public void AddStudentGroup(StudentGroup studentGroup)
    {
        Context.StudentGroups.Add(studentGroup);
    }

    public void RemoveStudentGroup(StudentGroup studentGroup)
    {
        Context.StudentGroups.Remove(studentGroup);
    }
}
