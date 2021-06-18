using AutoMapper;
using Cinema.DTOs;
using Cinema.Entities;
using Microsoft.AspNetCore.Identity;
using NetTopologySuite.Geometries;
using System.Collections.Generic;

namespace Cinema.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles(GeometryFactory geometryFactory)
        {
            CreateMap<Gender, GenderDTO>().ReverseMap();
            CreateMap<GenderCreationDTO, Gender>();

            CreateMap<Review, ReviewDTO>()
                .ForMember(x => x.UserName, x => x.MapFrom(y => y.User.UserName));

            CreateMap<ReviewDTO, Review>();
            CreateMap<ReviewCreateDTO, Review>();

            CreateMap<IdentityUser, UserDTO>();

            CreateMap<CinemaRoom, CinemaRoomDTO>()
                .ForMember(x => x.Latitude, x => x.MapFrom(y => y.Location.Y))
                .ForMember(x => x.Longitude, x => x.MapFrom(y => y.Location.X));

            CreateMap<CinemaRoomDTO, CinemaRoom>()
                .ForMember(x => x.Location, x => x.MapFrom(y =>
                geometryFactory.CreatePoint(new Coordinate(y.Longitude, y.Latitude))));

            CreateMap<CinemaRoomCreate, CinemaRoom>()
                 .ForMember(x => x.Location, x => x.MapFrom(y =>
                geometryFactory.CreatePoint(new Coordinate(y.Longitude, y.Latitude))));

            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorCreateDTO, Actor>()
                .ForMember(x => x.Photo, options => options.Ignore());
            CreateMap<ActorPatchDTO, Actor>().ReverseMap();

            CreateMap<Movie, MovieDTO>().ReverseMap();
            CreateMap<MovieCreateDTO, Movie>()
                .ForMember(x => x.Poster, options => options.Ignore())
                .ForMember(x => x.MoviesByGender, options => options.MapFrom(MapPeliculasGeneros))
                .ForMember(x => x.MoviesByGender, options => options.MapFrom(MapPeliculasActores));

            CreateMap<Movie, MovieDetailDTO>()
                .ForMember(x => x.Genders, options => options.MapFrom(MapPeliculasGeneros))
                .ForMember(x => x.Actors, options => options.MapFrom(MapPeliculasActores));

            CreateMap<MoviePatchDTO, Movie>().ReverseMap();
        }

        public AutoMapperProfiles()
        {
        }

        private List<ActorMovieDetailDTO> MapPeliculasActores(Movie pelicula, MovieDetailDTO peliculaDetallesDTO)
        {
            var result = new List<ActorMovieDetailDTO>();
            if (pelicula.MovieByActors == null) { return result; }
            foreach (var actorPelicula in pelicula.MovieByActors)
            {
                result.Add(new ActorMovieDetailDTO
                {
                    ActorId = actorPelicula.ActorId,
                    Character = actorPelicula.Character,
                    NameActor = actorPelicula.Actor.Name
                });
            }

            return result;
        }

        private List<GenderDTO> MapPeliculasGeneros(Movie pelicula, MovieDetailDTO peliculaDetallesDTO)
        {
            var result = new List<GenderDTO>();
            if (pelicula.MoviesByGender == null) { return result; }
            foreach (var generoPelicula in pelicula.MoviesByGender)
            {
                result.Add(new GenderDTO() { Id = generoPelicula.GenderId, Name = generoPelicula.Gender.Name });
            }

            return result;
        }

        private List<MoviesByGender> MapPeliculasGeneros(MovieCreateDTO peliculaCreacionDTO, Movie pelicula)
        {
            var result = new List<MoviesByGender>();
            if (peliculaCreacionDTO.GendersIDs == null) { return result; }
            foreach (var id in peliculaCreacionDTO.GendersIDs)
            {
                result.Add(new MoviesByGender() { GenderId = id });
            }

            return result;
        }

        private List<MovieByActors> MapPeliculasActores(MovieCreateDTO peliculaCreacionDTO, Movie pelicula)
        {
            var result = new List<MovieByActors>();
            if (peliculaCreacionDTO.Actors == null) { return result; }

            foreach (var actor in peliculaCreacionDTO.Actors)
            {
                result.Add(new MovieByActors() { ActorId = actor.ActorId, Character = actor.Character });
            }

            return result;
        }
    }
}
