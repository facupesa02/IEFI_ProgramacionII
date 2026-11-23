namespace IEFI.Entities.Models;

public class Mascota
{
    public int Id { get; set; }
    public required string Nombre { get; set; }
    public required string Especie { get; set; }
    public required string Raza { get; set; }
    public int Edad { get; set; }
    public decimal Peso { get; set; }
}
