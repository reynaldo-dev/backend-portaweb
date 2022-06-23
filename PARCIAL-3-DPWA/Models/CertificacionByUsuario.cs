using System;
using System.Collections.Generic;

namespace PARCIAL_3_DPWA.Models
{
    public partial class CertificacionByUsuario
    {
        public int Id_certificacion_by_Usuario { get; set; }
        public int? Id_certificacion { get; set; }
        public int? Id_usuario { get; set; }
    }
}
