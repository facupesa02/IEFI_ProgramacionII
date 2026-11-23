using System.Dynamic;

namespace IEFI.Entities.Models;

public class Espacio
{
    private int id;
    private string nombre;
    private int tamano;
    private int capacidad;
    private bool estado;
    private string tipo;

    public int Id{get; set;}
    public string Nombre{get; set;}
    public int Tamano{get; set;}
    public int Capacidad{get; set;}
    public bool Estado{get; set;}
    public string Tipo{get; set;}
}