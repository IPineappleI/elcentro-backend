using TangoSchool.DataAccess.Entities;

namespace TangoSchool.DataAccess.Repositories.Interfaces;

public interface ILessonsRepository : IRepositoryBase<Lesson>
{
    void AddLessonSubscriptions(IReadOnlyCollection<LessonSubscription> lessonSubscriptions);
}
