using TangoSchool.DataAccess.Entities.Interfaces;

namespace TangoSchool.ApplicationServices.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int itemPerPage, int page)
    {
        return query
            .Skip(itemPerPage * page)
            .Take(itemPerPage);
    }

    public static IQueryable<T> FilterActive<T>(this IQueryable<T> query) where T : IPersistent
    {
        return query.Where(x => !x.Terminated);
    }
}
