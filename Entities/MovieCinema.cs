﻿
namespace Cinema.Entities
{
    public class MovieCinema
    {
        public int MovieId { get; set; }
        public int CinemaRoomId { get; set; }
        public Movie Movie { get; set; }
        public CinemaRoom CinemaRoom { get; set; }
    }
}
