using TangoSchool.DataAccess.DatabaseContexts;
using TangoSchool.DataAccess.Entities;
using TangoSchool.DataAccess.Repositories.Interfaces;

namespace TangoSchool.DataAccess.Repositories;

internal class AuditLogsRepository : BaseRepository<AuditLog>, IAuditLogsRepository
{
    public AuditLogsRepository(TangoSchoolDbContext tangoSchoolDbContext) : base(tangoSchoolDbContext)
    {
    }
}
