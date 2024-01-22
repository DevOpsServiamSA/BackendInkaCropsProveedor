using Microsoft.EntityFrameworkCore;
using ProveedorApi.Models;
using ProveedorApi.Models.StoreProcedure;

namespace ProveedorApi.Data;

public class ExactusExtContext : DbContext
{
    public ExactusExtContext(DbContextOptions<ExactusExtContext> options) : base(options) { }

    #region Content Response (Store procedure)
    public DbSet<SpForJson> SpForJson => Set<SpForJson>();
    #endregion

    public DbSet<Cotizacion> Cotizacion => Set<Cotizacion>();
    public DbSet<CotizacionDetalle> CotizacionDetalle => Set<CotizacionDetalle>();
    public DbSet<CotizacionDetalleOriginal> CotizacionDetalleOriginal => Set<CotizacionDetalleOriginal>();
    public DbSet<CotizacionSolicitud> CotizacionSolicitud => Set<CotizacionSolicitud>();
    public DbSet<CotizacionSolicitudProveedor> CotizacionSolicitudProveedor => Set<CotizacionSolicitudProveedor>();
    public DbSet<CotizacionSolicitudProveedorDetalle> CotizacionSolicitudProveedorDetalle => Set<CotizacionSolicitudProveedorDetalle>();
    public DbSet<CotizacionSolicitudProveedorAttachment> CotizacionSolicitudProveedorAttachment => Set<CotizacionSolicitudProveedorAttachment>();
    public DbSet<CotizacionGenIdOc> CotizacionGenIdOc => Set<CotizacionGenIdOc>();
    public DbSet<CotizacionBitacora> CotizacionBitacora => Set<CotizacionBitacora>();
    public DbSet<CotizacionGenIdOcAttachment> CotizacionGenIdOcAttachment => Set<CotizacionGenIdOcAttachment>();

    #region STORE PROCEDURE
    public DbSet<SPCotizacionAttachmentsForGenId> SPCotizacionAttachmentsForGenId => Set<SPCotizacionAttachmentsForGenId>();
    #endregion
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Store Procedure Query Select
        modelBuilder.Entity<SpForJson>().HasNoKey();
        #endregion

        modelBuilder.Entity<Cotizacion>().ToTable("Com_cotizacion").HasKey(x => x.cot_codigo);
        modelBuilder.Entity<CotizacionDetalle>().ToTable("Com_Cotizacion_Detalle").HasKey(x => new { x.cot_codigo, x.cod_codigo });
        modelBuilder.Entity<CotizacionDetalleOriginal>().ToTable("Com_Cotizacion_Detalle_Original").HasKey(x => new { x.cot_codigo, x.cod_codigo, x.codd_codigo });
        modelBuilder.Entity<CotizacionSolicitud>().ToTable("Com_Cotizacion_Solicitud").HasKey(x => new { x.cot_codigo, x.cos_codigo });
        modelBuilder.Entity<CotizacionSolicitudProveedor>().ToTable("Com_Cotizacion_Solicitud_Proveedor").HasKey(x => new { x.cot_codigo, x.cos_codigo, x.csp_codigo });
        modelBuilder.Entity<CotizacionSolicitudProveedorDetalle>().ToTable("Com_Cotizacion_Solicitud_Proveedor_Detalle").HasKey(x => new { x.cot_codigo, x.cos_codigo, x.csp_codigo, x.csd_item });
        modelBuilder.Entity<CotizacionSolicitudProveedorAttachment>().ToTable("Com_Cotizacion_Solicitud_Proveedor_Attachment").HasKey(x => new { x.cot_codigo, x.cos_codigo, x.csp_codigo, x.csa_item });
        modelBuilder.Entity<CotizacionGenIdOc>().ToTable("Com_Cotizacion_GenIdOc").HasKey(x => new { x.gen_id });
        modelBuilder.Entity<CotizacionBitacora>().ToTable("Com_Cotizacion_Bitacora").HasKey(x => new { x.cot_codigo, x.linea_bitacora });
        modelBuilder.Entity<CotizacionGenIdOcAttachment>().ToTable("Com_Cotizacion_GenIdOc_Attachment").HasKey(x => new { x.cot_codigo, x.cos_codigo, x.csp_codigo, x.csa_item });

        #region Store Procedure
        modelBuilder.Entity<SPCotizacionAttachmentsForGenId>().HasNoKey();
        #endregion
    }
}