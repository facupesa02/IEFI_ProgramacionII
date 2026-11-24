using IEFI.Entities.DTOs;

namespace IEFI.Data.Repositories.Interfaces;

public interface IEspacioRepository
{
    Task<List<EspacioResponseDto>> GetAllAsync();
    Task<EspacioResponseDto?> GetByIdAsync(int id);
    Task CreateAsync(EspacioRequestDto newEspacio);
    Task<bool> UpdateAsync(int id, EspacioRequestDto espacioActualizado);
    Task<bool> DeleteAsync(int id);
}
