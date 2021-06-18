using AutoMapper;
using Cinema.DTOs;
using Cinema.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinema.Controllers
{
    [ApiController]
    [Route("api/genders")]
    public class GenerosController : CustomBaseController
    {
        public GenerosController(ApplicationDbContext context,
            IMapper mapper)
            : base(context, mapper)
        {
        }

        [HttpGet]
        public async Task<ActionResult<List<GenderDTO>>> Get()
        {
            return await Get<Gender, GenderDTO>();
        }

        [HttpGet("{id:int}", Name = "getGender")]
        public async Task<ActionResult<GenderDTO>> Get(int id)
        {
            return await Get<Gender, GenderDTO>(id);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GenderCreationDTO genderCreationDTO)
        {
            return await Post<GenderCreationDTO, Gender, GenderDTO>(genderCreationDTO, "getGender");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] GenderCreationDTO genderCreationDTO)
        {
            return await Put<GenderCreationDTO, Gender>(id, genderCreationDTO);
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<Gender>(id);
        }
    }
}
