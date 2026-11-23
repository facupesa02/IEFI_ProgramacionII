using IEFI.Entities.Models;

namespace IEFI.Data.Repositories.Interfaces;

public interface IEstadiaRepository
{
    Task<List<Estadia>> ObtenerTodasAsync(CancellationToken cancellationToken = default);
    Task<Estadia?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task AgregarAsync(Estadia estadia, CancellationToken cancellationToken = default);
    Task<bool> ActualizarAsync(Estadia estadia, CancellationToken cancellationToken = default);
    Task<bool> EliminarAsync(int id, CancellationToken cancellationToken = default);
}
