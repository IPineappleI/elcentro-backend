using TangoSchool.DataAccess.DatabaseContexts;
using TangoSchool.DataAccess.Entities;
using TangoSchool.DataAccess.Repositories.Interfaces;

namespace TangoSchool.DataAccess.Repositories;

internal class LessonRequestsRepository : BaseRepository<LessonRequest>, ILessonRequestsRepository
{
    public LessonRequestsRepository(TangoSchoolDbContext tangoSchoolDbContext) : base(tangoSchoolDbContext)
    {
    }
}
