namespace IEFI.Entities.Models;

public class Estadia
{
    public int Id { get; set; }
    public int MascotaId { get; set; }
    public Mascota Mascota { get; set; } = null!;
    public int EspacioId { get; set; }
    public Espacio Espacio { get; set; } = null!;
    public DateTime FechaIngreso { get; set; }
    public DateTime FechaSalida { get; set; }

    public Estadia()
    {
    }

    public Estadia(
        Mascota mascota,
        Espacio espacio,
        DateTime fechaIngreso,
        DateTime fechaSalida)
    {
        ArgumentNullException.ThrowIfNull(mascota);
        ArgumentNullException.ThrowIfNull(espacio);

        Mascota = mascota;
        MascotaId = mascota.Id;
        Espacio = espacio;
        EspacioId = espacio.Id;
        FechaIngreso = fechaIngreso;
        FechaSalida = fechaSalida;
    }
}
