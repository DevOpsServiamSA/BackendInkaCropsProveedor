namespace ProveedorApi.Models;

public class CotizacionBitacora : CotizacionBase
{
    public int linea_bitacora { get; set; }
    public byte ces_codigo { get; set; }
    public string? comentario { get; set; }
}