using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoreCodeCamp.Data;

namespace CoreCodeCamp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampDataController : ControllerBase
    {
        private readonly CampContext _context;

        public CampDataController(CampContext context)
        {
            _context = context;
        }

        // GET: api/CampData
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Camp>>> GetCamps()
        {
            return await _context.Camps.ToListAsync();
        }

        // GET: api/CampData/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Camp>> GetCamp(int id)
        {
            var camp = await _context.Camps.FindAsync(id);

            if (camp == null)
            {
                return NotFound();
            }

            return camp;
        }

        // PUT: api/CampData/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCamp(int id, Camp camp)
        {
            if (id != camp.CampId)
            {
                return BadRequest();
            }

            _context.Entry(camp).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampExists(id))
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

        // POST: api/CampData
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Camp>> PostCamp(Camp camp)
        {
            _context.Camps.Add(camp);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCamp", new { id = camp.CampId }, camp);
        }

        // DELETE: api/CampData/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCamp(int id)
        {
            var camp = await _context.Camps.FindAsync(id);
            if (camp == null)
            {
                return NotFound();
            }

            _context.Camps.Remove(camp);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CampExists(int id)
        {
            return _context.Camps.Any(e => e.CampId == id);
        }
    }
}
