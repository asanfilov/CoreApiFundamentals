using System.Threading.Tasks;

using AutoMapper;

using CoreCodeCamp.Data;
using CoreCodeCamp.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreCodeCamp.Controllers
{
    [Route( "api/[controller]" )]
    public class CampsController : ControllerBase
    {
        private readonly ICampRepository repository;
        private readonly IMapper mapper;

        public CampsController(ICampRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
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
                if (camp == null)
                {
                    return NotFound();
                }
                return mapper.Map<CampModel>( camp );
            }
            catch (System.Exception)
            {
                return this.StatusCode( StatusCodes.Status500InternalServerError, "Database failure" );
            }
        }
    }
}
