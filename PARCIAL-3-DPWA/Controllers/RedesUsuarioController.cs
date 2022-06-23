using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PARCIAL_3_DPWA.Models;
using PARCIAL_3_DPWA.Models.ViewModel.Red;
using PARCIAL_3_DPWA.Models.ViewModel.Usuario;

namespace PARCIAL_3_DPWA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedesUsuarioController : ControllerBase
    {
        private readonly railwayContext _context;

        public RedesUsuarioController(railwayContext context)
        {
            _context = context;
        }

        // GET: api/RedesUsuario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RedByUser>>> GetRedByUsers()
        {
          if (_context.RedByUsers == null)
          {
              return NotFound();
          }
            // Sacando datos ordenados
            var RedByUsuario = await (from redU in _context.RedByUsers
                                                orderby redU.Id_usuario ascending
                                                select redU).ToListAsync();

            List<RedUsuarioModel> ListaRedUsuarioModel = new List<RedUsuarioModel>();
            foreach (RedByUser red in RedByUsuario)
            {
                //Obteniendo usuario id
                var usuarioU_name = ObtenerU_nameUsuario(red.Id_usuario ?? default(int)).Result;

                // Verificando que hayan dados en la lista
                if (ListaRedUsuarioModel.Count != 0)
                {
                    // Verificando que no se repitan
                    if (ListaRedUsuarioModel.LastOrDefault().U_name.Equals(usuarioU_name))
                    {
                        continue;
                    };
                }

                // Obteniendo redes de usuario
                var redUsuarioData = await (from redU in _context.RedByUsers
                                                      join re in _context.Reds on redU.Id_red equals re.Id_red
                                                      where redU.Id_usuario == red.Id_usuario && re.Id_red == redU.Id_red
                                                      select new RedesModel
                                                      {
                                                          Nombre = re.Nombre,
                                                          Accesslink = redU.Accesslink
                                                      }).ToListAsync();

                //Uniendo
                RedUsuarioModel redUsuarioModel = new RedUsuarioModel
                {
                    U_name = usuarioU_name,
                    Redes = redUsuarioData
                };

                ListaRedUsuarioModel.Add(redUsuarioModel);

            }

            return Ok(ListaRedUsuarioModel);
        }

        // GET: api/RedesUsuario/5
        [HttpGet("{u_name}")]
        public async Task<ActionResult<RedByUser>> GetRedByUser(String u_name)
        {
            //Obteniendo usuario id
            var usuarioId = ObtenerIdUsuario(u_name).Result;

            //Obteniendo usuario id
            var usuarioU_name = ObtenerU_nameUsuario(usuarioId).Result;


            // Obteniendo redes de usuario
            var redUsuarioData = await (from redU in _context.RedByUsers
                                        join re in _context.Reds on redU.Id_red equals re.Id_red
                                        where redU.Id_usuario == usuarioId && redU.Id_red == re.Id_red
                                        select new RedesModel
                                        {
                                            Nombre = re.Nombre,
                                            Accesslink = redU.Accesslink
                                        }).ToListAsync();

            //Uniendo
            RedUsuarioModel redUsuarioModel = new RedUsuarioModel
            {
                U_name = usuarioU_name,
                Redes = redUsuarioData
            };


            if (redUsuarioModel == null)
            {
                return NotFound($"El usuario {u_name} no tiene redes 😓");
            }

            return Ok(redUsuarioModel);
        }

        // PUT: api/RedesUsuario/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("EditarRed/{u_name}/{redAnterior}/{redNueva}")]
        public async Task<IActionResult> PutRedByUserRed(String u_name,String redAnterior,String redNueva)
        {
            // Obteniendo usuario id
            var usuarioId = ObtenerIdUsuario(u_name).Result;

            //Obteniendo red id
            var redId = ObtenerIdRed(redAnterior).Result;

            //Buscando id de redByUsuarios
            var redByUsuariosId = ObtenerObjetoRedByUsuarios(usuarioId, redId).Result;

            if (usuarioId != redByUsuariosId.Id_usuario)
            {
                return BadRequest();
            }

            //Obteniendo red nueva id
            var RedNuevaId = ObtenerIdRed(redNueva).Result;

            //Cambiando red
            redByUsuariosId.Id_red = RedNuevaId;

            _context.Entry(redByUsuariosId).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RedByUserExists(usuarioId))
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
        // PUT: api/RedesUsuario/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("EditarLink/{u_name}/{red}/{linkNuevo}")]
        public async Task<IActionResult> PutRedByUserAccessLink(String u_name, String red, String linkNuevo)
        {
            // Obteniendo usuario id
            var usuarioId = ObtenerIdUsuario(u_name).Result;

            //Obteniendo red id
            var redId = ObtenerIdRed(red).Result;

            //Buscando id de redByUsuarios
            var redByUsuariosId = ObtenerObjetoRedByUsuarios(usuarioId, redId).Result;

            if (usuarioId != redByUsuariosId.Id_usuario)
            {
                return BadRequest();
            }

            //Cambiando link
            redByUsuariosId.Accesslink = linkNuevo.Replace("%2F", "/");

            _context.Entry(redByUsuariosId).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RedByUserExists(usuarioId))
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

        // POST: api/RedesUsuario
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RedByUser>> PostRedByUser(UnameRedModel redByUser)
        {
            if (_context.RedByUsers == null)
            {
                return Problem("Entity set 'railwayContext.redByUser' es nulo.");
            }

            //Obteniendo usuario id
            var usuarioId = ObtenerIdUsuario(redByUser.U_name).Result;

            //Obteniendo red id
            var redId = ObtenerIdRed(redByUser.nombreRed).Result;

            //Juntando todo
            RedByUser redByUsuario = new RedByUser
            {
                Id_red = redId,
                Id_usuario = usuarioId,
                Accesslink = redByUser.accessLink.Replace("%2F","/")
            };

            _context.RedByUsers.Add(redByUsuario);
            await _context.SaveChangesAsync();

            return await GetRedByUser(redByUser.U_name);
        }

        // DELETE: api/RedesUsuario/5
        [HttpDelete("{u_name}/{red}")]
        public async Task<IActionResult> DeleteRedByUser(String u_name, String red)
        {
            //Obteniendo usuario id
            var usuarioId = ObtenerIdUsuario(u_name).Result;

            //Obteniendo red id
            var redId = ObtenerIdRed(red).Result;

            //Buscando id de redByUsuarios
            var redByUsuariosId = ObtenerRedByUsuarios(usuarioId, redId).Result;

            if (_context.RedByUsers == null)
            {
                return NotFound($"El usuario {u_name} o red {red} no se encontro 😓");
            }
            var redByUsuario = await _context.RedByUsers.FindAsync(redByUsuariosId);
            if (redByUsuario == null)
            {
                return NotFound($"El usuario {u_name} o red {red} no se encontro 😓");
            }

            _context.RedByUsers.Remove(redByUsuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RedByUserExists(int id)
        {
            return (_context.RedByUsers?.Any(e => e.Id_red_by_user == id)).GetValueOrDefault();
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
        private async Task<int> ObtenerIdRed(String nombreRed)
        {
            //Obteniendo red id
            return await (from r in _context.Reds
                          where r.Nombre == nombreRed
                          select r.Id_red).FirstOrDefaultAsync();
        }

        private async Task<int> ObtenerRedByUsuarios(int idUsuario, int idRed)
        {
            //Obteniendo red id
            return await (from r in _context.RedByUsers
                          where r.Id_usuario == idUsuario && r.Id_red == idRed
                          select r.Id_red_by_user).FirstOrDefaultAsync();
        }

        private async Task<RedByUser> ObtenerObjetoRedByUsuarios(int idUsuario, int idRed)
        {
            //Obteniendo RedByUsuario
            return await (from r in _context.RedByUsers
                          where r.Id_usuario == idUsuario && r.Id_red == idRed
                          select r).FirstOrDefaultAsync();
        }
    }
}
