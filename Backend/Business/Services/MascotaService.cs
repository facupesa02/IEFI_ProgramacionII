using IEFI.Business.Interfaces;
using IEFI.Data.Repositories.Interfaces;
using IEFI.Entities.DTOs;
using IEFI.Entities.Models;

namespace IEFI.Business.Services;

public class MascotaService : IMascotaService
{
    private readonly IMascotaRepository _repository;

    public MascotaService(IMascotaRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<MascotaDto>> ObtenerTodas()
    {
        var mascotas = await _repository.ObtenerTodas();
        return mascotas.Select(MapearRespuesta).ToList();
    }

    public async Task<MascotaDto?> ObtenerPorId(int id)
    {
        var mascota = await _repository.ObtenerPorId(id);
        return mascota is null ? null : MapearRespuesta(mascota);
    }

    public async Task<MascotaDto> Crear(CrearMascotaDto mascota)
    {
        var nuevaMascota = new Mascota
        {
            Nombre = mascota.Nombre,
            Especie = mascota.Especie,
            Raza = mascota.Raza,
            Edad = mascota.Edad,
            Peso = mascota.Peso
        };

        var creada = await _repository.Crear(nuevaMascota);
        return MapearRespuesta(creada);
    }

    public async Task<MascotaDto?> Actualizar(int id, ActualizarMascotaDto mascota)
    {
        var existente = await _repository.ObtenerPorId(id);
        if (existente is null)
        {
            return null;
        }

        if (mascota.Edad > 0)
            existente.Edad = mascota.Edad.Value;
        if (mascota.Peso > 0)
            existente.Peso = mascota.Peso.Value;

        await _repository.Guardar(existente);
        return MapearRespuesta(existente);
    }

    public async Task<bool> Eliminar(int id)
    {
        return await _repository.Eliminar(id);
    }

    private static MascotaDto MapearRespuesta(Mascota mascota)
    {
        return new MascotaDto
        {
            Id = mascota.Id,
            Nombre = mascota.Nombre,
            Especie = mascota.Especie,
            Raza = mascota.Raza,
            Edad = mascota.Edad,
            Peso = mascota.Peso
        };
    }
}
