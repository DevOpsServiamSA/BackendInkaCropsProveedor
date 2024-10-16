namespace ProveedorApi.Models;
using System.ComponentModel.DataAnnotations.Schema;
public class SolicitudAcceso
{
    public int id { get; set; }
    public string ruc { get; set; } = null!;
    public string razonsocial { get; set; } = null!;
    public string nombre { get; set; } = null!;
    public string apellido { get; set; } = null!;
    public string usuario { get; set; } = null!;
    public string email { get; set; } = null!;
    public string estado { get; set; } = null!;
    public string active { get; set; } = null!;
    [Column(TypeName = "datetime")]
    public DateTime? created_at { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime? updated_at { get; set; }
    public string? updated_by { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime? deleted_at { get; set; }
    public string? deleted_by { get; set; }
    public string? password { get; set; }
}