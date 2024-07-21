using apiGBM.Models;

namespace apiGBM.Infra
{
    public class EntregaRepository : IEntregaRepository
    {
        private readonly ConexãoContext _context = new ConexãoContext();

        public void Add(Entrega entrega)
        {
            _context.Add(entrega);
            _context.SaveChanges(); 
        }
        public void Update(Entrega entrega)
        {
            _context.Update(entrega);
            _context.SaveChanges();
        }
        public void Delete(Entrega entrega)
        {
            _context.Remove(entrega);
            _context.SaveChanges();
        }

        public List<Entrega> GetByCPF(string fk_CPFmotorista)
        {
            return _context.Entrega.Where(entrega => entrega.fk_CPFmotorista == fk_CPFmotorista).ToList(); // retorna os objetos da entrega
        }

        public Entrega GetByPlaca(string fk_placacaminhao)
        {
            return _context.Entrega.SingleOrDefault(entrega => entrega.fk_PlacaCaminhao == fk_placacaminhao); // retorna o objeto da entrega
        }

        public Entrega GetById(Guid cd_Entrega)
        {
            return _context.Entrega.SingleOrDefault(entrega => entrega.cd_Entrega == cd_Entrega); // retorna o objeto da entrega
        }

        public List<Entrega> GetAll()
        {
            return _context.Entrega.ToList();
        }

    }
}
