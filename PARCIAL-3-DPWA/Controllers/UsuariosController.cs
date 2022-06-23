using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PARCIAL_3_DPWA.Models;
using PARCIAL_3_DPWA.Models.ViewModel.Usuario;

namespace PARCIAL_3_DPWA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly railwayContext _context;

        public UsuariosController(railwayContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            if (_context.Usuarios == null)
            {
                return NotFound($"El usuario no existe 😓");
            }
            return await _context.Usuarios.ToListAsync();
        }

        // GET: api/Usuarios/u_name
        [HttpGet("{u_name}")]
        public async Task<ActionResult<UsuarioModel>> GetUsuario(String u_name)
        {
            if (_context.Usuarios == null)
            {
                return NotFound($"El usuario {u_name} no existe 😓");
            }

            //Obteniendo usuario
            var Usuario = await (from u in _context.Usuarios
                                 where u.U_name == u_name
                                 select u).FirstOrDefaultAsync();

            // Obteniendo redes sociales usuario
            var RedesUsuario = await (from redU in _context.RedByUsers
                                      join red in _context.Reds on redU.Id_red equals red.Id_red
                                      where redU.Id_usuario == Usuario.Id_usuario && red.Id_red == redU.Id_red
                                      select new RedesModel
                                      {
                                          Nombre = red.Nombre,
                                          Accesslink = redU.Accesslink
                                      }).ToListAsync();

            // Obteniendo grados academico
            var GradoAcademicoUsuario = await (from gradU in _context.GradoAcademicoByUsuarios
                                      where gradU.Id_usuario == Usuario.Id_usuario
                                      select new GradoAcademicoModel
                                      {
                                          Profesion = gradU.Profesion,
                                          Universidad = gradU.Universidad,
                                          Objetivos = gradU.Objetivos
                                      }).FirstOrDefaultAsync();

            // Obteniendo experiencia de usuario
            var ExperienciaUsuario = await (from expeU in _context.ExperienciaByUsuarios
                                            where expeU.Id_usuario == Usuario.Id_usuario
                                            select new ExperienciaModel
                                            {
                                                Nombre_proyecto = expeU.Nombre_proyecto,
                                                Rol = expeU.Rol,
                                                Resumen = expeU.Resumen,
                                                Responsabilidades = expeU.Responsabilidades,
                                                Tecnologias = expeU.Tecnologias
                                            }).ToListAsync();

            // Obteniendo certificaciones de usuario
            var CertificacionUsuario = await (from certiU in _context.CertificacionByUsuarios
                                              join certi in _context.Certificacions on certiU.Id_certificacion equals certi.Id_certificacion
                                              where certiU.Id_usuario == Usuario.Id_usuario && certiU.Id_certificacion == certi.Id_certificacion
                                              select new CertificacionModel
                                              {
                                                  Nombre = certi.Nombre,
                                                  Institucion = certi.Institucion,
                                                  Link = certi.Link,
                                                  Descripcion = certi.Descripcion,
                                                  Objetivos = certi.Obtivos
                                              }).ToListAsync();

            // Uniendo todo
            UsuarioModel UserModel = new UsuarioModel
            {
                Id_usuario = Usuario.Id_usuario,
                U_name = Usuario.U_name,
                UrlFoto = Usuario.UrlFoto,
                Nombres = Usuario.Nombres,
                Apellidos = Usuario.Apellidos,
                Correo = Usuario.Correo,
                Intro = Usuario.Intro,
                Redes_sociales = RedesUsuario,
                Grado_academico = GradoAcademicoUsuario,
                Certificacion = CertificacionUsuario,
                Experiencia = ExperienciaUsuario
            };

            if (UserModel == null)
            {
                return NotFound($"El usuario {u_name} no existe 😓");
            }

            return Ok(UserModel);
        }

        // PUT: api/Usuarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{u_name}")]
        public async Task<IActionResult> PutUsuario(string u_name, Usuario usuario)
        {
            // Extrayecto objeto usuario
            Usuario? usuarioDb = await (from u in _context.Usuarios
                                        where u.U_name == u_name
                                        select u).FirstOrDefaultAsync();

            // Modificando el objeto
            usuarioDb.UrlFoto = usuario.UrlFoto;
            usuarioDb.Nombres = usuario.Nombres;
            usuarioDb.Apellidos = usuario.Apellidos;
            usuarioDb.Correo = usuario.Correo;
            usuarioDb.Intro = usuario.Intro;


            _context.Entry(usuarioDb).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(u_name))
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

        // POST: api/Usuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            if (_context.Usuarios == null)
            {
                return Problem("Entity set 'railwayContext.Usuarios' es nulo.");
            }
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuario", new { u_name = usuario.U_name }, usuario);
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{u_name}")]
        public async Task<IActionResult> DeleteUsuario(String u_name)
        {
            if (_context.Usuarios == null)
            {
                return NotFound($"El usuario {u_name} no existe 😓");
            }
            //Obteniendo usuario
            var usuario = await (from u in _context.Usuarios
                                 where u.U_name == u_name
                                 select u).FirstOrDefaultAsync();

            if (usuario == null)
            {
                return NotFound($"El usuario {u_name} no existe 😓");
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool UsuarioExists(String u_name)
        {
            return (_context.Usuarios?.Any(e => e.U_name == u_name)).GetValueOrDefault();
        }

    }
}
