namespace ProveedorApi.Models;

public class CotizacionDetalleOriginal : CotizacionBase
{
    public byte cod_codigo { get; set; }
    public byte codd_codigo { get; set; }
    public string articulo { get; set; } = null!;
    public decimal cod_cantidad { get; set; }
    public byte unm_codigo { get; set; }
    public string? cod_observacion { get; set; }
    public string? nro_solicitud { get; set; }
    public int linea_solicitud { get; set; }
}