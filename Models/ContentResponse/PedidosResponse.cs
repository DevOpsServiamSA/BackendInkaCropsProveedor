using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProveedorApi.Models.ContentResponse;

public class PedidosResponse
{
    public string cliente { get; set; } = null!;
    public string nombre { get; set; } = null!;
    [Column(TypeName = "datetime")]
    public DateTime? fecha_pedido { get; set; }
    public string? estado { get; set; }
    public string? orden_compra { get; set; }
    public string articulo { get; set; } = null!;
    public string descripcion { get; set; } = null!;
    [Column(TypeName = "numeric(18,2)")]
    public decimal? pedida { get; set; }
    [Column(TypeName = "numeric(18,2)")]
    public decimal? facturada { get; set; }
    [Column(TypeName = "numeric(18,2)")]
    public decimal? saldo { get; set; }
}




