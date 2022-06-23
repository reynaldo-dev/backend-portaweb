using PARCIAL_3_DPWA.Models.ViewModel.Usuario;

namespace PARCIAL_3_DPWA.Models.ViewModel.Certificacion
{
    public class CertificacionUsuarioModel
    {
        public CertificacionUsuarioModel(){
            Certificacion = new HashSet<CertificacionModel>();
        }
        public String U_name { get; set; }
        public virtual ICollection<CertificacionModel> Certificacion { get; set; }
       
    }
}
