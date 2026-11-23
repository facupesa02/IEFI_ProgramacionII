using IEFI.Entities.DTOs;

namespace IEFI.Business.Interfaces;

public interface IMascotaService
{
    Task<List<MascotaDto>> ObtenerTodas();
    Task<MascotaDto?> ObtenerPorId(int id);
    Task<MascotaDto> Crear(CrearMascotaDto mascota);
    Task<MascotaDto?> Actualizar(int id, ActualizarMascotaDto mascota);
    Task<bool> Eliminar(int id);
}
