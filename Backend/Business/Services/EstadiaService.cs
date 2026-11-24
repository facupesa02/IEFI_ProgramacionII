using IEFI.Entities.DTOs;
using IEFI.Data.Repositories.Implementations;
using IEFI.Entities.Models;

namespace IEFI.Business.Services;

public class EstadiaService
{
    private readonly EstadiaRepository _estadiaRepository;

    public EstadiaService(EstadiaRepository estadiaRepository)
    {
        _estadiaRepository = estadiaRepository;
    }

    public async Task<List<EstadiaResponseDto>> ObtenerTodasAsync()
    {
        var estadias = await _estadiaRepository.ObtenerTodasAsync();
        return estadias.Select(MapearRespuesta).ToList();
    }

    public async Task<EstadiaResponseDto?> ObtenerPorIdAsync(int id)
    {
        var estadia = await _estadiaRepository.ObtenerPorIdAsync(id);
        return estadia is null ? null : MapearRespuesta(estadia);
    }

    public async Task<string?> ValidarCreacionAsync(EstadiaRequestDto solicitud)
    {
        var mascotaYaTieneEstadia = await _estadiaRepository.ExisteEstadiaParaMascotaAsync(
            solicitud.MascotaId);
        if (mascotaYaTieneEstadia)
        {
            return "La mascota ya tiene una estadía registrada. Finalizá esa estadía antes de crear otra.";
        }

        var espacio = await _estadiaRepository.ObtenerEspacioAsync(solicitud.EspacioId);
        if (espacio is null)
        {
            return "El espacio seleccionado no existe.";
        }

        if (!espacio.Estado)
        {
            return "El espacio seleccionado no está disponible.";
        }

        var cantidadEstadias = await _estadiaRepository.ObtenerCantidadEstadiasEnEspacioAsync(
            solicitud.EspacioId,
            solicitud.FechaIngreso,
            solicitud.FechaSalida);
        if (cantidadEstadias >= espacio.Capacidad)
        {
            return "El espacio seleccionado ya alcanzó su capacidad máxima para las fechas indicadas.";
        }

        return null;
    }

    public async Task<EstadiaResponseDto> AgregarAsync(EstadiaRequestDto solicitud)
    {
        var estadia = MapearEntidad(solicitud);
        await _estadiaRepository.AgregarAsync(estadia);
        return MapearRespuesta(estadia);
    }

    public Task<bool> ActualizarAsync(int id, EstadiaRequestDto solicitud)
    {
        var estadia = MapearEntidad(solicitud);
        estadia.Id = id;
        return _estadiaRepository.ActualizarAsync(estadia);
    }

    public Task<bool> EliminarAsync(int id)
    {
        return _estadiaRepository.EliminarAsync(id);
    }

    private static Estadia MapearEntidad(EstadiaRequestDto solicitud)
    {
        return new Estadia
        {
            MascotaId = solicitud.MascotaId,
            EspacioId = solicitud.EspacioId,
            FechaIngreso = solicitud.FechaIngreso,
            FechaSalida = solicitud.FechaSalida
        };
    }

    private static EstadiaResponseDto MapearRespuesta(Estadia estadia)
    {
        return new EstadiaResponseDto
        {
            Id = estadia.Id,
            MascotaId = estadia.MascotaId,
            EspacioId = estadia.EspacioId,
            FechaIngreso = estadia.FechaIngreso,
            FechaSalida = estadia.FechaSalida
        };
    }
}
