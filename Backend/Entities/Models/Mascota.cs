namespace IEFI.Entities.Models;

public class Mascota
{
    private int id;
    private string nombre;
    private string especie;
    private string raza;
    private int edad;
    private decimal peso;
    
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Especie { get; set; }
    public string Raza { get; set; }
    public int Edad { get; set; }
    public decimal Peso { get; set; }
}