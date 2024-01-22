namespace ProveedorApi.Models;

public class Estado
{
    public string? estado { get; set; }
    public string? descripcion { get; set; }
    public string active { get; set; } = null!;
    public short orden { get; set; }
}