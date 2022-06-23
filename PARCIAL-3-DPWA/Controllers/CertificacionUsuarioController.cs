using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PARCIAL_3_DPWA.Models;
using PARCIAL_3_DPWA.Models.ViewModel.Certificacion;
using PARCIAL_3_DPWA.Models.ViewModel.Usuario;

namespace PARCIAL_3_DPWA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificacionUsuarioController : ControllerBase
    {
        private readonly railwayContext _context;

        public CertificacionUsuarioController(railwayContext context)
        {
            _context = context;
        }

        // GET: api/CertificacionUsuario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CertificacionUsuarioModel>>> GetCertificacionByUsuarios()
        {
            if (_context.CertificacionByUsuarios == null)
            {
                return NotFound();
            }

            // Sacando datos ordenados
            var certificacionByUsuario = await (from certiU in _context.CertificacionByUsuarios
                                                orderby certiU.Id_usuario ascending
                                                select certiU).ToListAsync();

            List<CertificacionUsuarioModel> ListaCertificacionUsuarioModel = new List<CertificacionUsuarioModel>();
            foreach (CertificacionByUsuario certificacion in certificacionByUsuario)
            {
                //Obteniendo usuario id
                var usuarioU_name = ObtenerU_nameUsuario(certificacion.Id_usuario ?? default(int)).Result;

                // Verificando que hayan dados en la lista
                if (ListaCertificacionUsuarioModel.Count != 0)
                {
                    // Verificando que no se repitan
                    if (ListaCertificacionUsuarioModel.LastOrDefault().U_name.Equals(usuarioU_name))
                    {
                        continue;
                    };
                }

                // Obteniendo certificaciones de usuario
                var CertificacionUsuarioData = await (from certiU in _context.CertificacionByUsuarios
                                                      join certi in _context.Certificacions on certiU.Id_certificacion equals certi.Id_certificacion
                                                      where certiU.Id_usuario == certificacion.Id_usuario && certiU.Id_certificacion == certi.Id_certificacion
                                                      select new CertificacionModel
                                                      {
                                                          Nombre = certi.Nombre,
                                                          Institucion = certi.Institucion,
                                                          Link = certi.Link,
                                                          Descripcion = certi.Descripcion,
                                                          Objetivos = certi.Obtivos
                                                      }).ToListAsync();

                //Uniendo
                CertificacionUsuarioModel CertificacionUsuarioModel = new CertificacionUsuarioModel
                {
                    U_name = usuarioU_name,
                    Certificacion = CertificacionUsuarioData
                };

                ListaCertificacionUsuarioModel.Add(CertificacionUsuarioModel);

            }

            return Ok(ListaCertificacionUsuarioModel);
        }

        // GET: api/CertificacionUsuario/5
        [HttpGet("{u_name}")]
        public async Task<ActionResult<CertificacionUsuarioModel>> GetCertificacionByUsuario(String u_name)
        {
            if (_context.CertificacionByUsuarios == null)
            {
                return NotFound();
            }
            //Obteniendo usuario id
            var usuarioId = ObtenerIdUsuario(u_name).Result;

            //Obteniendo usuario id
            var usuarioU_name = ObtenerU_nameUsuario(usuarioId).Result;

            // Obteniendo certificaciones de usuario
            var CertificacionUsuarioData = await (from certiU in _context.CertificacionByUsuarios
                                                  join certi in _context.Certificacions on certiU.Id_certificacion equals certi.Id_certificacion
                                                  where certiU.Id_usuario == usuarioId && certiU.Id_certificacion == certi.Id_certificacion
                                                  select new CertificacionModel
                                                  {
                                                      Nombre = certi.Nombre,
                                                      Institucion = certi.Institucion,
                                                      Link = certi.Link,
                                                      Descripcion = certi.Descripcion,
                                                      Objetivos = certi.Obtivos
                                                  }).ToListAsync();

            //Uniendo
            CertificacionUsuarioModel CertificacionUsuarioModel = new CertificacionUsuarioModel
            {
                U_name = usuarioU_name,
                Certificacion = CertificacionUsuarioData
            };


            if (CertificacionUsuarioModel == null)
            {
                return NotFound();
            }
            return Ok(CertificacionUsuarioModel);
        }

        // PUT: api/CertificacionUsuario/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{u_name}/{certificadoAnterior}/{certificadoNuevo}")]
        public async Task<IActionResult> PutCertificacionByUsuario(String u_name, String certificadoAnterior, String certificadoNuevo)
        {
            //Obteniendo usuario id
            var usuarioId = ObtenerIdUsuario(u_name).Result;

            //Obteniendo certificacion anterior id
            var certificacionAnteriorId = ObtenerIdCertificado(certificadoAnterior).Result;

            //Buscando id de CertificacionByUsuarios
            var certificacionByUsuarios = ObtenerObjetoCertificacionByUsuarios(usuarioId, certificacionAnteriorId).Result;

            if (usuarioId != certificacionByUsuarios.Id_usuario)
            {
                return BadRequest();
            }

            //Obteniendo certificacion nueva id
            var certificacionNuevaId = ObtenerIdCertificado(certificadoNuevo).Result;

            //Cambiando Certificado
            certificacionByUsuarios.Id_certificacion = certificacionNuevaId;

            _context.Entry(certificacionByUsuarios).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CertificacionByUsuarioExists(usuarioId))
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

        // POST: api/CertificacionUsuario
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CertificacionUsuarioModel>> PostCertificacionByUsuario(UnameCertificacionModel certificacionUsuario)
        {
            if (_context.CertificacionByUsuarios == null)
            {
                return Problem("Entity set 'railwayContext.CertificacionByUsuarios'  is null.");
            }

            //Obteniendo usuario id
            var usuarioId = ObtenerIdUsuario(certificacionUsuario.U_name).Result;

            //Obteniendo certificacion id
            var certificacionId = ObtenerIdCertificado(certificacionUsuario.nombreCertificacion).Result;

            //Juntando todo
            CertificacionByUsuario certificacionByUsuario = new CertificacionByUsuario
            {
                Id_certificacion = certificacionId,
                Id_usuario = usuarioId
            };

            _context.CertificacionByUsuarios.Add(certificacionByUsuario);
            await _context.SaveChangesAsync();

            return await GetCertificacionByUsuario(certificacionUsuario.U_name);
        }

        // DELETE: api/u_name/certificado
        [HttpDelete("{u_name}/{certificado}")]
        public async Task<IActionResult> DeleteCertificacionByUsuario(String u_name, String certificado)
        {
            //Obteniendo usuario id
            var usuarioId = ObtenerIdUsuario(u_name).Result;

            //Obteniendo certificacion id
            var certificacionId = ObtenerIdCertificado(certificado).Result;

            //Buscando id de CertificacionByUsuarios
            var certificacionByUsuariosId = ObtenerCertificacionByUsuarios(usuarioId, certificacionId).Result;

            if (_context.CertificacionByUsuarios == null)
            {
                return NotFound($"El usuario {u_name} o certificado {certificado} no se encontro 😓");
            }
            var certificacionByUsuario = await _context.CertificacionByUsuarios.FindAsync(certificacionByUsuariosId);
            if (certificacionByUsuario == null)
            {
                return NotFound($"El usuario {u_name} o certificado {certificado} no se encontro 😓");
            }

            _context.CertificacionByUsuarios.Remove(certificacionByUsuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CertificacionByUsuarioExists(int id)
        {
            return (_context.CertificacionByUsuarios?.Any(e => e.Id_certificacion_by_Usuario == id)).GetValueOrDefault();
        }

        // METODOS PARA
        // ACCEDER A DATOS
        // Y OBJETOS
        private async Task<int> ObtenerIdUsuario(String u_name)
        {
            //Obteniendo usuario id
            return await (from u in _context.Usuarios
                          where u.U_name == u_name
                          select u.Id_usuario).FirstOrDefaultAsync();
        }
        private async Task<String> ObtenerU_nameUsuario(int idUsuario)
        {
            //Obteniendo usuario id
            return await (from u in _context.Usuarios
                          where u.Id_usuario == idUsuario
                          select u.U_name).FirstOrDefaultAsync();
        }
        private async Task<Usuario> ObtenerUsuario(String u_name)
        {
            //Obteniendo usuario id
            return await (from u in _context.Usuarios
                          where u.U_name == u_name
                          select u).FirstOrDefaultAsync();
        }
        private async Task<int> ObtenerIdCertificado(String nombreCertificacion)
        {
            //Obteniendo certificado id
            return await (from c in _context.Certificacions
                          where c.Nombre == nombreCertificacion
                          select c.Id_certificacion).FirstOrDefaultAsync();
        }

        private async Task<int> ObtenerCertificacionByUsuarios(int idUsuario, int idCertificado)
        {
            //Obteniendo certificado id
            return await (from cu in _context.CertificacionByUsuarios
                          where cu.Id_usuario == idUsuario && cu.Id_certificacion == idCertificado
                          select cu.Id_certificacion_by_Usuario).FirstOrDefaultAsync();
        }

        private async Task<CertificacionByUsuario> ObtenerObjetoCertificacionByUsuarios(int idUsuario, int idCertificado)
        {
            //Obteniendo certificado id
            return await (from cu in _context.CertificacionByUsuarios
                          where cu.Id_usuario == idUsuario && cu.Id_certificacion == idCertificado
                          select cu).FirstOrDefaultAsync();
        }
    }
}
