using AutoMapper;
using Cinema.DTOs;
using Cinema.Entities;
using Cinema.Helpers;
using Cinema.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Cinema.Controllers
{
    [ApiController]
    [Route("api/movies")]
    public class PeliculasController : CustomBaseController
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly ISaveFile saveFile;
        private readonly ILogger<PeliculasController> logger;
        private readonly string container = "movies";

        public PeliculasController(ApplicationDbContext context,
            IMapper mapper,
            ISaveFile saveFile,
            ILogger<PeliculasController> logger)
            : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
            this.saveFile = saveFile;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<MovieIndexDTO>> Get()
        {
            var top = 5;
            var hoy = DateTime.Today;

            var oncomming = await context.Movies
                .Where(x => x.ReleaseDate > hoy)
                .OrderBy(x => x.ReleaseDate)
                .Take(top)
                .ToListAsync();

            var OnCinemas = await context.Movies
                .Where(x => x.OnCinema)
                .Take(top)
                .ToListAsync();

            var result = new MovieIndexDTO();
            result.OnComming = mapper.Map<List<MovieDTO>>(oncomming);
            result.OnCinemas = mapper.Map<List<MovieDTO>>(OnCinemas);
            return result;
        }

        [HttpGet("filter")]
        public async Task<ActionResult<List<MovieDTO>>> Filtrar([FromQuery] MovieFilter filterMovieDTO)
        {
            var moviesQueryable = context.Movies.AsQueryable();

            if (!string.IsNullOrEmpty(filterMovieDTO.Tittle))
            {
                moviesQueryable = moviesQueryable.Where(x => x.Tittle.Contains(filterMovieDTO.Tittle));
            }

            if (filterMovieDTO.OnCinemas)
            {
                moviesQueryable = moviesQueryable.Where(x => x.OnCinema);
            }

            if (filterMovieDTO.OnComming)
            {
                var hoy = DateTime.Today;
                moviesQueryable = moviesQueryable.Where(x => x.ReleaseDate > hoy);
            }

            if (filterMovieDTO.GenderId != 0)
            {
                moviesQueryable = moviesQueryable
                    .Where(x => x.MoviesByGender.Select(y => y.GenderId)
                    .Contains(filterMovieDTO.GenderId));
            }

            if (!string.IsNullOrEmpty(filterMovieDTO.OrderBy))
            {
                var tipoOrden = filterMovieDTO.OrderAsc ? "ascending" : "descending";

                try
                {
                    moviesQueryable = moviesQueryable.OrderBy($"{filterMovieDTO.OrderBy} {tipoOrden}");

                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message, ex);
                }
            }

            await HttpContext.PaginationParameters(moviesQueryable,
                filterMovieDTO.RegisterPerPage);

            var movies = await moviesQueryable.Paginate(filterMovieDTO.Pagination).ToListAsync();

            return mapper.Map<List<MovieDTO>>(movies);
        }

        [HttpGet("{id}", Name = "getMovies")]
        public async Task<ActionResult<MovieDetailDTO>> Get(int id)
        {
            var movie = await context.Movies
                .Include(x => x.MovieByActors).ThenInclude(x => x.Actor)
                .Include(x => x.MoviesByGender).ThenInclude(x => x.Gender)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            movie.MovieByActors = movie.MovieByActors.OrderBy(x => x.Order).ToList();

            return mapper.Map<MovieDetailDTO>(movie);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] MovieCreateDTO movieCreateDTO)
        {
            var movie = mapper.Map<Movie>(movieCreateDTO);

            if (movieCreateDTO.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await movieCreateDTO.Poster.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(movieCreateDTO.Poster.FileName);
                    movie.Poster = await saveFile.SaveFile(content, extension, container,
                        movieCreateDTO.Poster.ContentType);
                }
            }

            SetActorsOrder(movie);
            context.Add(movie);
            await context.SaveChangesAsync();
            var movieDTO = mapper.Map<MovieDTO>(movie);
            return new CreatedAtRouteResult("getMovies", new { id = movie.Id }, movieDTO);
        }

        private void SetActorsOrder(Movie movie)
        {
            if (movie.MovieByActors != null)
            {
                for (int i = 0; i < movie.MovieByActors.Count; i++)
                {
                    movie.MovieByActors[i].Order = i;
                }
            }
        }



        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] MovieCreateDTO movieCreateDTO)
        {
            var movieDB = await context.Movies
                .Include(x => x.MovieByActors)
                .Include(x => x.MoviesByGender)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (movieDB == null) { return NotFound(); }

            movieDB = mapper.Map(movieCreateDTO, movieDB);

            if (movieCreateDTO.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await movieCreateDTO.Poster.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(movieCreateDTO.Poster.FileName);
                    movieDB.Poster = await saveFile.EditFile(contenido, extension, container,
                        movieDB.Poster,
                        movieCreateDTO.Poster.ContentType);
                }
            }

            SetActorsOrder(movieDB);

            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id,
            [FromBody] JsonPatchDocument<MoviePatchDTO> patchDocument)
        {
            return await Patch<Movie, MoviePatchDTO>(id, patchDocument);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<Movie>(id);
        }
    }
}
