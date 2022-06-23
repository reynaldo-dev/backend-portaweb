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
    public class CertificacionController : ControllerBase
    {
        private readonly railwayContext _context;

        public CertificacionController(railwayContext context)
        {
            _context = context;
        }

        // GET: api/Certificacion
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Certificacion>>> GetCertificacions()
        {
          if (_context.Certificacions == null)
          {
              return NotFound();
          }
            return await _context.Certificacions.ToListAsync();
        }

        // GET: api/Certificacion/5
        [HttpGet("{certificacion}")]
        public async Task<ActionResult<Certificacion>> GetCertificacion(String certificacion)
        {
          if (_context.Certificacions == null)
          {
              return NotFound();
          }

            var certificacionData = await _context.Certificacions.FindAsync(ObtenerIdCertificado(certificacion).Result);

            if (certificacionData == null)
            {
                return NotFound();
            }

            return certificacionData;
        }

        // PUT: api/Certificacion/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{certificacion}")]
        public async Task<IActionResult> PutCertificacion(String certificacion, Certificacion certificacionData)
        {
            var id = ObtenerIdCertificado(certificacion).Result;

            certificacionData.Id_certificacion = id;

            _context.Entry(certificacionData).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CertificacionExists(id))
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

        // POST: api/Certificacion
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Certificacion>> PostCertificacion(Certificacion certificacion)
        {
          if (_context.Certificacions == null)
          {
              return Problem("Entity set 'railwayContext.Certificacions'  is null.");
          }
            _context.Certificacions.Add(certificacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCertificacion", new { certificacion = certificacion.Nombre }, certificacion);
        }

        // DELETE: api/Certificacion/5
        [HttpDelete("{certificacion}")]
        public async Task<IActionResult> DeleteCertificacion(String certificacion)
        {
            var id = ObtenerIdCertificado(certificacion).Result;

            if (_context.Certificacions == null)
            {
                return NotFound();
            }
            var certificacionData = await _context.Certificacions.FindAsync(id);

            if (certificacionData == null)
            {
                return NotFound();
            }

            _context.Certificacions.Remove(certificacionData);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CertificacionExists(int id)
        {
            return (_context.Certificacions?.Any(e => e.Id_certificacion == id)).GetValueOrDefault();
        }

        private async Task<int> ObtenerIdCertificado(String nombreCertificacion)
        {
            //Obteniendo certificado id
            return await (from c in _context.Certificacions
                          where c.Nombre == nombreCertificacion
                          select c.Id_certificacion).FirstOrDefaultAsync();
        }
    }
}
