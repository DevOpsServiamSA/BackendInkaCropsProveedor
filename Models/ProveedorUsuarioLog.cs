using System.ComponentModel.DataAnnotations.Schema;

namespace ProveedorApi.Models;

public class ProveedorUsuarioLog
{
    public int id { get; set; }
    [Column(TypeName = "date")]
    public DateTime fecha { get; set; }
    public string ruc { get; set; } = null!;
    public string username { get; set; } = null!;
    [Column(TypeName = "datetime")]
    public DateTime audfecha { get; set; }
}