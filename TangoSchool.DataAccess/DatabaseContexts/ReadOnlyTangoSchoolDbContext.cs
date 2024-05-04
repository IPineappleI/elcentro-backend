using Microsoft.EntityFrameworkCore;
using TangoSchool.DataAccess.DatabaseContexts.Interfaces;

namespace TangoSchool.DataAccess.DatabaseContexts;

internal sealed class ReadOnlyTangoSchoolDbContext : TangoSchoolDbContext, IReadOnlyTangoSchoolDbContext
{
    public ReadOnlyTangoSchoolDbContext(DbContextOptions<TangoSchoolDbContext> options)
        : base(options)
    { }

    public override int SaveChanges()
    {
        throw new InvalidOperationException("This context is read-only.");
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        throw new InvalidOperationException("This context is read-only.");
    }

    public override Task<int> SaveChangesAsync
    (
        CancellationToken cancellationToken = default
    )
    {
        throw new InvalidOperationException("This context is read-only.");
    }

    public override Task<int> SaveChangesAsync
    (
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default
    )
    {
        throw new InvalidOperationException("This context is read-only.");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        base.OnConfiguring(optionsBuilder);
    }
}
