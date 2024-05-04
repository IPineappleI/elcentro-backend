using Microsoft.EntityFrameworkCore;
using TangoSchool.DataAccess.DatabaseContexts.Interfaces;

namespace TangoSchool.DataAccess.DatabaseContexts;

internal class TangoSchoolDbContextMigrator : ITangoSchoolDbContextMigrator
{
    private readonly TangoSchoolDbContext _tangoSchoolDbContext;

    public TangoSchoolDbContextMigrator(TangoSchoolDbContext tangoSchoolDbContext)
    {
        _tangoSchoolDbContext = tangoSchoolDbContext;
    }

    public async Task Migrate(CancellationToken cancellationToken)
    {
        await _tangoSchoolDbContext.Database.MigrateAsync(cancellationToken);
    }
}
