using apiGBM.Models;
using Microsoft.EntityFrameworkCore;

namespace apiGBM.Infra
{
    //CONEXÃO AO BANCO DE DADOS
    public class ConexãoContext : DbContext
    {
        public DbSet<Motorista> Motorista {  get; set; }
        public DbSet<Caminhao> Caminhao { get; set; }
        public DbSet<Entrega> Entrega { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder.UseNpgsql("Server=touchingly-crack-woodcock.data-1.use1.tembo.io;" +
                                                        "Port=5432;" +
                                                        "Database=postgres;" +
                                                        "User Id=postgres;" +
                                                        "Password=6GlUAjJ0sPcPV0Xp;"));
        }
    }
}
