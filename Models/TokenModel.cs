namespace ProveedorApi.Models;
public class TokenModel
{
    public string? ruc { get; set; }
    public string? razonsocial { get; set; }
    public string username { get; set; } = null!;
    public string nombre { get; set; } = null!;
    public string apellido { get; set; } = null!;
    public string email { get; set; } = null!;
    public int perfil_id { get; set; }
    public bool isproveedor { get; set; }
    public bool read_only { get; set; }
}