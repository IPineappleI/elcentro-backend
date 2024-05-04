using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TangoSchool.DataAccess.DatabaseContexts.Interfaces;
using TangoSchool.DataAccess.Entities;

namespace TangoSchool.DataAccess.DatabaseContexts;

internal class TangoSchoolDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>, IUnitOfWork
{
    public TangoSchoolDbContext(DbContextOptions<TangoSchoolDbContext> options)
        : base(options)
    {
    }

    public DbSet<AuditLog> AuditLogs { get; set; } = null!;

    public DbSet<Classroom> Classrooms { get; set; } = null!;

    public DbSet<Group> Groups { get; set; } = null!;

    public DbSet<Lesson> Lessons { get; set; } = null!;

    public DbSet<LessonRequest> LessonRequests { get; set; } = null!;

    public DbSet<Student> Students { get; set; } = null!;

    public DbSet<StudentGroup> StudentGroups { get; set; } = null!;

    public DbSet<Subscription> Subscriptions { get; set; } = null!;
    
    public DbSet<LessonSubscription> LessonSubscriptions { get; set; } = null!;

    public DbSet<SubscriptionTemplate> SubscriptionTemplates { get; set; } = null!;

    public DbSet<Teacher> Teachers { get; set; } = null!;

    public DbSet<Administrator> Administrators { get; set; } = null!;
    
    public DbSet<Event> Events { get; set; } = null!;

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return await base.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        await base.Database.CommitTransactionAsync(cancellationToken);
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        await base.Database.RollbackTransactionAsync(cancellationToken);
    }
}
