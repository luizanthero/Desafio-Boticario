using boticario.Models;
using Microsoft.EntityFrameworkCore;

namespace boticario.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Compra> Compras { get; set; }
        public DbSet<Historico> Historicos { get; set; }
        public DbSet<ParametroSistema> ParametrosSistema { get; set; }
        public DbSet<RegraCashback> RegrasCashback { get; set; }
        public DbSet<Revendedor> Revendedores { get; set; }
        public DbSet<StatusCompra> StatusCompra { get; set; }
        public DbSet<TipoHistorico> TiposHistorico { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) { }
    }
}
