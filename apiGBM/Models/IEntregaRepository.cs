namespace apiGBM.Models
{
    public interface IEntregaRepository
    {
        void Add(Entrega entrega);
        void Update(Entrega entrega);
        void Delete(Entrega entrega);
        List<Entrega> GetAll();
        List<Entrega> GetByCPF(string CPF);
        Entrega GetByPlaca(string Placa);
        Entrega GetById(Guid id);

    }
}
