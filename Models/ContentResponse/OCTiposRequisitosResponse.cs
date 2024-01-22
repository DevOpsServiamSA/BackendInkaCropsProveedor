using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProveedorApi.Models.ContentResponse;

public class OCTiposRequisitosResponse
{
    public int key { get; set; }
    public string valor { get; set; } = null!;
    public string? extension { get; set; }
}




