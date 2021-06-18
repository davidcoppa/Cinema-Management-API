using System.Collections.Generic;

namespace Cinema.DTOs
{
    public class MovieIndexDTO
    {
        public List<MovieDTO> OnComming { get; set; }
        public List<MovieDTO> OnCinemas { get; set; }
    }
}
