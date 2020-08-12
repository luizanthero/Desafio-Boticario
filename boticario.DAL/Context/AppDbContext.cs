using Microsoft.EntityFrameworkCore;

namespace boticario.Models
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

        protected override void OnModelCreating(ModelBuilder builder) 
        {
            builder.HasDefaultSchema("BOTICARIO");

            builder.Entity<Compra>(entity =>
            {
                entity.Property(item => item.Ativo).HasDefaultValue(true);
                entity.Property(item => item.DataCriacao).HasDefaultValue("GETDATE()");
            });
            builder.Entity<Historico>(entity =>
            {
                entity.Property(item => item.Data).HasDefaultValue("GETDATE()");
            });
            builder.Entity<ParametroSistema>(entity =>
            {
                entity.Property(item => item.Ativo).HasDefaultValue(true);
                entity.Property(item => item.DataCriacao).HasDefaultValue("GETDATE()");

                entity.HasIndex(item => item.NomeParametro).IsUnique();
            });
            builder.Entity<RegraCashback>(entity =>
            {
                entity.Property(item => item.Ativo).HasDefaultValue(true);
                entity.Property(item => item.DataCriacao).HasDefaultValue("GETDATE()");
            });
            builder.Entity<Revendedor>(entity =>
            {
                entity.Property(item => item.Ativo).HasDefaultValue(true);
                entity.Property(item => item.DataCriacao).HasDefaultValue("GETDATE()");

                entity.HasIndex(item => item.Cpf).IsUnique();
                entity.HasIndex(item => item.Email).IsUnique();
            });
            builder.Entity<StatusCompra>(entity =>
            {
                entity.Property(item => item.Ativo).HasDefaultValue(true);
                entity.Property(item => item.DataCriacao).HasDefaultValue("GETDATE()");

                entity.HasIndex(item => item.Descricao).IsUnique();
            });
            builder.Entity<TipoHistorico>(entity =>
            {
                entity.Property(item => item.Ativo).HasDefaultValue(true);
                entity.Property(item => item.DataCriacao).HasDefaultValue("GETDATE()");

                entity.HasIndex(item => item.Descricao).IsUnique();
            });
        }
    }
}
