using System.Collections.Generic;

namespace Cinema.DTOs
{
    public class MovieDetailDTO : MovieDTO
    {
        public List<GenderDTO> Genders { get; set; }
        public List<ActorMovieDetailDTO> Actors { get; set; }
    }
}
