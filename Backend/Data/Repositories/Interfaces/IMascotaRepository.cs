using IEFI.Entities.Models;

namespace IEFI.Data.Repositories.Interfaces;

public interface IMascotaRepository
{
    Task<List<Mascota>> ObtenerTodas();
    Task<Mascota?> ObtenerPorId(int id);
    Task<Mascota> Crear(Mascota mascota);
    Task Guardar(Mascota mascota);
    Task<bool> Eliminar(int id);
}
