using System.ComponentModel.DataAnnotations;

namespace apiGBM.View
{
    public class MotoristaView
    {
        public string cd_CPF { get;  set; }
        public string nm_Nome { get;  set; }
        public string ds_CategoriaCNH { get;  set; }
        public string dt_Nascimento { get;  set; }
        public string ds_Telefone { get;  set; }
    }
}
