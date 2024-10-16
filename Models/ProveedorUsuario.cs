using System.ComponentModel.DataAnnotations.Schema;

namespace ProveedorApi.Models;

public class ProveedorUsuario
{
    public string ruc { get; set; } = null!;
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
    public string? token_reset { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime? token_reset_expire { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime? token_reset_request { get; set; }
}