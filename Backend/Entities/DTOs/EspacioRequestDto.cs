namespace IEFI.Entities.DTOs;

public class EspacioRequestDto
{
    public string Nombre { get; set; }
    public int Tamano { get; set; }
    public int Capacidad { get; set; }
    public bool Estado { get; set; }
    public string Tipo { get; set; }
}
