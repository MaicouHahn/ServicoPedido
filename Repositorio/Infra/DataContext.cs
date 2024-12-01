using Microsoft.EntityFrameworkCore;
using ServicoPedido.Models;

namespace ServicoPedido.Repositorio.Infra
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Pedido> pedidos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pedido>().HasKey(p => p.IdPedido);
            base.OnModelCreating(modelBuilder);
        }
    }
}

