namespace  ProveedorApi.Models.ContentBody;

public class CotizacionPrecioElegido
{
    public int cotcodigo { get; set; }
    public int coscodigo { get; set; }
    public int cspcodigo { get; set; }
    public int item { get; set; }
    public string articulo { get; set; } = null!;
    public bool elegido { get; set; }
}