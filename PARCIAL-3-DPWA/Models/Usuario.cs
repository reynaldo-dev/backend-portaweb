using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace PARCIAL_3_DPWA.Models
{
    public partial class Usuario
    {

        public int Id_usuario { get; set; }
        public string? U_name { get; set; }
        public string? UrlFoto { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string? Correo { get; set; }
        public string? Intro { get; set; }

    }
    /*
    //Tratando de hacer una funcion pero no reciben task los objetos
    //hacer DI para no llamar _context
    public class UsuarioFuncion
    {

        internal static async Task<Usuario> obtenerUsuario(String u_name, railwayContext _context)
        {
            return await(from u in _context.Usuarios
                         where u.U_name == u_name
                         select u).FirstOrDefaultAsync();
            throw new NotImplementedException();
        }
    }
    */
}
