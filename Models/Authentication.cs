namespace ProveedorApi.Models;

public class Authentication
{
    public string? ruc { get; set; }
    public string username { get; set; } = null!;
    public string password { get; set; } = null!;
    public bool isproveedor { get; set; }
    public string recaptcha { get; set; } = null!;
}