using AutoMapper;
using Cinema.DTOs;
using Cinema.Entities;
using Cinema.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Cinema.Controllers
{
    [ApiController]
    [Route("api/actores")]
    public class ActoresController : CustomBaseController
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly ISaveFile saveFile;
        private readonly string container = "actors";

        public ActoresController(ApplicationDbContext context,
            IMapper mapper,
            ISaveFile saveFile
            )
            : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
            this.saveFile = saveFile;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            return await Get<Actor, ActorDTO>(paginationDTO);
        }

        [HttpGet("{id}", Name = "getActor")]
        public async Task<ActionResult<ActorDTO>> Get(int id)
        {
            return await Get<Actor, ActorDTO>(id);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ActorCreateDTO actorCreateDTO)
        {
            var entity = mapper.Map<Actor>(actorCreateDTO);

            if (actorCreateDTO.Photo != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await actorCreateDTO.Photo.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(actorCreateDTO.Photo.FileName);
                    entity.Photo = await saveFile.SaveFile(content, extension, container,
                        actorCreateDTO.Photo.ContentType);
                }
            }

            context.Add(entity);
            await context.SaveChangesAsync();
            var dto = mapper.Map<ActorDTO>(entity);
            return new CreatedAtRouteResult("obtenerActor", new { id = entity.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] ActorCreateDTO actorCreateDTO)
        {
            Actor actorDB = await context.Actores.FirstOrDefaultAsync(x => x.Id == id);

            if (actorDB == null) { return NotFound(); }

            actorDB = mapper.Map(actorCreateDTO, actorDB);

            if (actorCreateDTO.Photo != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await actorCreateDTO.Photo.CopyToAsync(memoryStream);
                    byte[] content = memoryStream.ToArray();
                    string extension = Path.GetExtension(actorCreateDTO.Photo.FileName);
                    actorDB.Photo = await saveFile.EditFile(content, extension, container,
                        actorDB.Photo,
                        actorCreateDTO.Photo.ContentType);
                }
            }

            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<ActorPatchDTO> patchDocument)
        {
            return await Patch<Actor, ActorPatchDTO>(id, patchDocument);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<Actor>(id);
        }
    }
}
