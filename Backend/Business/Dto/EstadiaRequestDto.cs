namespace IEFI.Business.Dto;

public class EstadiaRequestDto
{
    public int MascotaId { get; set; }
    public int EspacioId { get; set; }
    public DateTime FechaIngreso { get; set; }
    public DateTime FechaSalida { get; set; }
}
