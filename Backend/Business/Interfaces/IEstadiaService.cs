using IEFI.Business.Dto;
using IEFI.Entities.Models;

namespace IEFI.Business.Interfaces;

public interface IEstadiaService
{
    Task<List<EstadiaResponseDto>> ObtenerTodasAsync(CancellationToken cancellationToken = default);
    Task<EstadiaResponseDto?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<EstadiaResponseDto> AgregarAsync(
        EstadiaRequestDto estadia,
        CancellationToken cancellationToken = default);
    Task<bool> ActualizarAsync(
        int id,
        EstadiaRequestDto estadia,
        CancellationToken cancellationToken = default);
    Task<bool> EliminarAsync(int id, CancellationToken cancellationToken = default);
}
