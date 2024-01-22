using System.ComponentModel.DataAnnotations;

namespace ProveedorApi.Models;

public class TipoComprobanteRequisito
{
    [Required]
    public int tipo_comprobante {get; set;}
    [Required]
    public int tipo_requisito {get; set;}
}