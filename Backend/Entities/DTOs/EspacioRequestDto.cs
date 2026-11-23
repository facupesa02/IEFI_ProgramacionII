namespace IEFI.Entities.DTOs;

public class EspacioRequestDto
{
    public string Nombre { get; set; } = string.Empty;
    public int Tamano { get; set; }
    public int Capacidad { get; set; }
    public bool Estado { get; set; }
    public string Tipo { get; set; } = string.Empty;
}
