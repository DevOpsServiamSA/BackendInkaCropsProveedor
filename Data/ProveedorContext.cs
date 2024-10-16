using Microsoft.EntityFrameworkCore;
using ProveedorApi.Models;
using ProveedorApi.Models.ContentResponse;

namespace ProveedorApi.Data;

public class ProveedorContext : DbContext
{
    public ProveedorContext(DbContextOptions<ProveedorContext> options) : base(options) { }
    #region Autenticación
    public DbSet<SpForJson> SpForJson => Set<SpForJson>();
    public DbSet<Perfil> Perfil => Set<Perfil>();
    public DbSet<Usuario> Usuario => Set<Usuario>();
    public DbSet<Proveedor> Proveedor => Set<Proveedor>();
    public DbSet<ProveedorUsuario> ProveedorUsuario => Set<ProveedorUsuario>();
    public DbSet<ProveedorUsuarioLog> ProveedorUsuarioLog => Set<ProveedorUsuarioLog>();
    public DbSet<SolicitudAcceso> SolicitudAcceso => Set<SolicitudAcceso>();
    #endregion

    #region TablasMaestras
    public DbSet<Estado> Estado => Set<Estado>();
    public DbSet<MotivoRechazo> MotivoRechazo => Set<MotivoRechazo>();
    public DbSet<Parametro> Parametro => Set<Parametro>();
    public DbSet<TipoArchivo> TipoArchivo => Set<TipoArchivo>();
    public DbSet<TipoComprobante> TipoComprobante => Set<TipoComprobante>();
    public DbSet<TipoRequisito> TipoRequisito => Set<TipoRequisito>();
    public DbSet<TipoComprobanteRequisito> TipoComprobanteRequisito => Set<TipoComprobanteRequisito>();
    #endregion

    #region TutorialVideo
    public DbSet<TutorialVideo> TutorialVideo => Set<TutorialVideo>();
    #endregion

    #region Orden de Compra
    // public DbSet<OrdenCompra> OrdenCompra => Set<OrdenCompra>();
    public DbSet<OrdenCompraEmbarqueEstado> OrdenCompraEmbarqueEstado => Set<OrdenCompraEmbarqueEstado>();
    public DbSet<OrdenCompraBitacora> OrdenCompraBitacora => Set<OrdenCompraBitacora>();
    public DbSet<OrdenCompraBitacoraRequisito> OrdenCompraBitacoraRequisito => Set<OrdenCompraBitacoraRequisito>();
    public DbSet<OrdenCompraRequisito> OrdenCompraRequisito => Set<OrdenCompraRequisito>();
    public DbSet<OrdenCompraSustento> OrdenCompraSustento => Set<OrdenCompraSustento>();
    #endregion

    #region Content Response (Store procedure)
    public DbSet<OrdenCompraResponse> OrdenCompraResponse => Set<OrdenCompraResponse>();
    public DbSet<OCBitacoraResponse> OCBitacoraResponse => Set<OCBitacoraResponse>();
    public DbSet<OCRequisitosResponse> OCRequisitosResponse => Set<OCRequisitosResponse>();
    public DbSet<OCRequisitoForMailResponse> OCRequisitoForMailResponse => Set<OCRequisitoForMailResponse>();
    public DbSet<OCTiposRequisitosResponse> OCTiposRequisitosResponse => Set<OCTiposRequisitosResponse>();
    public DbSet<ExisteResponse> ExisteResponse => Set<ExisteResponse>();

    public DbSet<PedidosResponse> PedidosResponse => Set<PedidosResponse>();
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Autenticación
        modelBuilder.Entity<SpForJson>().HasNoKey();
        modelBuilder.Entity<Perfil>().ToTable("perfil").HasKey(x => x.id);
        modelBuilder.Entity<Usuario>().ToTable("usuario").HasKey(x => x.username);
        modelBuilder.Entity<Proveedor>().ToTable("proveedor").HasKey(x => x.ruc);
        modelBuilder.Entity<ProveedorUsuario>().ToTable("proveedor_usuario").HasKey(x => new { x.ruc, x.username });
        modelBuilder.Entity<ProveedorUsuarioLog>().ToTable("proveedor_usuario_log").HasKey(x => new { x.id });
        modelBuilder.Entity<SolicitudAcceso>().ToTable("solicitud_acceso").HasKey(x => x.id);
        #endregion

        #region TablasMaestras
        modelBuilder.Entity<MotivoRechazo>().ToTable("motivo_rechazo").HasKey(x => x.motivo_rechazo);
        modelBuilder.Entity<Estado>().ToTable("estado").HasKey(x => x.estado);
        modelBuilder.Entity<TipoArchivo>().ToTable("tipo_archivo").HasKey(x => x.tipo_archivo);
        modelBuilder.Entity<TipoComprobante>().ToTable("tipo_comprobante").HasKey(x => x.tipo_comprobante);
        modelBuilder.Entity<TipoRequisito>().ToTable("tipo_requisito").HasKey(x => x.tipo_requisito);
        modelBuilder.Entity<TipoComprobanteRequisito>().ToTable("tipo_comprobante_requisito").HasKey(x => new { x.tipo_comprobante, x.tipo_requisito });
        modelBuilder.Entity<Parametro>().ToTable("parametro").HasKey(x => x.parametro);
        #endregion

        #region Tutoriales
        modelBuilder.Entity<TutorialVideo>().ToTable("tutorial_video").HasKey(x => x.id);
        #endregion

        #region Orden de Compra
        // modelBuilder.Entity<OrdenCompra>().ToTable("orden_compra").HasKey(x => x.orden_compra);
        modelBuilder.Entity<OrdenCompraEmbarqueEstado>().ToTable("orden_compra_embarque_estado").HasKey(x => new { x.orden_compra, x.embarque });
        modelBuilder.Entity<OrdenCompraBitacora>().ToTable("orden_compra_bitacora").HasKey(x => new { x.orden_compra, x.embarque, x.linea_bitacora });
        modelBuilder.Entity<OrdenCompraBitacoraRequisito>().ToTable("orden_compra_bitacora_requisito").HasKey(x => new { x.orden_compra, x.embarque, x.linea_bitacora, x.linea_requisito });
        modelBuilder.Entity<OrdenCompraRequisito>().ToTable("orden_compra_requisito").HasKey(x => new { x.orden_compra, x.embarque, x.linea_requisito });
        modelBuilder.Entity<OrdenCompraSustento>().ToTable("orden_compra_sustento").HasKey(x => new { x.orden_compra, x.embarque, x.linea_sustento });
        #endregion

        #region Store Procedure Query Select
        modelBuilder.Entity<OrdenCompraResponse>().HasNoKey();
        modelBuilder.Entity<OCBitacoraResponse>().HasNoKey();
        modelBuilder.Entity<OCRequisitosResponse>().HasNoKey();
        modelBuilder.Entity<OCRequisitoForMailResponse>().HasNoKey();
        modelBuilder.Entity<OCTiposRequisitosResponse>().HasNoKey();
        modelBuilder.Entity<ExisteResponse>().HasNoKey();
        modelBuilder.Entity<PedidosResponse>().HasNoKey();
        #endregion
    }
}