namespace ProveedorApi.Models.ContentBody;
public class NotificarRechazoBody
{
    public string ruc { get; set; } = null!;
    public string orden_compra { get; set; } = null!;
    public string embarque { get; set; } = null!;
    public bool rechazado { get; set; }
    public int motivo_rechazo { get; set; }
    public string motivo_rechazo_descripcion { get; set; } = null!;
    public string? mensaje_notificacion { get; set; }
    public bool solicita_nota_credito { get; set; }
    public List<NotificarRechazoRequisitosDetalleBody>? requisitos { get; set; }
    public List<NotificarRechazoSustentosDetalleBody>? sustentos { get; set; }
}
public class NotificarRechazoRequisitosDetalleBody
{
    public int tipo_requisito { get; set; }
    public string tipo_requisito_descripcion { get; set; } = null!;
    public int linea_requisito { get; set; }
    public int item { get; set; }
    public string valor { get; set; } = null!;

    public string? aceptado { get; set; }
}

public class NotificarRechazoSustentosDetalleBody
{
    public int linea_sustento { get; set; }
    public string nombre_archivo_original { get; set; } = null!;
    public string nombre_archivo { get; set; } = null!;
    public string? aceptado { get; set; }
}