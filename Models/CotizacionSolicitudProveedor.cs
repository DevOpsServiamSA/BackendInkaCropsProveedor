namespace ProveedorApi.Models;

public class CotizacionSolicitudProveedor : CotizacionBase
{
    public byte cos_codigo { get; set; }
    public int csp_codigo { get; set; }
    public string proveedor { get; set; } = null!;
    public bool csp_atendido { get; set; }
    public string? csp_condicion_pago { get; set; }
}