using Microsoft.EntityFrameworkCore;
using IEFI.Entities.Models;
using IEFI.Entities.Models;

namespace IEFI.Data.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Mascota> Mascotas => Set<Mascota>();
    public DbSet<Espacio> Espacios => Set<Espacio>();
    public DbSet<Estadia> Estadias => Set<Estadia>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Mascota>()
            .Property(m => m.Peso)
            .HasPrecision(10, 2);
    }
}
