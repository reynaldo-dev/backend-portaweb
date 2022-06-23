using System;
using System.Collections.Generic;

namespace PARCIAL_3_DPWA.Models
{
    public partial class GradoAcademicoByUsuario
    {
        public int Id_grado_academico_by_usuario { get; set; }
        public string? Profesion { get; set; }
        public string? Universidad { get; set; }
        public string? Objetivos { get; set; }
        public int? Id_usuario { get; set; }

    }
}
