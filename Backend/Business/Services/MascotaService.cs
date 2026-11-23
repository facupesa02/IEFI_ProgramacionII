using IEFI.Business.Interfaces;
using IEFI.Data.Repositories.Interfaces;
using IEFI.Entities.DTOs;

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
        return await _repository.ObtenerTodas();
    }

    public async Task<MascotaDto?> ObtenerPorId(int id)
    {
        return await _repository.ObtenerPorId(id);
    }

    public async Task<MascotaDto> Crear(CrearMascotaDto mascota)
    {
        return await _repository.Crear(mascota);
    }

    public async Task<MascotaDto?> Actualizar(int id, ActualizarMascotaDto mascota)
    {
        return await _repository.Actualizar(id, mascota);
    }

    public async Task<bool> Eliminar(int id)
    {
        return await _repository.Eliminar(id);
    }
}
