using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using CoreCodeCamp.Data;
using CoreCodeCamp.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreCodeCamp.Controllers
{
    [Route( "api/[controller]" )]
    [ApiController]
    public class CodeCampsController : ControllerBase
    {
        private readonly CampContext _context;
        private readonly IMapper mapper;

        public CodeCampsController(CampContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // GET: api/CodeCamps
        [HttpGet]
        public async Task<ActionResult<CampModel[]>> GetCamps()
        {
            var camps = await _context.Camps.ToListAsync();
            return mapper.Map<CampModel[]>( camps );
        }

        // GET: api/CodeCamps/5
        [HttpGet( "{id}" )]
        public async Task<ActionResult<CampModel>> GetCamp(int id)
        {
            var camp = await _context.Camps.FindAsync( id );

            if (camp == null)
            {
                return NotFound();
            }

            return mapper.Map<CampModel>( camp );
        }

        // PUT: api/CodeCamps/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut( "{id}" )]
        public async Task<IActionResult> PutCamp(int id, Camp camp)
        {
            if (id != camp.CampId)
            {
                return BadRequest();
            }

            _context.Entry( camp ).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampExists( id ))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CodeCamps
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Camp>> PostCamp(Camp camp)
        {
            _context.Camps.Add( camp );
            await _context.SaveChangesAsync();

            return CreatedAtAction( "GetCamp", new { id = camp.CampId }, camp );
        }

        // DELETE: api/CodeCamps/5
        [HttpDelete( "{id}" )]
        public async Task<IActionResult> DeleteCamp(int id)
        {
            var camp = await _context.Camps.FindAsync( id );
            if (camp == null)
            {
                return NotFound();
            }

            _context.Camps.Remove( camp );
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CampExists(int id)
        {
            return _context.Camps.Any( e => e.CampId == id );
        }
    }
}
