using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProveedorApi.Models.ContentResponse;

public class OCBitacoraResponse
{
    public string orden_compra { get; set; } = null!;
    public string embarque { get; set; } = null!;
    public int? linea_bitacora { get; set; }
    public string estado { get; set; } = null!;
    public string estado_descripcion { get; set; } = null!;
    public string comentario { get; set; } = null!;
    [Column(TypeName = "datetime")]
    public DateTime fecha { get; set; }
    public string icon { get; set; } = null!;
    public string color { get; set; } = null!;
}




