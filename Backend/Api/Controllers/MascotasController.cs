using IEFI.Business.Interfaces;
using IEFI.Entities.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace IEFI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MascotasController : ControllerBase
{
    private readonly IMascotaService _service;
    private readonly ILogger<MascotasController> _logger;

    public MascotasController(IMascotaService service, ILogger<MascotasController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<MascotaDto>>> ObtenerTodas()
    {
        try
        {
            List<MascotaDto> mascotas = await _service.ObtenerTodas();
            return Ok(mascotas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener las mascotas");
            return StatusCode(500, "Error al obtener las mascotas.");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MascotaDto>> ObtenerPorId(int id)
    {
        try
        {
            MascotaDto? mascota = await _service.ObtenerPorId(id);
            if (mascota == null)
            {
                return NotFound();
            }

            return Ok(mascota);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la mascota con Id {MascotaId}", id);
            return StatusCode(500, "Error al obtener la mascota.");
        }
    }

    [HttpPost]
    public async Task<ActionResult<MascotaDto>> Crear(CrearMascotaDto mascota)
    {
        try
        {
            MascotaDto creada = await _service.Crear(mascota);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = creada.Id }, creada);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear la mascota");
            return StatusCode(500, "Error al crear la mascota.");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MascotaDto>> Actualizar(int id, ActualizarMascotaDto mascota)
    {
        try
        {
            MascotaDto? actualizada = await _service.Actualizar(id, mascota);
            if (actualizada == null)
            {
                return NotFound();
            }

            return Ok(actualizada);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar la mascota con Id {MascotaId}", id);
            return StatusCode(500, "Error al actualizar la mascota.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        try
        {
            bool eliminada = await _service.Eliminar(id);
            if (!eliminada)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar la mascota con Id {MascotaId}", id);
            return StatusCode(500, "Error al eliminar la mascota.");
        }
    }
}
