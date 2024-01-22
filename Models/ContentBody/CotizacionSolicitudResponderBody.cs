namespace  ProveedorApi.Models.ContentBody;

public class CotizacionSolicitudResponderBody
{
    public int cotcodigo { get; set; }
    public int coscodigo { get; set; }
    public int cspcodigo { get; set; }
    public List<CotizacionSolicitudDetalleResponderBody> detalle { get; set; } = null!;
}
public class CotizacionSolicitudDetalleResponderBody
{
    public int item { get; set; }
    public string articulo { get; set; } = null!;
    public string moneda { get; set; } = null!;
    public decimal? precio { get; set; }
    public decimal? descuento { get; set; }
    public int dias { get; set; }
    public string? observacion { get; set; }
}