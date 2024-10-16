namespace ProveedorApi.Models;

public class CotizacionBase
{
    public int cot_codigo { get; set; }
    public bool activo { get; set; }
    public string aud_usuario_reg { get; set; } = null!;
    public DateTime aud_fecha_reg { get; set; }
    public string? aud_usuario_act { get; set; }
    public DateTime? aud_fecha_act { get; set; }
    public string? aud_usuario_eli { get; set; }
    public DateTime? aud_fecha_eli { get; set; }
}