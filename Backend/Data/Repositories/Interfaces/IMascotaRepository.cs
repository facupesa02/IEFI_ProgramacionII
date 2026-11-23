using IEFI.Entities.DTOs;

namespace IEFI.Data.Repositories.Interfaces;

public interface IMascotaRepository
{
    Task<List<MascotaDto>> ObtenerTodas();
    Task<MascotaDto?> ObtenerPorId(int id);
    Task<MascotaDto> Crear(CrearMascotaDto mascota);
    Task<MascotaDto?> Actualizar(int id, ActualizarMascotaDto mascota);
    Task<bool> Eliminar(int id);
}
