using AutoMapper;
using Cinema.DTOs;
using Cinema.Entities;
using Cinema.Helpers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Controllers
{
    public class CustomBaseController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public CustomBaseController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        protected async Task<List<TDTO>> Get<TEntidad, TDTO>() where TEntidad : class
        {
            List<TEntidad> entities = await context.Set<TEntidad>().AsNoTracking().ToListAsync();
            List<TDTO> dtos = mapper.Map<List<TDTO>>(entities);
            return dtos;
        }

        protected async Task<List<TDTO>> Get<TEntidad, TDTO>(PaginationDTO paginationDTO)
            where TEntidad : class
        {
            IQueryable<TEntidad> queryable = context.Set<TEntidad>().AsQueryable();
            return await Get<TEntidad, TDTO>(paginationDTO, queryable);
        }

        protected async Task<List<TDTO>> Get<TEntidad, TDTO>(PaginationDTO paginationDTO,
            IQueryable<TEntidad> queryable)
            where TEntidad : class
        {
            await HttpContext.PaginationParameters(queryable, paginationDTO.RegisterPerPage);
            List<TEntidad> entidades = await queryable.Paginate(paginationDTO).ToListAsync();
            return mapper.Map<List<TDTO>>(entidades);
        }

        protected async Task<ActionResult<TDTO>> Get<TEntidad, TDTO>(int id) where TEntidad : class, IId
        {
            TEntidad entity = await context.Set<TEntidad>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return NotFound();
            }

            return mapper.Map<TDTO>(entity);
        }

        protected async Task<ActionResult> Post<TCreacion, TEntidad, TLectura>
            (TCreacion createDTO, string routName) where TEntidad : class, IId
        {
            TEntidad entities = mapper.Map<TEntidad>(createDTO);
            context.Add(entities);
            await context.SaveChangesAsync();
            var dtoRead = mapper.Map<TLectura>(entities);

            return new CreatedAtRouteResult(routName, new { id = entities.Id }, dtoRead);
        }

        protected async Task<ActionResult> Put<TCreacion, TEntidad>
            (int id, TCreacion createDTO) where TEntidad : class, IId
        {
            TEntidad entity = mapper.Map<TEntidad>(createDTO);
            entity.Id = id;
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        protected async Task<ActionResult> Delete<TEntity>(int id) where TEntity : class, IId, new()
        {
            bool exists = await context.Set<TEntity>().AnyAsync(x => x.Id == id);
            if (!exists)
            {
                return NotFound();
            }

            context.Remove(new TEntity() { Id = id });
            await context.SaveChangesAsync();

            return NoContent();
        }

        protected async Task<ActionResult> Patch<TEntity, TDTO>(int id, JsonPatchDocument<TDTO> patchDocument)
            where TDTO : class
            where TEntity : class, IId
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            TEntity TEntityDB = await context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);

            if (TEntityDB == null)
            {
                return NotFound();
            }

            TDTO dto = mapper.Map<TDTO>(TEntityDB);

            patchDocument.ApplyTo(dto, ModelState);

            bool isValid = TryValidateModel(dto);

            if (!isValid)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(dto, TEntityDB);

            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
