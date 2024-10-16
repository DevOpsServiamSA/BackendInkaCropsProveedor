namespace ProveedorApi.Models;

public class CotizacionSolicitudProveedorDetalle : CotizacionBase
{
    public byte cos_codigo { get; set; }
    public int csp_codigo { get; set; }
    public int csd_item { get; set; }
    public string articulo { get; set; } = null!;
    public decimal csd_cantidad { get; set; }
    public byte unm_codigo { get; set; }
    public string? mon_codigo { get; set; }
    public decimal? csd_precio { get; set; }
    public decimal? csd_descuento { get; set; }
    public int? csd_dias_entrega { get; set; }
    public string? csd_observacion { get; set; }
    public bool csd_elegido { get; set; }
    public int? gen_id { get; set; }
}