using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PARCIAL_3_DPWA.Models;
using PARCIAL_3_DPWA.Models.ViewModel.GradoAcademico;

namespace PARCIAL_3_DPWA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradoAcademicoUsuarioController : ControllerBase
    {
        private readonly railwayContext _context;

        public GradoAcademicoUsuarioController(railwayContext context)
        {
            _context = context;
        }

        // GET: api/GradoAcademicoUsuario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GradoAcademicoByUsuario>>> GetGradoAcademicoByUsuarios()
        {
            if (_context.GradoAcademicoByUsuarios == null)
            {
                return NotFound();
            }
            // Sacando datos ordenados
            var GradoAcademicoByUsuario = await (from gaU in _context.GradoAcademicoByUsuarios
                                      orderby gaU.Id_usuario ascending
                                      select gaU).ToListAsync();

            List<GradoAcademicoModel> ListaGradoAcademicoModel = new List<GradoAcademicoModel>();
            foreach (GradoAcademicoByUsuario gradoAcademico in GradoAcademicoByUsuario)
            {
                //Obteniendo usuario id
                var usuarioU_name = ObtenerU_nameUsuario(gradoAcademico.Id_usuario ?? default(int)).Result;

                // Verificando que hayan dados en la lista
                if (ListaGradoAcademicoModel.Count != 0)
                {
                    // Verificando que no se repitan
                    if (ListaGradoAcademicoModel.LastOrDefault().U_name.Equals(usuarioU_name))
                    {
                        continue;
                    };
                }

                // Obteniendo Objeto
                var GradoAcademicoData = await (from gradU in _context.GradoAcademicoByUsuarios
                                            where gradU.Id_usuario == gradoAcademico.Id_usuario
                                            select new PutGradoAcademicoModel
                                            {
                                                Profesion = gradU.Profesion,
                                                Universidad = gradU.Universidad,
                                                Objetivos = gradU.Objetivos
                                            }).ToListAsync();

                //Uniendo
                GradoAcademicoModel GradoAcademicoModel = new GradoAcademicoModel
                {
                    U_name = usuarioU_name,
                    GradoAcademico = GradoAcademicoData
                };

                ListaGradoAcademicoModel.Add(GradoAcademicoModel);

            }

            return Ok(ListaGradoAcademicoModel);
        }

        // GET: api/GradoAcademicoUsuario/5
        [HttpGet("{u_name}")]
        public async Task<ActionResult<GradoAcademicoByUsuario>> GetGradoAcademicoByUsuario(String u_name)
        {
            //Obteniendo usuario id
            var usuarioId = ObtenerIdUsuario(u_name).Result;

            var gradoAcademicoUsuario = ObtenerObjetoGradoAcademicoByUsuarios(usuarioId).Result;

            if (gradoAcademicoUsuario == null)
            {
                return NotFound($"El usuario {u_name} no tiene grado academico 😓");
            }

            // Obteniendo Objeto
            var GradoAcademicoData = await (from gradU in _context.GradoAcademicoByUsuarios
                                            where gradU.Id_usuario == usuarioId
                                            select new PutGradoAcademicoModel
                                            {
                                                Profesion = gradU.Profesion,
                                                Universidad = gradU.Universidad,
                                                Objetivos = gradU.Objetivos
                                            }).ToListAsync();

            //Uniendo
            GradoAcademicoModel GradoAcademicoModel = new GradoAcademicoModel
            {
                U_name = u_name,
                GradoAcademico = GradoAcademicoData
            };

            if (GradoAcademicoModel == null)
            {
                return NotFound($"El usuario {u_name} no tiene grado academico 😓");
            }

            return Ok(GradoAcademicoModel);
    }

        // PUT: api/GradoAcademicoUsuario/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{u_name}/{gradoAcademico}")]
        public async Task<IActionResult> PutGradoAcademicoByUsuario(String u_name, string gradoAcademico, PutGradoAcademicoModel gradoAcademicoData)
        {
            // idUsuario
            var idUsuario = ObtenerIdUsuario(u_name).Result;

            // Extrayecto objeto usuario
            GradoAcademicoByUsuario? gradoAcademicoDb = await (from ga in _context.GradoAcademicoByUsuarios
                                        where ga.Id_usuario == idUsuario && ga.Profesion == gradoAcademico
                                                               select ga).FirstOrDefaultAsync();

            // Modificando el objeto
            gradoAcademicoDb.Profesion = gradoAcademicoData.Profesion;
            gradoAcademicoDb.Universidad = gradoAcademicoData.Universidad;
            gradoAcademicoDb.Objetivos = gradoAcademicoData.Objetivos;

            _context.Entry(gradoAcademicoDb).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GradoAcademicoByUsuarioExists(idUsuario))
                {
                    return NotFound($"El usuario no existe 😓");
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/GradoAcademicoUsuario
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GradoAcademicoByUsuario>> PostGradoAcademicoByUsuario(UnameGradoAcademicoModel gradoAcademicoByUsuario)
        {
            if (_context.GradoAcademicoByUsuarios == null)
            {
                return Problem("Entity set 'railwayContext.GradoAcademicoByUser' es nulo.");
            }

            //Obteniendo usuario id
            var usuarioId = ObtenerIdUsuario(gradoAcademicoByUsuario.U_name).Result;

            //Juntando todo
            GradoAcademicoByUsuario gradoAcademicoByUser= new GradoAcademicoByUsuario
            {
                Profesion = gradoAcademicoByUsuario.Profesion,
                Universidad = gradoAcademicoByUsuario.Universidad,
                Objetivos = gradoAcademicoByUsuario.Objetivos,
                Id_usuario = usuarioId
            };

            _context.GradoAcademicoByUsuarios.Add(gradoAcademicoByUser);
            await _context.SaveChangesAsync();

            return await GetGradoAcademicoByUsuario(gradoAcademicoByUsuario.U_name);
        }

        // DELETE: api/GradoAcademicoUsuario/5
        [HttpDelete("{u_name}/{gradoAcademico}")]
        public async Task<IActionResult> DeleteGradoAcademicoByUsuario(String u_name, String gradoAcademico)
        {
            //Obteniendo usuario id
            var usuarioId = ObtenerIdUsuario(u_name).Result;

            //Obteniendo gradoAcademico id
            var gradoAcademicoId = ObtenerIdGradoAcademico(usuarioId, gradoAcademico).Result;

            if (_context.GradoAcademicoByUsuarios == null)
            {
                return NotFound($"El usuario {u_name} no se encontro 😓");
            }
            var gradoAcademicoByUsuario = await _context.GradoAcademicoByUsuarios.FindAsync(gradoAcademicoId);
            if (gradoAcademicoByUsuario == null)
            {
                return NotFound($"El usuario {u_name} no se encontro 😓");
            }

            _context.GradoAcademicoByUsuarios.Remove(gradoAcademicoByUsuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GradoAcademicoByUsuarioExists(int id)
        {
            return (_context.GradoAcademicoByUsuarios?.Any(e => e.Id_grado_academico_by_usuario == id)).GetValueOrDefault();
        }
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
        private async Task<int> ObtenerIdGradoAcademico(int idUsuario)
        {
            //Obteniendo id gradoAcademico
            return await (from ga in _context.GradoAcademicoByUsuarios
                          where ga.Id_usuario == idUsuario
                          select ga.Id_grado_academico_by_usuario).FirstOrDefaultAsync();
        }

        private async Task<int> ObtenerIdGradoAcademico(int idUsuario, String GradoAcademico)
        {
            //Obteniendo id gradoAcademico
            return await (from ga in _context.GradoAcademicoByUsuarios
                          where ga.Id_usuario == idUsuario && ga.Profesion == GradoAcademico
                          select ga.Id_grado_academico_by_usuario).FirstOrDefaultAsync();
        }
        private async Task<GradoAcademicoByUsuario> ObtenerObjetoGradoAcademicoByUsuarios(int idUsuario)
        {
            //Obteniendo gradoAcademicoByUsuario
            return await (from gaU in _context.GradoAcademicoByUsuarios
                          where gaU.Id_usuario == idUsuario
                          select gaU).FirstOrDefaultAsync();
        }
    }
}
