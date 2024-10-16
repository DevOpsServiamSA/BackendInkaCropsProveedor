using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProveedorApi.Models;

public class MotivoRechazo
{
    [Required]
    public int motivo_rechazo { get; set; }
    [Required]
    public string descripcion { get; set; } = null!;
    public string mensaje_plantilla { get; set; } = null!;
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