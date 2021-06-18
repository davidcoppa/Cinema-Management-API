using Cinema.Validations;
using Microsoft.AspNetCore.Http;

namespace Cinema.DTOs
{
    public class ActorCreateDTO : ActorPatchDTO
    {
        [FileSizeValidation(MaxFileSizeMB: 4)]
        [FileTypeValidation(fileType: FileType.Image)]
        public IFormFile Photo { get; set; }
    }
}
