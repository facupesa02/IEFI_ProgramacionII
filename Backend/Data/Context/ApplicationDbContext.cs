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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Las configuraciones de Entity Framework se deben agregar aqui.
    }
}
