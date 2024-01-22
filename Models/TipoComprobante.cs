using System.ComponentModel.DataAnnotations;

namespace ProveedorApi.Models;

public class TipoComprobante
{
    [Required]
    public int tipo_comprobante {get; set;}
    [Required]
    public string descripcion {get; set;} = null!;
    [Required]
    public string active { get; set; } = null!;
}