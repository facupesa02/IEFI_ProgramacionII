using IEFI.Data.Context;
using IEFI.Entities.DTOs;
using IEFI.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace IEFI.Data.Repositories.Implementations;

public class EspacioRepository
{
    private readonly ApplicationDbContext _context;

    public EspacioRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<EspacioResponseDto>> GetAllAsync()
    {
        return await _context.Espacios
            .Select(x => new EspacioResponseDto
            {
                Id = x.Id,
                Nombre = x.Nombre,
                Tamano = x.Tamano,
                Capacidad = x.Capacidad,
                Estado = x.Estado,
                Tipo = x.Tipo
            })
            .ToListAsync();
    }

    public async Task<EspacioResponseDto?> GetByIdAsync(int id)
    {
        return await _context.Espacios
            .Where(x => x.Id == id)
            .Select(x => new EspacioResponseDto
            {
                Id = x.Id,
                Nombre = x.Nombre,
                Tamano = x.Tamano,
                Capacidad = x.Capacidad,
                Estado = x.Estado,
                Tipo = x.Tipo
            })
            .FirstOrDefaultAsync();
    }

    public async Task CreateAsync(EspacioRequestDto newEspacio)
    {
        var espacio = new Espacio
        {
            Nombre = newEspacio.Nombre,
            Tamano = newEspacio.Tamano,
            Capacidad = newEspacio.Capacidad,
            Estado = newEspacio.Estado,
            Tipo = newEspacio.Tipo
        };

        await _context.Espacios.AddAsync(espacio);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(int id, EspacioRequestDto espacioActualizado)
    {
        var espacio = await _context.Espacios.FirstOrDefaultAsync(x => x.Id == id);

        if (espacio is null)
        {
            return false;
        }

        espacio.Nombre = espacioActualizado.Nombre;
        espacio.Tamano = espacioActualizado.Tamano;
        espacio.Capacidad = espacioActualizado.Capacidad;
        espacio.Estado = espacioActualizado.Estado;
        espacio.Tipo = espacioActualizado.Tipo;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var espacio = await _context.Espacios.FirstOrDefaultAsync(x => x.Id == id);

        if (espacio is null)
        {
            return false;
        }

        _context.Espacios.Remove(espacio);
        await _context.SaveChangesAsync();
        return true;
    }
}
