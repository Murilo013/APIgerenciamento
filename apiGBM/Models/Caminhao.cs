using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apiGBM.Models
{
    [Table("caminhao")]
    public class Caminhao
    {
        [Key]
        public string cd_Placa { get; set; }
        public string ds_Modelo { get; set; }
        public int aa_Fabricacao { get; set; }
        public string ds_CorPrincipal { get; set; }
        public int qt_EixosTracao { get; set; }

        [ForeignKey(nameof(fk_CPFmotorista))]
        public string fk_CPFmotorista { get; set; }

        public Caminhao() { }

        public Caminhao(string cd_Placa, string ds_Modelo, int aa_Fabricacao, string ds_CorPrincipal, int qt_EixosTracao, string fk_CPFmotorista)
        {
            this.cd_Placa = cd_Placa;
            this.ds_Modelo = ds_Modelo;
            this.aa_Fabricacao = aa_Fabricacao;
            this.ds_CorPrincipal = ds_CorPrincipal;
            this.qt_EixosTracao = qt_EixosTracao;
            this.fk_CPFmotorista = fk_CPFmotorista;
        }
    }
}
