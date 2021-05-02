using System.Threading.Tasks;

using CoreCodeCamp.Data;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreCodeCamp.Controllers
{
    [Route( "api/[controller]" )]
    public class CampsController : ControllerBase
    {
        private readonly ICampRepository repository;

        public CampsController(ICampRepository repo)
        {
            this.repository = repo;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var camps = await this.repository.GetAllCampsAsync();
                return Ok( camps );
            }
            catch (System.Exception)
            {
                return this.StatusCode( StatusCodes.Status500InternalServerError, "Database failure" );
            }
        }
    }
}
