using IEFI.Business.Interfaces;
using IEFI.Entities.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace IEFI.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class EspacioController : ControllerBase
{
    private readonly IEspacioService _service;
    private readonly ILogger<EspacioController> _logger;

    public EspacioController(IEspacioService service, ILogger<EspacioController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var espacios = await _service.GetAllAsync();
            return Ok(espacios);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ocurrió un error al procesar la solicitud. {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var espacio = await _service.GetByIdAsync(id);
            if (espacio == null)
            {
                return NotFound("No se encontro el espacio con ese id.");
            }
            return Ok(espacio);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ocurrió un error al procesar la solicitud. {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(EspacioRequestDto espacio)
    {
        try
        {
            var error = await _service.CreateAsync(espacio);
            if (error != null)
            {
                return BadRequest(error);
            }
            return Ok("Espacio creado con exito.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ocurrió un error al procesar la solicitud. {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, EspacioRequestDto espacio)
    {
        try
        {
            var existente = await _service.GetByIdAsync(id);
            if (existente == null)
            {
                return NotFound("No se encontro el espacio con ese id.");
            }

            var error = await _service.UpdateAsync(id, espacio);
            if (error != null)
            {
                return BadRequest(error);
            }
            return Ok("Espacio actualizado con exito.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ocurrió un error al procesar la solicitud. {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest("El ID del espacio debe ser mayor que cero.");
            }

            var eliminado = await _service.DeleteAsync(id);
            if (!eliminado)
            {
                return NotFound("No se encontro el espacio con ese id.");
            }

            return Ok("Espacio eliminado con exito.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ocurrió un error al procesar la solicitud. {ex.Message}");
        }
    }
}
