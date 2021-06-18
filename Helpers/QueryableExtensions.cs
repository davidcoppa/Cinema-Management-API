using Cinema.DTOs;
using System.Linq;

namespace Cinema.Helpers
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationDTO paginationDTO)
        {
            return queryable
                .Skip((paginationDTO.Page - 1) * paginationDTO.RegisterPerPage)
                .Take(paginationDTO.RegisterPerPage);
        }
    }
}
