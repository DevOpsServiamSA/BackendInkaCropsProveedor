using System.ComponentModel.DataAnnotations;

namespace ProveedorApi.Models;

public class TipoArchivo
{
    [Required]
    public int tipo_archivo { get; set; }
    [Required]
    public string descripcion { get; set; } = null!;
    public string? extension { get; set; }
    [Required]
    public string active { get; set; } = null!;
}




