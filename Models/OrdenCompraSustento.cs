using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProveedorApi.Models;

public class OrdenCompraSustento
{
    public string orden_compra { get; set; } = null!;
    public string embarque { get; set; } = null!;
    public int? linea_sustento { get; set; }
    public string nombre_archivo_original { get; set; } = null!;
    public string nombre_archivo { get; set; } = null!;
    public string? aceptado { get; set; }
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