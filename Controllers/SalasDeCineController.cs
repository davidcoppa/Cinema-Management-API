using AutoMapper;
using Cinema.DTOs;
using Cinema.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Controllers
{
    [Route("api/SalasDeCine")]
    [ApiController]
    public class SalasDeCineController : CustomBaseController
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly GeometryFactory geometryFactory;

        public SalasDeCineController(ApplicationDbContext context,
            IMapper mapper,
            GeometryFactory geometryFactory)
            : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
            this.geometryFactory = geometryFactory;
        }

        [HttpGet]
        public async Task<ActionResult<List<CinemaRoomDTO>>> Get()
        {
            return await Get<CinemaRoom, CinemaRoomDTO>();
        }

        [HttpGet("{id:int}", Name = "getCinemaRoom")]
        public async Task<ActionResult<CinemaRoomDTO>> Get(int id)
        {
            return await Get<CinemaRoom, CinemaRoomDTO>(id);
        }

        [HttpGet("closer")]
        public async Task<ActionResult<List<CinemaRoomCreateDTO>>> Closer(
            [FromQuery] CinemaRoomCloserFilterDTO filtro)
        {
            var userLocation = geometryFactory.CreatePoint(new Coordinate(filtro.Longitude, filtro.Latitude));

            var room = await context.CinemaRooms
                .OrderBy(x => x.Location.Distance(userLocation))
                .Where(x => x.Location.IsWithinDistance(userLocation, filtro.DistanceKM * 1000))
                .Select(x => new CinemaRoomCreateDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Latitude = x.Location.Y,
                    Longitude = x.Location.X,
                    DistanceMtrs = Math.Round(x.Location.Distance(userLocation))
                })
                .ToListAsync();

            return room;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CinemaRoomCreate cinemaRoomCreateDTO)
        {
            return await
                Post<CinemaRoomCreate, CinemaRoom, CinemaRoomDTO>(cinemaRoomCreateDTO, "getCinemaRoom");
        }

        [HttpPost("{id}/addMovie/{movieId}")]
        public async Task<ActionResult> AgregarPelicula(int id, int movieId)
        {
            var movieCinema = new MovieCinema() { MovieId = movieId, CinemaRoomId = id };
            context.Add(movieCinema);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] CinemaRoomCreate cinemaRoomCreate)
        {
            return await Put<CinemaRoomCreate, CinemaRoom>(id, cinemaRoomCreate);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<CinemaRoom>(id);
        }
    }
}
