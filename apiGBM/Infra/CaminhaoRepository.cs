using apiGBM.Models;

namespace apiGBM.Infra
{
    public class CaminhaoRepository : ICaminhaoRepository
    {
        private readonly ConexãoContext _context = new ConexãoContext();

        public void Add(Caminhao caminhao)
        { 
            _context.Caminhao.Add(caminhao);
            _context.SaveChanges();
        }

        public List<Caminhao> GetAll()
        {
            return _context.Caminhao.ToList();
        }

        public Caminhao GetByPlaca(string cd_Placa)
        {
            return _context.Caminhao.SingleOrDefault(caminhao => caminhao.cd_Placa == cd_Placa); // retorna o objeto do caminhão
        }

        public Caminhao GetByCPF(string fk_CPFmotorista)
        {
            return _context.Caminhao.SingleOrDefault(caminhao => caminhao.fk_CPFmotorista == fk_CPFmotorista); // retorna o objeto do caminhao
        }

        public void Update(Caminhao caminhao)
        {
            _context.Caminhao.Update(caminhao);
            _context.SaveChanges();
        }

        public void Delete(Caminhao caminhao)
        {
            _context.Remove(caminhao);
            _context.SaveChanges();
        }
    }
}
