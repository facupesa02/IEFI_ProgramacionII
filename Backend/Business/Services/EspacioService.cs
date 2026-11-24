using IEFI.Data.Repositories.Implementations;
using IEFI.Entities.DTOs;

namespace IEFI.Business.Services;

public class EspacioService
{
    private readonly EspacioRepository _repository;

    public EspacioService(EspacioRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<EspacioResponseDto>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<EspacioResponseDto?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<string?> CreateAsync(EspacioRequestDto newEspacio)
    {
        if (string.IsNullOrWhiteSpace(newEspacio.Nombre))
        {
            return "El nombre no puede estar vacio";
        }
        if (string.IsNullOrWhiteSpace(newEspacio.Tipo))
        {
            return "El tipo no puede estar vacio";
        }
        if (newEspacio.Tamano <= 0)
        {
            return "El tamano debe ser mayor a 0";
        }
        if (newEspacio.Capacidad <= 0)
        {
            return "La capacidad debe ser mayor a 0";
        }

        await _repository.CreateAsync(newEspacio);
        return null;
    }

    public async Task<string?> UpdateAsync(int id, EspacioRequestDto espacioActualizado)
    {
        if (string.IsNullOrWhiteSpace(espacioActualizado.Nombre))
        {
            return "El nombre no puede estar vacio";
        }
        if (string.IsNullOrWhiteSpace(espacioActualizado.Tipo))
        {
            return "El tipo no puede estar vacio";
        }
        if (espacioActualizado.Tamano <= 0)
        {
            return "El tamano debe ser mayor a 0";
        }
        if (espacioActualizado.Capacidad <= 0)
        {
            return "La capacidad debe ser mayor a 0";
        }

        await _repository.UpdateAsync(id, espacioActualizado);
        return null;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}
