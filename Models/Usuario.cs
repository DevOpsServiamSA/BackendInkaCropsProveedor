using System.ComponentModel.DataAnnotations.Schema;

namespace ProveedorApi.Models;

public class Usuario
{
    public string username { get; set; } = null!;
    public byte[] password { get; set; } = null!;
    public string nombre { get; set; } = null!;
    public string apellido { get; set; } = null!;
    public string email { get; set; } = null!;
    public int perfil_id { get; set; }
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
    public bool read_only { get; set; }
}