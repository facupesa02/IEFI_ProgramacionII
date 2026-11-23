using IEFI.Data.Context;
using IEFI.Data.Repositories.Interfaces;
using IEFI.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace IEFI.Data.Repositories.Implementations;

public class EstadiaRepository : IEstadiaRepository
{
    private readonly ApplicationDbContext _context;

    public EstadiaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Estadia>> ObtenerTodasAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Estadias
            .AsNoTracking()
            .Include(estadia => estadia.Mascota)
            .Include(estadia => estadia.Espacio)
            .ToListAsync(cancellationToken);
    }

    public async Task<Estadia?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Estadias
            .AsNoTracking()
            .Include(estadia => estadia.Mascota)
            .Include(estadia => estadia.Espacio)
            .FirstOrDefaultAsync(estadia => estadia.Id == id, cancellationToken);
    }

    public Task<bool> ExisteMascotaAsync(int id, CancellationToken cancellationToken = default)
    {
        return _context.Mascotas.AnyAsync(mascota => mascota.Id == id, cancellationToken);
    }

    public Task<bool> ExisteEspacioAsync(int id, CancellationToken cancellationToken = default)
    {
        return _context.Espacios.AnyAsync(espacio => espacio.Id == id, cancellationToken);
    }

    public async Task AgregarAsync(Estadia estadia, CancellationToken cancellationToken = default)
    {
        await _context.Estadias.AddAsync(estadia, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ActualizarAsync(Estadia estadia, CancellationToken cancellationToken = default)
    {
        var estadiaExistente = await _context.Estadias.FindAsync(
            new object[] { estadia.Id },
            cancellationToken);

        if (estadiaExistente is null)
        {
            return false;
        }

        estadiaExistente.MascotaId = estadia.MascotaId;
        estadiaExistente.EspacioId = estadia.EspacioId;
        estadiaExistente.FechaIngreso = estadia.FechaIngreso;
        estadiaExistente.FechaSalida = estadia.FechaSalida;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> EliminarAsync(int id, CancellationToken cancellationToken = default)
    {
        var estadia = await _context.Estadias.FindAsync(new object[] { id }, cancellationToken);

        if (estadia is null)
        {
            return false;
        }

        _context.Estadias.Remove(estadia);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
