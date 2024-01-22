using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProveedorApi.Models;

public class OrdenCompraEmbarqueEstado
{
    [Required]
    public string orden_compra { get; set; } = null!;
    [Required]
    public string embarque { get; set; } = null!;
    public string estado { get; set; } = null!;
    public string? created_by { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime created_at { get; set; }
    public string? updated_by { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime? updated_at { get; set; }
    public string? rechazado { get; set; }
    public int? motivo_rechazo { get; set; }
    public string? nota_credito { get; set; }
}