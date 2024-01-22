namespace ProveedorApi.Models.ContentBody;
public class RequestAccessBody
{
    public string ruc { get; set; } = null!;
    public string razonsocial { get; set; } = null!;
    public string nombre { get; set; } = null!;
    public string apellido { get; set; } = null!;
    public string usuario { get; set; } = null!;
    public string email { get; set; } = null!;
}