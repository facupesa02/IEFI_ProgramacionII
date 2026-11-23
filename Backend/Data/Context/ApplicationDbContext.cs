using Microsoft.EntityFrameworkCore;
using IEFI.Entities.Models;

namespace IEFI.Data.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Mascota> Mascotas => Set<Mascota>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Mascota>()
            .Property(m => m.Peso)
            .HasPrecision(10, 2);
    }
}
