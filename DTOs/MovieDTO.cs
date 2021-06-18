using System;

namespace Cinema.DTOs
{
    public class MovieDTO
    {
        public int Id { get; set; }
        public string Tittle { get; set; }
        public bool OnCinemas { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Poster { get; set; }
    }
}
