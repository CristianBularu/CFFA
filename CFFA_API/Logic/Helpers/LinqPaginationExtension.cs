using System.Linq;

namespace CFFA_API.Logic.Helpers
{
    public static class LinqPaginationExtension
    {
        public static IQueryable<T> Page<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            return query.Skip(pageNumber * pageSize).Take(pageSize);
        }
    }
}
