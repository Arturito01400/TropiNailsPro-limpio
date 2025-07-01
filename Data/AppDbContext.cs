using Microsoft.EntityFrameworkCore;
using TropiNailsPro.Models;

namespace TropiNailsPro.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // ✅ Tablas principales
        public DbSet<Usuario> Usuarios { get; set; } = default!;
        public DbSet<Cita> Citas { get; set; } = default!;
        public DbSet<ModeloUnas> ModelosUnas { get; set; } = default!;
        public DbSet<Pago> Pagos { get; set; } = default!;
        public DbSet<Producto> Productos { get; set; } = default!;
        public DbSet<PagoEfectivo> PagosEfectivo { get; set; } = default!;
        public DbSet<PagoTransferencia> PagosTransferencia { get; set; } = default!;
        public DbSet<Gasto> Gastos { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ Renombrar tablas
            modelBuilder.Entity<Usuario>().ToTable("Usuarios");
            modelBuilder.Entity<Cita>().ToTable("Citas");
            modelBuilder.Entity<ModeloUnas>().ToTable("ModelosUnas");
            modelBuilder.Entity<Pago>().ToTable("Pagos");
            modelBuilder.Entity<Producto>().ToTable("Productos");
            modelBuilder.Entity<PagoEfectivo>().ToTable("PagosEfectivo");
            modelBuilder.Entity<PagoTransferencia>().ToTable("PagosTransferencia");
            modelBuilder.Entity<Gasto>().ToTable("Gastos");

            // ✅ Índices únicos condicionales
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique()
                .HasFilter("[Email] IS NOT NULL");

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Telefono)
                .IsUnique()
                .HasFilter("[Telefono] IS NOT NULL");
        }
    }
}
