using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Entities
{
    public class Movie : IId
    {
        public int Id { get; set; }
        [Required]
        [StringLength(300)]
        public string Tittle { get; set; }
        public bool OnCinema { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Poster { get; set; }
        public List<MovieByActors> MovieByActors { get; set; }
        public List<MoviesByGender> MoviesByGender { get; set; }
        public List<MovieCinema> MovieCinema { get; set; }
    }
}
