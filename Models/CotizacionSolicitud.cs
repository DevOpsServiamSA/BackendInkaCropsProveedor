namespace ProveedorApi.Models;

public class CotizacionSolicitud : CotizacionBase
{
    public byte cos_codigo { get; set; }
    public DateTime cos_fecha_vigencia_ini { get; set; }
    public DateTime cos_fecha_vigencia_fin { get; set; }
    public byte ces_codigo { get; set; }
}