namespace ProveedorApi.Models;

public class CotizacionSolicitudProveedorAttachment : CotizacionBase
{
    public byte cos_codigo { get; set; }
    public int csp_codigo { get; set; }
    public int csa_item { get; set; }
    public string csa_filename_original { get; set; } = null!;
    public string csa_filename { get; set; } = null!;
}