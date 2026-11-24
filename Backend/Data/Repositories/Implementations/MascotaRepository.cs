using IEFI.Data.Context;
using IEFI.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace IEFI.Data.Repositories.Implementations;

public class MascotaRepository
{
    private readonly ApplicationDbContext _context;

    public MascotaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Mascota>> ObtenerTodas()
    {
        return await _context.Mascotas
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Mascota?> ObtenerPorId(int id)
    {
        return await _context.Mascotas
            .FirstOrDefaultAsync(mascota => mascota.Id == id);
    }

    public async Task<Mascota> Crear(Mascota mascota)
    {
        await _context.Mascotas.AddAsync(mascota);
        await _context.SaveChangesAsync();
        return mascota;
    }

    public async Task Guardar(Mascota mascota)
    {
        await _context.SaveChangesAsync();
    }

    public async Task<bool> Eliminar(int id)
    {
        var mascota = await _context.Mascotas.FindAsync(id);
        if (mascota is null)
        {
            return false;
        }

        _context.Mascotas.Remove(mascota);
        await _context.SaveChangesAsync();
        return true;
    }
}