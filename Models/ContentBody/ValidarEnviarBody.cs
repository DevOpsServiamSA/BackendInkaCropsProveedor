namespace ProveedorApi.Models.ContentBody;
public class ValidarEnviarBody
{
    public string ruc { get; set; } = null!;
    
    public string subcarpeta { get; set; } = null!;
    public string orden_compra { get; set; } = null!;
    public string embarque { get; set; } = null!;
}