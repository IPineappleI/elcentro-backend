using Microsoft.EntityFrameworkCore;
using TangoSchool.ApplicationServices.Constants;
using TangoSchool.ApplicationServices.Extensions;
using TangoSchool.ApplicationServices.Mappers;
using TangoSchool.ApplicationServices.Models.Lessons;
using TangoSchool.ApplicationServices.Models.Students;
using TangoSchool.ApplicationServices.Models.Subscriptions;
using TangoSchool.ApplicationServices.Models.SubscriptionTemplates;
using TangoSchool.ApplicationServices.Services.Interfaces;
using TangoSchool.DataAccess.DatabaseContexts.Interfaces;
using TangoSchool.DataAccess.Repositories.Interfaces;

namespace TangoSchool.ApplicationServices.Services;

internal class SubscriptionsService : ISubscriptionsService
{
    private readonly IReadOnlyTangoSchoolDbContext _readOnlyTangoSchoolDbContext;
    private readonly ISubscriptionsRepository _subscriptionsRepository;

    public SubscriptionsService
    (
        IReadOnlyTangoSchoolDbContext readOnlyTangoSchoolDbContext,
        ISubscriptionsRepository subscriptionsRepository
    )
    {
        _readOnlyTangoSchoolDbContext = readOnlyTangoSchoolDbContext;
        _subscriptionsRepository = subscriptionsRepository;
    }

    public async Task<SubscriptionMetadata> GetSubscriptionsMetadata
    (
        CancellationToken cancellationToken
    )
    {
        var students = await _readOnlyTangoSchoolDbContext
            .Students
            .FilterActive()
            .Select(x => new StudentHeader
            (
                x.Id,
                x.ApplicationUser.FirstName,
                x.ApplicationUser.LastName,
                x.ApplicationUser.MiddleName
            ))
            .ToListAsync(cancellationToken);

        var subscriptionTemplates = await _readOnlyTangoSchoolDbContext
            .SubscriptionTemplates
            .Where(x => x.Active)
            .Select(x => new SubscriptionTemplateHeader
            (
                x.Id,
                x.Name,
                x.LessonType,
                x.Price
            ))
            .ToListAsync(cancellationToken);

        return new(students, subscriptionTemplates);
    }

    public async Task<Guid> CreateSubscription(CreateSubscriptionPayload payload, CancellationToken cancellationToken)
    {
        var newSubscription = _subscriptionsRepository.Add(payload.MapToDatabaseSubscription());

        await _subscriptionsRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return newSubscription.Id;
    }

    public async Task UpdateSubscription(UpdateSubscription payload, CancellationToken cancellationToken)
    {
        var subscription = await _readOnlyTangoSchoolDbContext
            .Subscriptions
            .Where(x => x.Id == payload.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (subscription is null)
        {
            throw new ApplicationException(GeneralErrorMessages.SubscriptionWasNotFound);
        }

        subscription.Name = payload.Name;
        subscription.Description = payload.Description;
        subscription.LessonType = payload.LessonType;
        subscription.LessonCount = payload.LessonCount;
        subscription.ExpirationDate = payload.ExpirationDate;
        subscription.Price = payload.Price;
        subscription.StudentId = payload.StudentId;

        _subscriptionsRepository.Update(subscription);

        await _subscriptionsRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<GetSubscriptionResponse> GetSubscription(Guid id, CancellationToken cancellationToken)
    {
        var subscription = await _readOnlyTangoSchoolDbContext
            .Subscriptions
            .Where(x => x.Id == id)
            .Select(x => new GetSubscriptionResponse
            (
                x.Name,
                x.Description,
                x.LessonType,
                x.LessonCount,
                x.ExpirationDate,
                x.Price,
                new
                (
                    x.Student.Id,
                    x.Student.ApplicationUser.FirstName,
                    x.Student.ApplicationUser.LastName,
                    x.Student.ApplicationUser.MiddleName
                ),
                x.AttendedLessons.Select(y =>
                        new LessonHeader
                        (
                            y.Lesson.Id,
                            y.Lesson.Name,
                            y.Lesson.LessonType,
                            y.Lesson.StartTime,
                            y.Lesson.FinishTime
                        ))
                    .ToList()
            ))
            .AsSplitQuery()
            .SingleOrDefaultAsync(cancellationToken);

        if (subscription is null)
        {
            throw new ApplicationException(GeneralErrorMessages.SubscriptionWasNotFound);
        }

        return subscription;
    }
}
