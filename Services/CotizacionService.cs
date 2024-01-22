using Microsoft.EntityFrameworkCore;
using ProveedorApi.Models.ContentBody;
using ProveedorApi.Data;
using ProveedorApi.Helpers;
using ProveedorApi.Models;
using ProveedorApi.Models.StoreProcedure;

namespace ProveedorApi.Services;

public class CotizacionService : _BaseService
{
    public CotizacionService(ExactusExtContext context) : base(context) { }

    #region GET
    public async Task<object> GetAllASync(byte? ge, DateTime fi, DateTime ff)
    {
        try
        {
            var result = await new JsonResultHelper(_contexte)
            .ToJsonResultAsync<object>($"exec cotizacion_get_all {ge}, {fi.Date}, {ff.Date}");
            return result ?? new object[] { };
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }

    public async Task<object> GetGrupoEstadoAllASync()
    {
        try
        {
            var result = await new JsonResultHelper(_contexte)
            .ToJsonResultAsync<object>($"exec cotizacion_get_grupo_estado");
            return result ?? new object[] { };
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }
    public async Task<object> GetBitacoraAsync(int p_cotcodigo)
    {
        try
        {
            var result = await new JsonResultHelper(_contexte)
            .ToJsonResultAsync<object>($"exec cotizacion_get_bitacora {p_cotcodigo}");
            return result ?? new object[] { };
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }
    public async Task<Cotizacion?> GetOneSync(int p_cotcodigo)
    {
        try
        {
            var result = await _contexte.Cotizacion.Where(x => x.cot_codigo == p_cotcodigo).FirstOrDefaultAsync();
            return result;
        }
        catch (System.Exception)
        {
            return null;
        }
    }
    public async Task<object> GetCategoriaAllASync()
    {
        try
        {
            var result = await new JsonResultHelper(_contexte)
            .ToJsonResultAsync<object>($"exec cotizacion_get_categoria");
            return result ?? new object[] { };
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }
    public async Task<object> GetCondicionPagoAllASync()
    {
        try
        {
            var result = await new JsonResultHelper(_contexte)
            .ToJsonResultAsync<object>($"exec cotizacion_get_condicion_pago");
            return result ?? new object[] { };
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }
    public async Task<object> GetMonedasAllASync()
    {
        try
        {
            var result = await new JsonResultHelper(_contexte)
            .ToJsonResultAsync<object>($"exec cotizacion_get_monedas");
            return result ?? new object[] { };
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }
    public async Task<object> GetSolicitudesSync(int cotcodigo)
    {
        try
        {
            var result = await new JsonResultHelper(_contexte)
            .ToJsonResultAsync<object>($"exec cotizacion_get_solicitudes {cotcodigo}");
            return result ?? new object[] { };
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }
    public async Task<CotizacionSolicitud?> GetOneSync(int p_cotcodigo, int p_coscodigo)
    {
        try
        {
            return await _contexte.CotizacionSolicitud.Where(x => x.cot_codigo == p_cotcodigo && x.cos_codigo == p_coscodigo).FirstOrDefaultAsync();
        }
        catch (System.Exception)
        {
            return null;
        }
    }

    public async Task<object> GetRequerimientoSync()
    {
        try
        {
            var result = await new JsonResultHelper(_contexte)
            .ToJsonResultAsync<object>($"exec cotizacion_get_requerimiento");
            return result ?? new object[] { };
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }

    public async Task<object> GetProveedoresASync(int cot)
    {
        try
        {
            var result = await new JsonResultHelper(_contexte)
            .ToJsonResultAsync<object>($"exec cotizacion_get_proveedor {cot}");
            return result ?? new object[] { };
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }
    public async Task<List<CotizacionSolicitudProveedor>?> GetListSync(int p_cotcodigo, int p_coscodigo)
    {
        try
        {
            return await _contexte.CotizacionSolicitudProveedor.Where(x => x.cot_codigo == p_cotcodigo && x.cos_codigo == p_coscodigo).ToListAsync();
        }
        catch (System.Exception)
        {
            return null;
        }
    }
    public async Task<object> GetSolicitudesProveedorSync(string rucProveedor, DateTime fi, DateTime ff)
    {
        try
        {
            var result = await new JsonResultHelper(_contexte)
            .ToJsonResultAsync<object>($"exec cotizacion_get_solicitudes_x_proveedor {rucProveedor}, {fi.Date}, {ff.Date}");
            return result ?? new object[] { };
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }
    public async Task<object> GetSolicitudesProveedorDetalleSync(string rucProveedor, int cot, int cos, int csp, string moneda)
    {
        try
        {
            var result = await new JsonResultHelper(_contexte)
            .ToJsonResultAsync<object>($"exec cotizacion_get_solicitudes_x_proveedor_detalle {rucProveedor}, {cot}, {cos}, {csp}, {moneda}");
            return result ?? new object[] { };
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }
    public async Task<object> GetSolicitudesProveedorAttachmentsSync(string rucProveedor, int cot, int cos, int csp)
    {
        try
        {
            var result = await new JsonResultHelper(_contexte)
            .ToJsonResultAsync<object>($"exec cotizacion_get_solicitudes_x_proveedor_attachments {rucProveedor}, {cot}, {cos}, {csp}");
            return result ?? new object[] { };
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }
    public async Task<object> GetCuadroComparativoSync(int cot, string? moneda)
    {
        try
        {
            var result = await new JsonResultHelper(_contexte)
            .ToJsonResultAsync<object>($"exec cotizacion_get_cuadro_comparativo {cot}, {moneda}");
            return result ?? new object[] { };
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }
    public async Task<object> GetUltimaCompraArticuloSync(string articulo)
    {
        try
        {
            var result = await new JsonResultHelper(_contexte)
            .ToJsonResultAsync<object>($"exec cotizacion_ultima_compra_articulo {articulo}");
            return result ?? new object[] { };
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }

    public async Task<object> GetPendientesxGeneraOC()
    {
        try
        {
            var result = await new JsonResultHelper(_contexte)
            .ToJsonResultAsync<object>($"exec cotizacion_pendientes_por_generar_oc");
            return result ?? new object[] { };
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }

    #endregion

    #region POST NUEVA COTIZACION
    public async Task<int> SaveCotizacionSync(CotizacionBody body, IFormFileCollection? formFiles, string username)
    {
        using (var transaction = _contexte.Database.BeginTransaction())
        {
            try
            {
                DateTime now = DateTime.Now;

                int idLast = CodCotLast() + 1;


                if (formFiles != null && formFiles.Any())
                {
                    body.archivos = new List<string>();

                    await Parallel.ForEachAsync(formFiles, new ParallelOptions() { MaxDegreeOfParallelism = 20 }, async (x, CancellationToken) =>
                    {
                        string filenameFull = Path.Combine(AppConfig.Configuracion.CarpetaArchivos, "cotizacion", idLast.ToString(), x.FileName);
                        bool status = await UtilityHelper.UploadFormFileAsync(Path.GetDirectoryName(filenameFull) ?? "", filenameFull, x);
                        if (!status)
                        {
                            throw new Exception("Error al subir un archivo");
                        }
                        body.archivos.Add(x.FileName);
                    });
                }

                var cotizacion = new Cotizacion()
                {
                    cot_codigo = idLast,
                    cot_descripcion = body.descripcion,
                    cot_fecha_registro = now,
                    ces_codigo = 1,
                    observacion = body.observacion,
                    puja = body.puja,
                    archivos = body.archivos == null ? "" : string.Join(";", body.archivos),
                    activo = true,
                    cot_fecha_requerida = body.fecha_requerida,
                    aud_usuario_reg = username,
                    aud_fecha_reg = now
                };

                _contexte.Cotizacion.Add(cotizacion);
                await _contexte.SaveChangesAsync();

                List<CotizacionDetalle> detalle = new List<CotizacionDetalle>();


                body.detalle.ForEach(x =>
                {
                    detalle.Add(new CotizacionDetalle()
                    {
                        cot_codigo = cotizacion.cot_codigo,
                        cod_codigo = x.cod_codigo,
                        articulo = x.articulo,
                        cod_cantidad = x.cantidad,
                        unm_codigo = x.unidad,
                        activo = true,
                        aud_usuario_reg = username,
                        aud_fecha_reg = now
                    });
                });

                _contexte.CotizacionDetalle.AddRange(detalle);

                #region COTIZACIÓN BITACORA

                _contexte.CotizacionBitacora.Add(new CotizacionBitacora()
                {
                    cot_codigo = cotizacion.cot_codigo,
                    linea_bitacora = LineaBitacoraLast(cotizacion.cot_codigo) + 1,
                    ces_codigo = 1,
                    activo = true,
                    comentario = "Solicitud de cotización registrada",
                    aud_fecha_reg = now,
                    aud_usuario_reg = username
                });

                #endregion

                await _contexte.SaveChangesAsync();




                List<CotizacionDetalleOriginal> detalle_original = new List<CotizacionDetalleOriginal>();

                body.detalle.ForEach(x =>
                {
                    x.original.ForEach(y =>
                    {
                        detalle_original.Add(new CotizacionDetalleOriginal()
                        {
                            cot_codigo = cotizacion.cot_codigo,
                            cod_codigo = x.cod_codigo,
                            codd_codigo = y.codd_codigo,
                            articulo = x.articulo,
                            cod_cantidad = y.cantidad,
                            unm_codigo = y.unidad,
                            activo = true,
                            aud_usuario_reg = username,
                            cod_observacion = y.observacion,
                            nro_solicitud = y.solicitud,
                            linea_solicitud = y.solicitud_linea,
                            aud_fecha_reg = now
                        });
                    });
                });

                _contexte.CotizacionDetalleOriginal.AddRange(detalle_original);
                await _contexte.SaveChangesAsync();

                await transaction.CommitAsync();

                return 1;

            }
            catch (System.Exception)
            {
                await transaction.RollbackAsync();
                return 0;
            }
        }

    }

    private int CodCotLast()
    {
        return _contexte.Cotizacion.OrderBy(x => x.cot_codigo).Select(x => x.cot_codigo).LastOrDefault();
    }

    #endregion

    #region POST NUEVA SOLICITUD
    public async Task<int> SaveCotizacionSolicitudSync(CotizacionSolicitudBody body, int p_cotcodigo, string username)
    {
        using (var transaction = _contexte.Database.BeginTransaction())
        {

            try
            {
                DateTime now = DateTime.Now;

                var solicitud = new CotizacionSolicitud()
                {
                    cot_codigo = p_cotcodigo,
                    cos_codigo = (byte)(CodCosLast(p_cotcodigo) + 1),
                    cos_fecha_vigencia_ini = body.fecha_vigencia_ini,
                    cos_fecha_vigencia_fin = body.fecha_vigencia_fin,
                    ces_codigo = 2,
                    activo = true,
                    aud_usuario_reg = username,
                    aud_fecha_reg = now
                };

                _contexte.CotizacionSolicitud.Add(solicitud);
                await _contexte.SaveChangesAsync();

                List<CotizacionSolicitudProveedor> proveedores = new List<CotizacionSolicitudProveedor>();

                body.proveedores.ForEach(x =>
                {
                    var solProveedor = new CotizacionSolicitudProveedor()
                    {
                        cot_codigo = solicitud.cot_codigo,
                        cos_codigo = solicitud.cos_codigo,
                        csp_codigo = x.csp_codigo,
                        proveedor = x.proveedor,
                        csp_atendido = false,
                        csp_condicion_pago = x.condicion_pago,
                        activo = true,
                        aud_usuario_reg = username,
                        aud_fecha_reg = now
                    };

                    _contexte.CotizacionSolicitudProveedor.Add(solProveedor);

                    List<CotizacionSolicitudProveedorDetalle> detalle = new List<CotizacionSolicitudProveedorDetalle>();

                    var cotdetalle = _contexte.CotizacionDetalle.Where(c => c.cot_codigo == solicitud.cot_codigo).ToList();

                    cotdetalle.ForEach(y =>
                    {
                        detalle.Add(new CotizacionSolicitudProveedorDetalle()
                        {
                            cot_codigo = y.cot_codigo,
                            cos_codigo = solicitud.cos_codigo,
                            csp_codigo = solProveedor.csp_codigo,
                            csd_item = y.cod_codigo,
                            articulo = y.articulo,
                            csd_cantidad = y.cod_cantidad,
                            unm_codigo = y.unm_codigo,
                            activo = true,
                            aud_usuario_reg = username,
                            aud_fecha_reg = now
                        });
                    });

                    _contexte.CotizacionSolicitudProveedorDetalle.AddRange(detalle);

                });

                await _contexte.SaveChangesAsync();
                await transaction.CommitAsync();

                return 1;
            }
            catch (System.Exception)
            {
                await transaction.RollbackAsync();
                return 0;
            }
        }
    }

    public async Task<int> SaveSolicitudChangeVigenciaSync(CotizacionSolicitudChangeVigencia body, string username)
    {
        try
        {
            var solicitud = _contexte.CotizacionSolicitud.Where(x => x.cot_codigo == body.cot && x.cos_codigo == body.cos && x.activo == true).FirstOrDefault();
            if (solicitud == null) return 0;

            solicitud.cos_fecha_vigencia_ini = body.vigencia_ini.Date;
            solicitud.cos_fecha_vigencia_fin = body.vigencia_fin.Date;
            solicitud.aud_fecha_act = DateTime.Now;
            solicitud.aud_usuario_act = username;

            var cotizacion = _contexte.Cotizacion.Where(x => x.cot_codigo == solicitud.cot_codigo).FirstOrDefault();

            if (cotizacion != null)
            {
                cotizacion.ces_codigo = 1;
                _contexte.Cotizacion.Update(cotizacion);
            }

            _contexte.CotizacionSolicitud.Update(solicitud);
            return await _contexte.SaveChangesAsync();
        }
        catch (System.Exception)
        {
            return 0;
        }
    }

    public async Task<int> UpdateStatusCotizacionJob(int? cot)
    {
        return await _contexte.Database.ExecuteSqlInterpolatedAsync($"cotizacion_update_status_job {cot}");
    }
    private byte CodCosLast(int cotcodigo)
    {
        return _contexte.CotizacionSolicitud.Where(x => x.cot_codigo == cotcodigo).OrderBy(x => x.cos_codigo).Select(x => x.cos_codigo).LastOrDefault();
    }
    public async Task<string> GetHtmlForSendSolicitudSync(int cot) => await new JsonResultHelper(_contexte).ToStringResultAsync<string>($"exec cotizacion_get_html_for_send_new_solicitud {cot}");
    public async Task<string> GetHtmlForChangeVigenciaSolicitudSync(int cot) => await new JsonResultHelper(_contexte).ToStringResultAsync<string>($"exec cotizacion_get_html_for_change_vigencia_solicitud {cot}");

    #endregion


    #region POST ADJUNTAR / DELETE ARCHIVOS ANTES DE RESPONDER
    public async Task<Tuple<int, int, string>> UploadFileAttachmentAsync(int cot, int cos, int csp, string p_nombre_original, string usersession)
    {
        try
        {
            CotizacionSolicitudProveedorAttachment attachment = new CotizacionSolicitudProveedorAttachment();
            attachment.cot_codigo = cot;
            attachment.cos_codigo = (byte)cos;
            attachment.csp_codigo = csp;
            attachment.csa_item = AttachmentItemTheLast(cot, cos, csp) + 1;
            attachment.csa_filename_original = p_nombre_original;

            string filename = $"{attachment.csa_item.ToString()}.{p_nombre_original.Substring(p_nombre_original.LastIndexOf(".") + 1)}";
            string filenameFull = Path.Combine(AppConfig.Configuracion.CarpetaArchivos, "cotizacion", cot.ToString(), cos.ToString(), csp.ToString(), filename);

            attachment.csa_filename = filename;
            attachment.activo = true;
            attachment.aud_usuario_reg = usersession;
            attachment.aud_fecha_reg = DateTime.Now;

            _contexte.CotizacionSolicitudProveedorAttachment.Add(attachment);

            int saveInt = await _contexte.SaveChangesAsync();
            if (saveInt > 0)
            {
                return Tuple.Create(saveInt, attachment.csa_item, filenameFull);
            }
            return Tuple.Create(0, 0, "");
        }
        catch (System.Exception)
        {
            return Tuple.Create(0, 0, "");
        }
    }

    public async Task<int> UploadFileAttachmentZipBytesAsync(int cot, int cos, int csp, int item, string filename, string p_nombre_original)
    {
        try
        {
            string fullname = Path.Combine(AppConfig.Configuracion.CarpetaArchivos, "cotizacion", cot.ToString(), cos.ToString(), csp.ToString(), filename);

            if (!File.Exists(fullname)) return 0;

            byte[] filebyte = await System.IO.File.ReadAllBytesAsync(fullname);

            using var compressedStream = new MemoryStream();
            using (var gzipStream = new System.IO.Compression.GZipStream(compressedStream, System.IO.Compression.CompressionLevel.Optimal, false))
            {
                await gzipStream.WriteAsync(filebyte);
            }

            byte[] filebyteF = compressedStream.ToArray();

            compressedStream.Flush();
            compressedStream.Dispose();

            _contexte.CotizacionGenIdOcAttachment.Add(new CotizacionGenIdOcAttachment()
            {
                cot_codigo = cot,
                cos_codigo = (byte)cos,
                csp_codigo = csp,
                csa_item = item,
                csa_filename_original = p_nombre_original,
                csa_file_binary = filebyteF
            });

            return await _contexte.SaveChangesAsync();
        }
        catch (System.Exception)
        {
            return 0;
        }
    }

    public async Task<int> DeleteTotalAttachmentAsync(int cot, int cos, int csp, int item, string usersession)
    {
        try
        {
            var attachment = await _contexte.CotizacionSolicitudProveedorAttachment.Where(x => x.cot_codigo == cot && x.cos_codigo == cos && x.csp_codigo == csp && x.csa_item == item).FirstOrDefaultAsync();
            var attachment2 = await _contexte.CotizacionGenIdOcAttachment.Where(x => x.cot_codigo == cot && x.cos_codigo == cos && x.csp_codigo == csp && x.csa_item == item).FirstOrDefaultAsync();

            if (attachment == null) return 0;
            _contexte.CotizacionSolicitudProveedorAttachment.Remove(attachment);

            if (attachment2 != null)
            {
                _contexte.CotizacionGenIdOcAttachment.Remove(attachment2);
            }

            return await _contexte.SaveChangesAsync();
        }
        catch (System.Exception)
        {
            return 0;
        }
    }
    private int AttachmentItemTheLast(int cot, int cos, int csp)
    {
        return _contexte.CotizacionSolicitudProveedorAttachment.Where(x => x.cot_codigo == cot && x.cos_codigo == cos && x.csp_codigo == csp).OrderBy(x => x.csa_item).Select(x => x.csa_item).LastOrDefault();
    }

    #endregion

    #region POST RESPONDER SOLICITUD
    public async Task<int> ResponderCotizacionSolicitudSync(CotizacionSolicitudResponderBody body, string username)
    {
        try
        {
            DateTime now = DateTime.Now;

            var solicitud = await _contexte.CotizacionSolicitud.Where(x => x.cot_codigo == body.cotcodigo && x.cos_codigo == body.coscodigo).FirstOrDefaultAsync();
            if (solicitud == null) return 0;

            if (solicitud.cos_fecha_vigencia_ini.Date > now.Date || solicitud.cos_fecha_vigencia_fin.Date < now.Date) return 0;

            var solProveedor = await _contexte.CotizacionSolicitudProveedor.Where(x => x.cot_codigo == body.cotcodigo && x.cos_codigo == body.coscodigo && x.csp_codigo == body.cspcodigo).FirstOrDefaultAsync();

            if (solProveedor == null) return 0;

            solProveedor.csp_atendido = true;
            solProveedor.aud_usuario_act = username;
            solProveedor.aud_fecha_act = now;

            _contexte.CotizacionSolicitudProveedor.Update(solProveedor);

            var detalle = await _contexte.CotizacionSolicitudProveedorDetalle.Where(x => x.cot_codigo == body.cotcodigo && x.cos_codigo == body.coscodigo && x.csp_codigo == body.cspcodigo).ToListAsync();

            if (detalle == null) return 0;


            foreach (CotizacionSolicitudDetalleResponderBody d in body.detalle)
            {
                var _det = await _contexte.CotizacionSolicitudProveedorDetalle
                                                    .Where(x => x.cot_codigo == body.cotcodigo
                                                            && x.cos_codigo == body.coscodigo
                                                            && x.csp_codigo == body.cspcodigo
                                                            && x.csd_item == d.item)
                                                    .FirstOrDefaultAsync();

                if (_det == null) continue;

                _det.mon_codigo = d.moneda;
                _det.csd_precio = d.precio;
                _det.csd_descuento = d.descuento ?? 0;
                _det.csd_dias_entrega = d.dias;
                _det.csd_observacion = d.observacion;
                _det.aud_fecha_act = now;
                _det.aud_usuario_act = username;

                _contexte.CotizacionSolicitudProveedorDetalle.Update(_det);

            }

            await _contexte.SaveChangesAsync();

            return 1;
        }
        catch (System.Exception)
        {
            return 0;
        }
    }

    #endregion

    #region POST CUADRO COMPARATIVO
    public async Task<int> SavePrecioElegidoAsync(CotizacionPrecioElegido body, string username)
    {
        try
        {
            var provdetalleArticulo = await _contexte.CotizacionSolicitudProveedorDetalle
                                            .Where(x => x.cot_codigo == body.cotcodigo && x.articulo == body.articulo).ToListAsync();

            if (provdetalleArticulo != null)
            {
                provdetalleArticulo.ForEach(x =>
                {
                    x.csd_elegido = false;
                    if (x.cos_codigo == body.coscodigo && x.csp_codigo == body.cspcodigo && x.csd_item == body.item)
                    {
                        x.csd_elegido = body.elegido;
                        x.aud_usuario_act = username;
                        x.aud_fecha_act = DateTime.Now;
                    }
                });


                _contexte.CotizacionSolicitudProveedorDetalle.UpdateRange(provdetalleArticulo);
                return await _contexte.SaveChangesAsync();
            }
            return 0;
        }
        catch (System.Exception)
        {
            return 0;
        }
    }

    #endregion

    #region FINALIZAR COTIZACION
    public async Task<int> FinalizarCotizacion(int codigo, string username)
    {
        try
        {
            var cotizacion = await _contexte.Cotizacion.Where(x => x.cot_codigo == codigo).FirstOrDefaultAsync();
            if (cotizacion == null) return 0;

            if (cotizacion.ces_codigo == 4) return 0;

            cotizacion.aud_fecha_act = DateTime.Now;
            cotizacion.aud_usuario_act = username;
            cotizacion.ces_codigo = 4;

            _contexte.Cotizacion.Update(cotizacion);


            #region COTIZACIÓN BITACORA

            _contexte.CotizacionBitacora.Add(new CotizacionBitacora()
            {
                cot_codigo = cotizacion.cot_codigo,
                linea_bitacora = LineaBitacoraLast(cotizacion.cot_codigo) + 1,
                ces_codigo = 4,
                activo = true,
                comentario = "Solicitud de cotización finalizada",
                aud_fecha_reg = DateTime.Now,
                aud_usuario_reg = username
            });

            #endregion


            return await _contexte.SaveChangesAsync();
        }
        catch (System.Exception)
        {
            return 0;
        }
    }

    public async Task<int> UpdateStatusDetalleOriginalCasoAccionFinalizar(int cot)
    {
        return await _contexte.Database.ExecuteSqlInterpolatedAsync($"cotizacion_update_status_original_case_finalizado {cot}");
    }
    #endregion


    #region GENERAR OC
    public async Task<int> GenerarOC(List<GenerarOCBody> body, string username)
    {
        using (var transaction = _contexte.Database.BeginTransaction())
        {
            try
            {
                int genId = _contexte.CotizacionGenIdOc.OrderBy(x => x.gen_id).Select(x => x.gen_id).LastOrDefault();
                genId++;


                _contexte.CotizacionGenIdOc.Add(new CotizacionGenIdOc()
                {
                    gen_id = genId,
                    activo = true,
                    aud_fecha_reg = DateTime.Now,
                    aud_usuario_reg = username,
                });

                List<CotizacionSolicitudProveedorDetalle> list = new List<CotizacionSolicitudProveedorDetalle>();
                List<CotizacionGenIdOcAttachment> atts = new List<CotizacionGenIdOcAttachment>();

                foreach (GenerarOCBody item in body)
                {
                    var cspd = _contexte.CotizacionSolicitudProveedorDetalle.Where(x => x.cot_codigo == item.cot && x.cos_codigo == item.cos && x.csp_codigo == item.csp && x.csd_item == item.item).FirstOrDefault();
                    if (cspd == null) throw new Exception();
                    cspd.gen_id = genId;
                    list.Add(cspd);
                }
                _contexte.CotizacionSolicitudProveedorDetalle.UpdateRange(list);

                int status = await _contexte.SaveChangesAsync();

                if (status == 0)
                {
                    await transaction.RollbackAsync();
                    return 0;
                }

                var listAttForGenId = await GetCotsOfGendId(genId);

                foreach (var item in listAttForGenId)
                {
                    await UploadFileAttachmentZipBytesAsync(item.cot_codigo, item.cos_codigo, item.csp_codigo, item.csa_item, item.csa_filename, item.csa_filename_original);
                }

                int statusGen = await CotizacionGeneraOC(genId);

                if (status == 0)
                {
                    await transaction.RollbackAsync();
                    return 0;
                }

                await transaction.CommitAsync();
                return 1;
            }
            catch (System.Exception)
            {
                await transaction.RollbackAsync();
                return 0;
            }
        }
    }

    #endregion

    public async Task<List<SPCotizacionAttachmentsForGenId>> GetCotsOfGendId(int gendId)
    {
        return await _contexte.SPCotizacionAttachmentsForGenId.FromSqlInterpolated($"cotizacion_get_attachment_for_genId {gendId}").ToListAsync();
    }

    public async Task<int> CotizacionGeneraOC(int genId)
    {
        return await _contexte.Database.ExecuteSqlInterpolatedAsync($"cotizacion_generar_orden_compra {genId}");
    }

    #region DELETE COTIZACION
    public async Task<int> DeleteCotizacion(int codigo, string username)
    {
        try
        {
            DateTime now = DateTime.Now;
            var cotizacion = await _contexte.Cotizacion.Where(x => x.cot_codigo == codigo).FirstOrDefaultAsync();
            if (cotizacion == null) return 0;

            if (!cotizacion.activo) return 0;

            cotizacion.aud_fecha_eli = now;
            cotizacion.aud_usuario_eli = username;
            cotizacion.activo = false;

            _contexte.Cotizacion.Update(cotizacion);


            #region COTIZACIÓN BITACORA

            _contexte.CotizacionBitacora.Add(new CotizacionBitacora()
            {
                cot_codigo = cotizacion.cot_codigo,
                linea_bitacora = LineaBitacoraLast(cotizacion.cot_codigo) + 1,
                ces_codigo = 5,
                activo = true,
                comentario = "Solicitud de cotización anulada",
                aud_fecha_reg = now,
                aud_usuario_reg = username
            });

            #endregion

            return await _contexte.SaveChangesAsync();
        }
        catch (System.Exception)
        {
            return 0;
        }
    }
    #endregion


    private int LineaBitacoraLast(int cot)
    {
        return _contexte.CotizacionBitacora.Where(x => x.cot_codigo == cot).OrderBy(x => x.linea_bitacora).Select(x => x.linea_bitacora).LastOrDefault();
    }
}