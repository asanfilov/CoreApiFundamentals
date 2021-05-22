using System;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using CoreCodeCamp.Data;
using CoreCodeCamp.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace CoreCodeCamp.Controllers
{
    [Route( "api/[controller]" )]
    [ApiController]
    public class CampsController : ControllerBase
    {
        private readonly ICampRepository repository;
        private readonly IMapper mapper;
        private readonly LinkGenerator linkGenerator;

        public CampsController(ICampRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<CampModel[]>> Get(bool includeTalks = false)
        {
            try
            {
                var camps = await repository.GetAllCampsAsync( includeTalks );
                return mapper.Map<CampModel[]>( camps );
            }
            catch (System.Exception ex)
            {
                return this.StatusCode( StatusCodes.Status500InternalServerError, "Database failure" );
            }
        }

        [HttpGet( "{moniker}" )]
        public async Task<ActionResult<CampModel>> Get(string moniker)
        {
            try
            {
                var camp = await repository.GetCampAsync( moniker );
                if (camp == null) return NotFound();

                return mapper.Map<CampModel>( camp );
            }
            catch (Exception ex)
            {
                return StatusCode( StatusCodes.Status500InternalServerError, "Database failure" );
            }
        }

        [HttpGet( "search" )]
        public async Task<ActionResult<CampModel[]>> SearchByDate(DateTime theDate, bool includeTalks = false)
        {
            try
            {
                var camps = await repository.GetAllCampsByEventDate( theDate, includeTalks );
                if (!camps.Any()) return NotFound();

                return mapper.Map<CampModel[]>( camps );
            }
            catch (Exception ex)
            {
                return StatusCode( StatusCodes.Status500InternalServerError, "Database failure" );
            }
        }

        public async Task<ActionResult<CampModel>> Create(CampModel campModel)
        {
            try
            {
                var existing = await repository.GetCampAsync( campModel.Moniker );
                if (existing != null) return BadRequest( "Moniker is in use" );

                string createdAt = linkGenerator.GetPathByAction( "GET", "camps",
                                                                new { moniker = campModel.Moniker } );
                if (string.IsNullOrWhiteSpace( createdAt )) return BadRequest( "Could not use current moniker" );

                var camp = mapper.Map<Camp>( campModel );
                repository.Add( camp );
                if (await repository.SaveChangesAsync()) return Created( createdAt, mapper.Map<CampModel>( camp ) );
            }
            catch (Exception ex)
            {
                return StatusCode( StatusCodes.Status500InternalServerError, "Database failure" );
            }

            return BadRequest();
        }
            }
            return BadRequest();
        }
    }
}
