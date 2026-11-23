using IEFI.Entities.DTOs;

namespace IEFI.Business.Interfaces;

public interface IEspacioService
{
    Task<List<EspacioResponseDto>> GetAllAsync();
    Task<EspacioResponseDto?> GetByIdAsync(int id);
    Task<string?> CreateAsync(EspacioRequestDto newEspacio);
    Task<string?> UpdateAsync(int id, EspacioRequestDto espacioActualizado);
    Task<bool> DeleteAsync(int id);
}
