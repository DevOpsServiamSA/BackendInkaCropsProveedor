using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProveedorApi.Models.ContentResponse;

public class OrdenCompraResponse
{
    public string ruc { get; set; } = null!;
    public string razonsocial { get; set; } = null!;
    public string orden_compra { get; set; } = null!;
    public string? guia_remision { get; set; }
    public string? comprobante { get; set; }
    public string? embarque { get; set; }
    public int tipo_comprobante { get; set; }
    public string tipo_comprobante_descripcion { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? fecha_registro { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime? fecha_vencimiento { get; set; }
    [Column(TypeName = "date")]
    public DateTime? fecha_programada_pago { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime? fecha_pago { get; set; }
    public string moneda { get; set; } = null!;

    [Column(TypeName = "numeric(18,2)")]
    public decimal monto_total { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal? monto_retencion { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? monto_pagado { get; set; }
    public string url_retencion { get; set; } = null!;
    public string? estado { get; set; }
    public string? estado_descripcion { get; set; }
    public string? comentario { get; set; }
    public string? rechazado { get; set; }
    public string? nota_credito { get; set; }
    public int? motivo_rechazo { get; set; }
    public string? motivo_rechazo_descripcion { get; set; }
    public string condicion_pago { get; set; } = null!;
    public string modulo_origen { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime? fecha_carga_documentos { get; set; }
}   




