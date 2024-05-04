using TangoSchool.DataAccess.DatabaseContexts;
using TangoSchool.DataAccess.Entities;
using TangoSchool.DataAccess.Repositories.Interfaces;

namespace TangoSchool.DataAccess.Repositories;

internal class LessonsRepository : BaseRepository<Lesson>, ILessonsRepository
{
    public LessonsRepository(TangoSchoolDbContext tangoSchoolDbContext) : base(tangoSchoolDbContext)
    {
    }

    public void AddLessonSubscriptions(IReadOnlyCollection<LessonSubscription> lessonSubscriptions)
    {
        Context.LessonSubscriptions.AddRange(lessonSubscriptions);
    }
}
