using IEFI.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace IEFI.Data.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Agregar aqui los DbSet de las entidades:
    // public DbSet<Entidad> Entidades => Set<Entidad>();
    public DbSet<Espacio> Espacios => Set<Espacio>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Las configuraciones de Entity Framework se deben agregar aqui.
    }
}
