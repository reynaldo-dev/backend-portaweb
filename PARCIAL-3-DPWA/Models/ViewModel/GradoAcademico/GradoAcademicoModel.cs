using PARCIAL_3_DPWA.Models.ViewModel.Usuario;

namespace PARCIAL_3_DPWA.Models.ViewModel.GradoAcademico
{
    public class GradoAcademicoModel
    {
        public GradoAcademicoModel()
        {
            GradoAcademico = new HashSet<PutGradoAcademicoModel>();
        }
        public String U_name { get; set; }
        public virtual ICollection<PutGradoAcademicoModel> GradoAcademico { get; set; }
    }
}
