namespace ProveedorApi.Models;
public class SolicitudAccesoBody
{
    public int id { get; set; }
    public string ruc { get; set; } = null!;
    public string usuario { get; set; } = null!;
    public string estado { get; set; } = null!;
}