using PARCIAL_3_DPWA.Models.ViewModel.Usuario;

namespace PARCIAL_3_DPWA.Models.ViewModel.ExperienciaUsuario
{
    public class ExperienciaUsuarioModel
    {
        public ExperienciaUsuarioModel()
        {
            Experiencia = new HashSet<PutExperienciaUsuarioModel>();
        }
        public String U_name { get; set; }
        public virtual ICollection<PutExperienciaUsuarioModel> Experiencia { get; set; }
    }
}
