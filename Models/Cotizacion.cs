namespace ProveedorApi.Models;

public class Cotizacion : CotizacionBase
{
    public string cot_descripcion { get; set; } = null!;
    public DateTime cot_fecha_registro { get; set; }
    public DateTime? cot_fecha_finalizacion { get; set; }
    public byte ces_codigo { get; set; }
    public string? observacion { get; set; }
    public DateTime? cot_fecha_requerida { get; set; }
    public bool puja { get; set; }
    public string? archivos { get; set; }
}