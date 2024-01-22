using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProveedorApi.Models.ContentResponse;

public class OCRequisitosResponse
{
    public string orden_compra { get; set; } = null!;
    public string embarque { get; set; } = null!;
    public int tipo_comprobante { get; set; }
    public string tipo_comprobante_descripcion { get; set; } = null!;
    public int tipo_requisito { get; set; }
    public string tipo_requisito_descripcion { get; set; } = null!;
    public int? linea_requisito { get; set; }
    public int? item { get; set; }
    public string? valor { get; set; }
    public string? aceptado { get; set; }
    public int tipo_archivo { get; set; }
    public string tipo_archivo_descripcion { get; set; } = null!;
    public string? extension { get; set; }
    public bool loading { get; set; }
}




