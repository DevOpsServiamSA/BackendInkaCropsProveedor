namespace ProveedorApi.Models;

public class CotizacionGenIdOc
{
    public int gen_id { get; set; }
    public string? nro_oc { get; set; }
    public bool activo { get; set; }
    public string aud_usuario_reg { get; set; } = null!;
    public DateTime aud_fecha_reg { get; set; }
    public string? aud_usuario_act { get; set; }
    public DateTime? aud_fecha_act { get; set; }
    public string? aud_usuario_eli { get; set; }
    public DateTime? aud_fecha_eli { get; set; }
}