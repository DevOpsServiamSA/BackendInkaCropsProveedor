namespace ProveedorApi.Models.ContentBody;
public class DeleteItemSustentoBody
{
    public string p_ruc { get; set; } = null!;
    public string p_orden_compra { get; set; } = null!;
    public string p_embarque { get; set; } = null!;
    public int linea { get; set; }
}