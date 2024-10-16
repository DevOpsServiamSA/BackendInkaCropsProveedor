namespace ProveedorApi.Models.ContentBody;
public class SaveRequisitoNoFileBody
{
    public string? orden_compra { get; set; }
    public string? embarque { get; set; }
    public int linea { get; set; }
    public int tipo_requisito { get; set; }
    public int item { get; set; }
    public string? valor { get; set; }
}