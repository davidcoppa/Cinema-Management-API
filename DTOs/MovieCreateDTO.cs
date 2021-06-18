using Cinema.Helpers;
using Cinema.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Cinema.DTOs
{
    public class MovieCreateDTO : MoviePatchDTO
    {
        [FileSizeValidation(MaxFileSizeMB: 4)]
        [FileTypeValidation(FileType.Image)]
        public IFormFile Poster { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> GendersIDs { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<ActorMovieCreateDTO>>))]
        public List<ActorMovieCreateDTO> Actors { get; set; }
    }
}
