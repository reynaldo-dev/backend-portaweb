using System;
using System.Collections.Generic;

namespace PARCIAL_3_DPWA.Models
{
    public partial class RedByUser
    {
        public int Id_red_by_user { get; set; }
        public int? Id_usuario { get; set; }
        public int? Id_red { get; set; }
        public string? Accesslink { get; set; }
    }
}
