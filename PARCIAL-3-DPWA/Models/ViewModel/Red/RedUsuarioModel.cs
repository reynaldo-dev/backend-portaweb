using PARCIAL_3_DPWA.Models.ViewModel.Usuario;

namespace PARCIAL_3_DPWA.Models.ViewModel.Red
{
    public class RedUsuarioModel
    {
        public RedUsuarioModel(){
            Redes = new HashSet<RedesModel>();
        }
        public String U_name { get; set; }
        public virtual ICollection<RedesModel> Redes { get; set; }
       
    }
}
