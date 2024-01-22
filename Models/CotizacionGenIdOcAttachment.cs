namespace ProveedorApi.Models;

public class CotizacionGenIdOcAttachment
{
    public int cot_codigo { get; set; }
    public byte cos_codigo { get; set; }
    public int csp_codigo { get; set; }
    public int csa_item { get; set; }
    public string csa_filename_original { get; set; } = null!;
    public byte[] csa_file_binary { get; set; } = null!;
}