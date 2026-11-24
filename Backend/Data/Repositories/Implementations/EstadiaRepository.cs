using IEFI.Data.Context;
using IEFI.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace IEFI.Data.Repositories.Implementations;

public class EstadiaRepository
{
    private readonly ApplicationDbContext _context;

    public EstadiaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Estadia>> ObtenerTodasAsync()
    {
        return await _context.Estadias
            .AsNoTracking()
            .Include(estadia => estadia.Mascota)
            .Include(estadia => estadia.Espacio)
            .ToListAsync();
    }

    public async Task<Estadia?> ObtenerPorIdAsync(int id)
    {
        return await _context.Estadias
            .AsNoTracking()
            .Include(estadia => estadia.Mascota)
            .Include(estadia => estadia.Espacio)
            .FirstOrDefaultAsync(estadia => estadia.Id == id);
    }

    public Task<Espacio?> ObtenerEspacioAsync(int id)
    {
        return _context.Espacios
            .FirstOrDefaultAsync(espacio => espacio.Id == id);
    }

    public Task<int> ObtenerCantidadEstadiasEnEspacioAsync(
        int espacioId,
        DateTime fechaIngreso,
        DateTime fechaSalida)
    {
        return _context.Estadias.CountAsync(estadia =>
            estadia.EspacioId == espacioId &&
            estadia.FechaIngreso.Date <= fechaSalida.Date &&
            estadia.FechaSalida.Date >= fechaIngreso.Date);
    }

    public Task<bool> ExisteEstadiaParaMascotaAsync(int mascotaId)
    {
        return _context.Estadias.AnyAsync(
            estadia => estadia.MascotaId == mascotaId);
    }

    public Task<bool> ExisteMascotaAsync(int id)
    {
        return _context.Mascotas.AnyAsync(mascota => mascota.Id == id);
    }

    public Task<bool> ExisteEspacioAsync(int id)
    {
        return _context.Espacios.AnyAsync(espacio => espacio.Id == id);
    }

    public async Task AgregarAsync(Estadia estadia)
    {
        await _context.Estadias.AddAsync(estadia);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ActualizarAsync(Estadia estadia)
    {
        var estadiaExistente = await _context.Estadias.FindAsync(new object[] { estadia.Id });

        if (estadiaExistente is null)
        {
            return false;
        }

        estadiaExistente.MascotaId = estadia.MascotaId;
        estadiaExistente.EspacioId = estadia.EspacioId;
        estadiaExistente.FechaIngreso = estadia.FechaIngreso;
        estadiaExistente.FechaSalida = estadia.FechaSalida;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var estadia = await _context.Estadias.FindAsync(new object[] { id });

        if (estadia is null)
        {
            return false;
        }

        _context.Estadias.Remove(estadia);
        await _context.SaveChangesAsync();
        return true;
    }
}
