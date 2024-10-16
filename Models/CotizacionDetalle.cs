namespace ProveedorApi.Models;

public class CotizacionDetalle : CotizacionBase
{
    public byte cod_codigo { get; set; }
    public string articulo { get; set; } = null!;
    public decimal cod_cantidad { get; set; }
    public byte unm_codigo { get; set; }
}