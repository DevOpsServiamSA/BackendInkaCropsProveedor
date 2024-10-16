namespace  ProveedorApi.Models.ContentBody;

public class CotizacionBody
{
    public string descripcion { get; set; } = null!;
    public DateTime fecha_requerida { get; set; }
    public string? observacion { get; set; }
    public bool puja { get; set; }
    public List<string>? archivos { get; set; }
    public List<CotizacionDetalleBody> detalle { get; set; } = null!;
}

public class CotizacionDetalleBody
{
    public byte cod_codigo { get; set; }
    public string articulo { get; set; } = null!;
    public decimal cantidad { get; set; }
    public byte unidad { get; set; }
    public List<CotizacionDetalleOriginalBody> original { get; set; } = null!;
}


public class CotizacionDetalleOriginalBody
{
    public byte codd_codigo { get; set; }
    public string solicitud { get; set; } = null!;
    public int solicitud_linea { get; set; }
    public decimal cantidad { get; set; }
    public byte unidad { get; set; }
    public string? observacion { get; set; }
}