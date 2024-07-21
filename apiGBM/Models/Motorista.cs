using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apiGBM.Models
{
    [Table("motorista")]
    public class Motorista
    {
        [Key]
        public string cd_CPF {  get;  set; }
        public string nm_Nome { get;  set; }

        public string ds_CategoriaCNH { get;  set; }

        public string dt_Nascimento { get;  set; }

        public string ds_Telefone { get;  set; }


        [ForeignKey(nameof(fk_PlacaCaminhao))]
        public string ?fk_PlacaCaminhao { get; set; }

        public Motorista() { }

        public Motorista(string cd_cpf, string nm_nome, string ds_categoriaCNH, string dt_nascimento, string ds_telefone)
        {
            this.cd_CPF = cd_cpf;
            this.nm_Nome = nm_nome;
            this.ds_CategoriaCNH = ds_categoriaCNH;
            this.dt_Nascimento = dt_nascimento;
            this.ds_Telefone = ds_telefone;
        }
    }
}