using apiGBM.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlTypes;

namespace apiGBM.Infra
{
    // METODOS DE MANIPULAÇÃO COM O BANCO DE DADOS
    public class MotoristaRepository : IMotoristaRepository
    {
        private readonly ConexãoContext _context = new ConexãoContext();

        public void Add(Motorista motorista)
        {
            _context.Motorista.Add(motorista);
            _context.SaveChanges();
        }

        public List<Motorista> GetAll()
        {
            return  _context.Motorista.ToList();
        }

        public Motorista GetByCPF(string cd_CPF)
        {
            return _context.Motorista.SingleOrDefault(motorista => motorista.cd_CPF == cd_CPF); // retorna o objeto do motorista
        }

        public void Update(Motorista motorista)
        {
            _context.Motorista.Update(motorista);
            _context.SaveChanges(); 
        }

        public void Delete(Motorista motorista)
        {
            _context.Remove(motorista);
            _context.SaveChanges();
        }
    }
}
