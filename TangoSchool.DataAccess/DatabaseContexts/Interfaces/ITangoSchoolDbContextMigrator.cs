namespace TangoSchool.DataAccess.DatabaseContexts.Interfaces;

public interface ITangoSchoolDbContextMigrator
{
    Task Migrate(CancellationToken cancellationToken);
}
