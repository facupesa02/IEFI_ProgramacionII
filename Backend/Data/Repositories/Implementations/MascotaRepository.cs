using IEFI.Data.Context;
using IEFI.Entities.DTOs;
using IEFI.Entities.Models;
using IEFI.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IEFI.Data.Repositories.Implementations;

public class MascotaRepository : IMascotaRepository
{
    private readonly ApplicationDbContext _context;

    public MascotaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<MascotaDto>> ObtenerTodas()
    {
        return await _context.Mascotas
            .Select(m => new MascotaDto
            {
                Id = m.Id,
                Nombre = m.Nombre,
                Especie = m.Especie,
                Raza = m.Raza,
                Edad = m.Edad,
                Peso = m.Peso
            })
            .ToListAsync();
    }

    public async Task<MascotaDto?> ObtenerPorId(int id)
    {
        return await _context.Mascotas
            .Where(m => m.Id == id)
            .Select(m => new MascotaDto
            {
                Id = m.Id,
                Nombre = m.Nombre,
                Especie = m.Especie,
                Raza = m.Raza,
                Edad = m.Edad,
                Peso = m.Peso
            })
            .FirstOrDefaultAsync();
    }

    public async Task<MascotaDto> Crear(CrearMascotaDto mascota)
    {
        Mascota nuevaMascota = new Mascota
        {
            Nombre = mascota.Nombre,
            Especie = mascota.Especie,
            Raza = mascota.Raza,
            Edad = mascota.Edad,
            Peso = mascota.Peso
        };

        _context.Mascotas.Add(nuevaMascota);
        await _context.SaveChangesAsync();

        return new MascotaDto
        {
            Id = nuevaMascota.Id,
            Nombre = nuevaMascota.Nombre,
            Especie = nuevaMascota.Especie,
            Raza = nuevaMascota.Raza,
            Edad = nuevaMascota.Edad,
            Peso = nuevaMascota.Peso
        };
    }

    public async Task<MascotaDto?> Actualizar(int id, ActualizarMascotaDto mascota)
    {
        Mascota? existente = await _context.Mascotas.FindAsync(id);
        if (existente == null)
        {
            return null;
        }

        existente.Nombre = mascota.Nombre;
        existente.Especie = mascota.Especie;
        existente.Raza = mascota.Raza;
        existente.Edad = mascota.Edad;
        existente.Peso = mascota.Peso;

        await _context.SaveChangesAsync();

        return new MascotaDto
        {
            Id = existente.Id,
            Nombre = existente.Nombre,
            Especie = existente.Especie,
            Raza = existente.Raza,
            Edad = existente.Edad,
            Peso = existente.Peso
        };
    }

    public async Task<bool> Eliminar(int id)
    {
        Mascota? mascota = await _context.Mascotas.FindAsync(id);
        if (mascota == null)
        {
            return false;
        }

        _context.Mascotas.Remove(mascota);
        await _context.SaveChangesAsync();
        return true;
    }
}
