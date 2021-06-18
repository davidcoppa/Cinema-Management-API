using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Helpers
{
    public static class HttpContextExtensions
    {
        public async static Task PaginationParameters<T>(this HttpContext httpContext,
            IQueryable<T> queryable, int registersPerPage)
        {
            double quantity = await queryable.CountAsync();
            double pages = Math.Ceiling(quantity / registersPerPage);
            httpContext.Response.Headers.Add("Pages", pages.ToString());
        }

    }
}
