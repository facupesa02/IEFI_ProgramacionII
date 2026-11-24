using IEFI.Entities.DTOs;
using IEFI.Business.Interfaces;
using IEFI.Data.Repositories.Interfaces;
using IEFI.Entities.Models;

namespace IEFI.Business.Services;

public class EstadiaService : IEstadiaService
{
    private readonly IEstadiaRepository _estadiaRepository;

    public EstadiaService(IEstadiaRepository estadiaRepository)
    {
        _estadiaRepository = estadiaRepository;
    }

    public async Task<List<EstadiaResponseDto>> ObtenerTodasAsync(
        CancellationToken cancellationToken = default)
    {
        var estadias = await _estadiaRepository.ObtenerTodasAsync(cancellationToken);
        return estadias.Select(MapearRespuesta).ToList();
    }

    public async Task<EstadiaResponseDto?> ObtenerPorIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var estadia = await _estadiaRepository.ObtenerPorIdAsync(id, cancellationToken);
        return estadia is null ? null : MapearRespuesta(estadia);
    }

    public async Task<string?> ValidarCreacionAsync(
        EstadiaRequestDto solicitud,
        CancellationToken cancellationToken = default)
    {
        if (solicitud.FechaIngreso.Date < DateTime.Today)
        {
            return "La fecha de ingreso no puede ser anterior a la fecha actual.";
        }

        if (solicitud.FechaSalida.Date < solicitud.FechaIngreso.Date)
        {
            return "La fecha de salida no puede ser anterior a la fecha de ingreso.";
        }

        var mascotaYaTieneEstadia = await _estadiaRepository.ExisteEstadiaParaMascotaAsync(
            solicitud.MascotaId,
            cancellationToken);
        if (mascotaYaTieneEstadia)
        {
            return "La mascota ya tiene una estadía registrada. Finalizá esa estadía antes de crear otra.";
        }

        var espacioDisponible = await _estadiaRepository.ObtenerDisponibilidadEspacioAsync(
            solicitud.EspacioId,
            cancellationToken);

        if (espacioDisponible is null)
        {
            return "El espacio seleccionado no existe.";
        }

        return espacioDisponible.Value
            ? null
            : "El espacio seleccionado no está disponible.";
    }

    public async Task<EstadiaResponseDto> AgregarAsync(
        EstadiaRequestDto solicitud,
        CancellationToken cancellationToken = default)
    {
        var estadia = MapearEntidad(solicitud);
        await _estadiaRepository.AgregarAsync(estadia, cancellationToken);
        return MapearRespuesta(estadia);
    }

    public Task<bool> ActualizarAsync(
        int id,
        EstadiaRequestDto solicitud,
        CancellationToken cancellationToken = default)
    {
        var estadia = MapearEntidad(solicitud);
        estadia.Id = id;
        return _estadiaRepository.ActualizarAsync(estadia, cancellationToken);
    }

    public Task<bool> EliminarAsync(int id, CancellationToken cancellationToken = default)
    {
        return _estadiaRepository.EliminarAsync(id, cancellationToken);
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
