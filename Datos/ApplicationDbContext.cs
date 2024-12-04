using Microsoft.EntityFrameworkCore;
using TrackPay.Models;

namespace TrackPay.datos
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Pago> Pagos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Pago>(entity =>
            {
                entity.Property(p => p.Monto)
                      .HasColumnType("decimal(18,2)"); // Precisión de 18 dígitos y 2 decimales
            });
        }
    }
}
