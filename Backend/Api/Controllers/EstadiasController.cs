using System.Data.Common;
using IEFI.Entities.DTOs;
using IEFI.Business.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IEFI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EstadiasController : ControllerBase
{
    private readonly EstadiaService _estadiaService;
    private readonly ILogger<EstadiasController> _logger;

    public EstadiasController(
        EstadiaService estadiaService,
        ILogger<EstadiasController> logger)
    {
        _estadiaService = estadiaService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<EstadiaResponseDto>>> ObtenerTodas()
    {
        try
        {
            var estadias = await _estadiaService.ObtenerTodasAsync();
            return Ok(estadias);
        }
        catch (DbException exception)
        {
            _logger.LogError(exception, "Error al consultar las estadias.");
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new { mensaje = "No se pudieron consultar las estadias." });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EstadiaResponseDto>> ObtenerPorId(string id)
    {
        if (!TryParseId(id, out var estadiaId))
        {
            return BadRequest(new
            {
                mensaje = "El id debe ser un numero entero positivo dentro del rango permitido."
            });
        }

        try
        {
            var estadia = await _estadiaService.ObtenerPorIdAsync(estadiaId);
            if (estadia is null)
            {
                return NotFound(new { mensaje = $"No existe una estadia con id {estadiaId}." });
            }

            return Ok(estadia);
        }
        catch (DbException exception)
        {
            _logger.LogError(exception, "Error al consultar la estadia {EstadiaId}.", estadiaId);
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new { mensaje = "No se pudo consultar la estadia." });
        }
    }

    [HttpPost]
    public async Task<ActionResult<EstadiaResponseDto>> Crear(
        [FromBody] EstadiaRequestDto solicitud)
    {
        var errorValidacion = ValidarSolicitud(solicitud);
        if (errorValidacion is not null)
        {
            return BadRequest(new { mensaje = errorValidacion });
        }

        try
        {
            var errorCreacion = await _estadiaService.ValidarCreacionAsync(solicitud);
            if (errorCreacion is not null)
            {
                return BadRequest(new { mensaje = errorCreacion });
            }

            var estadia = await _estadiaService.AgregarAsync(solicitud);
            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = estadia.Id },
                estadia);
        }
        catch (DbUpdateException exception)
        {
            _logger.LogError(exception, "Error al crear una estadia.");
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new { mensaje = "Ocurrio un error al guardar la estadia." });
        }
        catch (DbException exception)
        {
            _logger.LogError(exception, "Error de base de datos al crear una estadia.");
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new { mensaje = "Ocurrio un error al guardar la estadia." });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(
        string id,
        [FromBody] EstadiaRequestDto solicitud)
    {
        if (!TryParseId(id, out var estadiaId))
        {
            return BadRequest(new
            {
                mensaje = "El id debe ser un numero entero positivo dentro del rango permitido."
            });
        }

        var errorValidacion = ValidarSolicitud(solicitud);
        if (errorValidacion is not null)
        {
            return BadRequest(new { mensaje = errorValidacion });
        }

        try
        {
            var actualizado = await _estadiaService.ActualizarAsync(estadiaId, solicitud);

            if (!actualizado)
            {
                return NotFound(new { mensaje = $"No existe una estadia con id {estadiaId}." });
            }

            return NoContent();
        }
        catch (DbUpdateException exception)
        {
            _logger.LogError(exception, "Error al actualizar la estadia {EstadiaId}.", estadiaId);
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new { mensaje = "Ocurrio un error al actualizar la estadia." });
        }
        catch (DbException exception)
        {
            _logger.LogError(exception, "Error de base de datos al actualizar la estadia {EstadiaId}.", estadiaId);
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new { mensaje = "Ocurrio un error al actualizar la estadia." });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(string id)
    {
        if (!TryParseId(id, out var estadiaId))
        {
            return BadRequest(new
            {
                mensaje = "El id debe ser un numero entero positivo dentro del rango permitido."
            });
        }

        try
        {
            var eliminado = await _estadiaService.EliminarAsync(estadiaId);
            if (!eliminado)
            {
                return NotFound(new { mensaje = $"No existe una estadia con id {estadiaId}." });
            }

            return NoContent();
        }
        catch (DbUpdateException exception)
        {
            _logger.LogError(exception, "Error al eliminar la estadia {EstadiaId}.", estadiaId);
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new { mensaje = "Ocurrio un error al eliminar la estadia." });
        }
        catch (DbException exception)
        {
            _logger.LogError(exception, "Error de base de datos al eliminar la estadia {EstadiaId}.", estadiaId);
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new { mensaje = "Ocurrio un error al eliminar la estadia." });
        }
    }

    private static bool TryParseId(string value, out int id)
    {
        return int.TryParse(value, out id) && id > 0;
    }

    private static string? ValidarSolicitud(EstadiaRequestDto solicitud)
    {
        if (solicitud.MascotaId <= 0)
        {
            return "El id de la mascota debe ser mayor que cero.";
        }

        if (solicitud.EspacioId <= 0)
        {
            return "El id del espacio debe ser mayor que cero.";
        }

        if (solicitud.FechaIngreso.Date < DateTime.Today)
        {
            return "La fecha de ingreso no puede ser anterior a la fecha actual.";
        }

        if (solicitud.FechaSalida.Date < solicitud.FechaIngreso.Date)
        {
            return "La fecha de salida no puede ser anterior a la fecha de ingreso.";
        }

        return null;
    }
}
