using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PARCIAL_3_DPWA.Models;

namespace PARCIAL_3_DPWA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedesController : ControllerBase
    {
        private readonly railwayContext _context;

        public RedesController(railwayContext context)
        {
            _context = context;
        }

        // GET: api/Redes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Red>>> GetReds()
        {
          if (_context.Reds == null)
          {
              return NotFound();
          }
            return await _context.Reds.ToListAsync();
        }

        // GET: api/Redes/5
        [HttpGet("{red}")]
        public async Task<ActionResult<Red>> GetRed(String red)
        {
          if (_context.Reds == null)
          {
              return NotFound();
          }
            var redData = await _context.Reds.FindAsync(ObtenerIdRed(red).Result);

            if (redData == null)
            {
                return NotFound();
            }

            return redData;
        }

        // PUT: api/Redes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{red}")]
        public async Task<IActionResult> PutRed(String red, Red redData)
        {
            var id = ObtenerIdRed(red).Result;

            redData.Id_red = id;

            _context.Entry(redData).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RedExists(id))
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

        // POST: api/Redes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Red>> PostRed(Red red)
        {
          if (_context.Reds == null)
          {
              return Problem("Entity set 'railwayContext.Reds'  is null.");
          }
            _context.Reds.Add(red);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRed", new { red = red.Nombre }, red);
        }

        // DELETE: api/Redes/5
        [HttpDelete("{red}")]
        public async Task<IActionResult> DeleteRed(String red)
        {
            if (_context.Reds == null)
            {
                return NotFound();
            }

            var redData = await _context.Reds.FindAsync(ObtenerIdRed(red).Result);
            if (redData == null)
            {
                return NotFound();
            }

            _context.Reds.Remove(redData);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RedExists(int id)
        {
            return (_context.Reds?.Any(e => e.Id_red == id)).GetValueOrDefault();
        }

        private async Task<int> ObtenerIdRed(String nombreRed)
        {
            //Obteniendo certificado id
            return await (from r in _context.Reds
                          where r.Nombre == nombreRed
                          select r.Id_red).FirstOrDefaultAsync();
        }
    }
}
