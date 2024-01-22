namespace ProveedorApi.Models.ContentBody;
public class SaveOnOffDeudaSunatBody
{
    public string ruc { get; set; } = null!;
    public string orden_compra { get; set; } = null!;
    public string embarque { get; set; } = null!;
    public string estado { get; set; } = null!;
    public bool notificacion_proveedor { get; set; }
}