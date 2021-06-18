using System;
using System.ComponentModel.DataAnnotations;

namespace Cinema.DTOs
{
    public class MoviePatchDTO
    {
        [Required]
        [StringLength(300)]
        public string Tittle { get; set; }
        public bool OnCinemas { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
