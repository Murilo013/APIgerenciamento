using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace apiGBM.Models
{

    [Table("entrega")]
    public class Entrega
    {
        [Key]
        public Guid cd_Entrega { get; set; } 

        public string dt_Entrega { get; set; }
        public string ds_Origem { get; set; }
        public string ds_Destino { get; set; }
        public string ds_CargaTransportada { get; set; }

        public string ds_StatusEntrega { get; set; }   

        [ForeignKey(nameof(fk_PlacaCaminhao))]
        public string fk_PlacaCaminhao { get; set; }

        [ForeignKey(nameof(fk_CPFmotorista))]
        public string fk_CPFmotorista { get; set; }

        public Entrega() {}

        public Entrega(string dt_entrega, string ds_origem, string ds_destino, string ds_cargatransportada,string ds_StatusEntrega,string fk_placacaminhao,string fk_cpfmotorista)
        { 
            this.cd_Entrega = Guid.NewGuid();
            this.dt_Entrega = dt_entrega;
            this.ds_Origem = ds_origem;
            this.ds_Destino = ds_destino;
            this.ds_CargaTransportada = ds_cargatransportada;
            this.ds_StatusEntrega = ds_StatusEntrega;
            this.fk_PlacaCaminhao = fk_placacaminhao;
            this.fk_CPFmotorista = fk_cpfmotorista;
        }
    }
}
