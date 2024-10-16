using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProveedorApi.Models;

public class Proveedor
{
    [Required]
    public string ruc { get; set; } = null!;
    [Required]
    public string razonsocial { get; set; } = null!;
    public string? direccion { get; set; }
    [Required]
    public string email { get; set; } = null!;
    public string? telefono { get; set; }
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