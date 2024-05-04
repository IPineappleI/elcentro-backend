using TangoSchool.DataAccess.DatabaseContexts;
using TangoSchool.DataAccess.Entities;
using TangoSchool.DataAccess.Repositories.Interfaces;

namespace TangoSchool.DataAccess.Repositories;

internal class TeachersRepository : BaseRepository<Teacher>, ITeachersRepository
{
    public TeachersRepository(TangoSchoolDbContext tangoSchoolDbContext) : base(tangoSchoolDbContext)
    {
    }
}
