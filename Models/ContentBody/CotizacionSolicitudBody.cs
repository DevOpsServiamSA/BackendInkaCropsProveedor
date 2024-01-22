namespace  ProveedorApi.Models.ContentBody;

public class CotizacionSolicitudBody
{
    public int cotcodigo { get; set; }
    public DateTime fecha_vigencia_ini { get; set; }
    public DateTime fecha_vigencia_fin { get; set; }
    public List<CotizacionSolicitudProveedorBody> proveedores { get; set; } = null!;
}

public class CotizacionSolicitudProveedorBody
{
    public int csp_codigo { get; set; }
    public string proveedor { get; set; } = null!; //RUC
    public string razonsocial { get; set; } = null!;
    public string? email { get; set; }
    public string? condicion_pago { get; set; }
}


public class CotizacionSolicitudChangeVigencia
{
    public int cot { get; set; }
    public int cos { get; set; }
    public DateTime vigencia_ini { get; set; }
    public DateTime vigencia_fin { get; set; }
    public List<CotizacionSolicitudProveedorBody> proveedores { get; set; } = null!;
}