namespace ProveedorApi.Models.ContentBody;

public class EliminarRequisitoBody
{
    public string p_ruc { get; set; } = null!;
    public string p_orden_compra { get; set; } = null!;
    public string p_embarque { get; set; } = null!;
    public int tipo { get; set; }
    public int item { get; set; }
    public string filename { get; set; } = null!;
}