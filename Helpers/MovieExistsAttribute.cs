using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Cinema.Helpers
{
    public class MovieExistsAttribute : Attribute, IAsyncResourceFilter
    {
        private readonly ApplicationDbContext Dbcontext;

        public MovieExistsAttribute(ApplicationDbContext context)
        {
            Dbcontext = context;
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            object MovieObject = context.HttpContext.Request.RouteValues["MovieId"];

            if (MovieObject == null)
            {
                return;
            }

            int MovieId = int.Parse(MovieObject.ToString());

            bool existsMovie = await Dbcontext.Movies.AnyAsync(x => x.Id == MovieId);

            if (!existsMovie)
            {
                context.Result = new NotFoundResult();
            }
            else
            {
                await next();
            }
        }
    }
}
