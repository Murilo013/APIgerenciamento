using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace apiGBM.View
{
    public class EntregaView
    {
        public string dt_Entrega { get; set; }
        public string ds_Origem { get; set; }
        public string ds_Destino { get; set; }
        public string ds_CargaTransportada { get; set; }
        public string ds_StatusEntrega { get; set; }
        public string fk_PlacaCaminhao { get; set; }
        public string fk_CPFmotorista { get; set; }


    }
}
