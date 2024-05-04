using Microsoft.Extensions.DependencyInjection;
using TangoSchool.ApplicationServices.Services;
using TangoSchool.ApplicationServices.Services.Interfaces;

namespace TangoSchool.ApplicationServices;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices
    (
        this IServiceCollection services
    )
        => services
            .AddScoped<IIdentityService, IdentityService>()
            .AddScoped<IAuditLogService, AuditLogService>()
            .AddScoped<IClassroomsService, ClassroomsService>()
            .AddScoped<ISubscriptionsService, SubscriptionsService>()
            .AddScoped<ILessonsService, LessonsService>()
            .AddScoped<ILessonRequestsService, LessonRequestsService>()
            .AddScoped<ISubscriptionTemplatesService, SubscriptionTemplatesService>()
            .AddScoped<IEventService, EventService>()
            .AddScoped<IGroupsService, GroupsService>()
            .AddScoped<IEmailSender, EmailSender>()
            .AddScoped<ITeacherService, TeacherService>()
            .AddScoped<IStudentService, StudentService>();
}
