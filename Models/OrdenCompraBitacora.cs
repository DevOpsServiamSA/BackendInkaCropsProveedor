using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProveedorApi.Models;

public class OrdenCompraBitacora
{
    [Required]
    public string orden_compra { get; set; } = null!;
    [Required]
    public string embarque { get; set; } = null!;
    public int? linea_bitacora { get; set; }
    public string estado { get; set; } = null!;
    public int? motivo_rechazo { get; set; }
    public string comentario { get; set; } = null!;
    public string active { get; set; } = null!;
    public string? created_by { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime created_at { get; set; }
    public string? updated_by { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime? updated_at { get; set; }
    public string? deleted_by { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime? deleted_at { get; set; }
}




