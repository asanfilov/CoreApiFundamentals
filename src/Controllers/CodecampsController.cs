using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CoreCodeCamp.Data;
using CoreCodeCamp.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreCodeCamp.Controllers
{
    [Route( "api/[controller]" )]
    [ApiController]
    public class CodecampsController : ControllerBase
    {
        private readonly CampContext _context;

        public CodecampsController(CampContext context)
        {
            _context = context;
        }

        // GET: api/Codecamps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampModel>>> GetCamp()
        {
            return await _context.CampModel.ToListAsync();
        }

        // GET: api/Codecamps/5
        [HttpGet( "{id}" )]
        public async Task<ActionResult<CampModel>> GetCamp(int id)
        {
            var campModel = await _context.CampModel.FindAsync( id );

            if (campModel == null)
            {
                return NotFound();
            }

            return campModel;
        }

        // PUT: api/Codecamps/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut( "{id}" )]
        public async Task<IActionResult> PutCamp(int id, CampModel campModel)
        {
            if (id != campModel.Id)
            {
                return BadRequest();
            }

            _context.Entry( campModel ).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampModelExists( id ))
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

        // POST: api/Codecamps
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CampModel>> PostCamp(CampModel campModel)
        {
            _context.CampModel.Add( campModel );
            await _context.SaveChangesAsync();

            return CreatedAtAction( "GetCampModel", new { id = campModel.Id }, campModel );
        }

        // DELETE: api/Codecamps/5
        [HttpDelete( "{id}" )]
        public async Task<IActionResult> DeleteCamp(int id)
        {
            var campModel = await _context.CampModel.FindAsync( id );
            if (campModel == null)
            {
                return NotFound();
            }

            _context.CampModel.Remove( campModel );
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CampModelExists(int id)
        {
            return _context.CampModel.Any( e => e.Id == id );
        }
    }
}
