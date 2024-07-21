namespace apiGBM.Models
{

    public interface IMotoristaRepository
    {

        void Add(Motorista motorista);
        void Update(Motorista motorista);
        void Delete(Motorista motorista);

        Motorista GetByCPF(string cd_CPF);

        List<Motorista> GetAll();
    }
}
