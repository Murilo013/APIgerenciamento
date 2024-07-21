namespace apiGBM.Models
{
    public interface ICaminhaoRepository
    {

        void Add(Caminhao caminhao);
        void Update(Caminhao caminhao);
        void Delete(Caminhao caminhao);

        Caminhao GetByPlaca(string cd_placa);

        Caminhao GetByCPF(string cd_CPF);

        List<Caminhao> GetAll();
    }
}



