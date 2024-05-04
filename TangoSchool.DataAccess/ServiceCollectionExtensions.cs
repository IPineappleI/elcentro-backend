using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TangoSchool.DataAccess.DatabaseContexts;
using TangoSchool.DataAccess.DatabaseContexts.Interfaces;
using TangoSchool.DataAccess.Repositories;
using TangoSchool.DataAccess.Repositories.Interfaces;

namespace TangoSchool.DataAccess;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataAccess
    (
        this IServiceCollection services,
        string connectionString
    )
        => services
            .AddDbContext<TangoSchoolDbContext>(opt => opt.UseNpgsql(connectionString))
            .AddDbContext<ReadOnlyTangoSchoolDbContext>(opt => opt.UseNpgsql(connectionString))
            .AddScoped<IReadOnlyTangoSchoolDbContext, ReadOnlyTangoSchoolDbContext>()
            .AddScoped<ITangoSchoolDbContextMigrator, TangoSchoolDbContextMigrator>()
            .AddScoped<IStudentsRepository, StudentsRepository>()
            .AddScoped<IClassroomsRepository, ClassroomsRepository>()
            .AddScoped<IEventRepository, EventRepository>()
            .AddScoped<IGroupsRepository, GroupsRepository>()
            .AddScoped<ISubscriptionsRepository, SubscriptionsRepository>()
            .AddScoped<ILessonsRepository, LessonsRepository>()
            .AddScoped<ILessonRequestsRepository, LessonRequestsRepository>()
            .AddScoped<ISubscriptionTemplatesRepository, SubscriptionTemplatesRepository>()
            .AddScoped<ITeachersRepository, TeachersRepository>()
            .AddScoped<IAdministratorsRepository, AdministratorsRepository>()
            .AddScoped<IAuditLogsRepository, AuditLogsRepository>();

    public static IdentityBuilder AddDataAccess
    (
        this IdentityBuilder builder
    )
        => builder.AddEntityFrameworkStores<TangoSchoolDbContext>();
}
