using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProveedorApi.Models.ContentResponse;

public class TransporteSegResponse
{
    public int requerimiento { get; set; }
    public string documento { get; set; } = null!;
    [Column(TypeName = "datetime")]
    public DateTime? fecha_solicitada { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime? fecha_programacion { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime? fecha_hora_entregada { get; set; }
    public string turno_programacion { get; set; } = null!;
    public string tipo_programacion { get; set; } = null!;
    public string estado_requerimiento { get; set; } = null!;
    public byte estado { get; set; }
    public string prioridad_requerimiento { get; set; } = null!;
    public string origen_ruc { get; set; } = null!;
    public string origen_razon_social { get; set; } = null!;
}




