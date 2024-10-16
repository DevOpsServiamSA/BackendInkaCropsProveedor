using System.ComponentModel.DataAnnotations;

namespace ProveedorApi.Models;

public class TipoRequisito
{
    [Required]
    public int tipo_requisito { get; set; }
    [Required]
    public string descripcion { get; set; } = null!;
    public int tipo_archivo { get; set; }
    [Required]
    public string active { get; set; } = null!;
}