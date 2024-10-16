using Microsoft.EntityFrameworkCore;
using ProveedorApi.Models.ContentBody;
using ProveedorApi.Data;
using ProveedorApi.Models;
using ProveedorApi.Models.ContentResponse;

namespace ProveedorApi.Services;

public class OrdenCompraRequisitoService : _BaseService
{
    public OrdenCompraRequisitoService(ProveedorContext context) : base(context) { }

    public async Task<object> GetAllSPAsync(string p_ruc, string p_orden_compra, string p_embarque)
    {
        try
        {
            var result = await _context.OCRequisitosResponse.FromSqlInterpolated($"exec oc_get_orden_compra_requisitos {p_ruc}, {p_orden_compra}, {p_embarque}").ToListAsync();
            if (result == null) return new object[] { };
            return result;
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }
    public async Task<object> GetTipoRequisitosSPAsync(string p_ruc, string p_oc, string p_embarque)
    {
        try
        {
            var result = await _context.OCTiposRequisitosResponse.FromSqlInterpolated($"exec oc_get_tipos_requisitos_x_tipo_comprobante {p_ruc}, {p_oc}, {p_embarque}").ToListAsync();
            if (result == null) return new object[] { };
            return result;
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }
    public async Task<List<OCRequisitoForMailResponse>> GetForMailAsync(string p_ruc, string p_orden_compra, string p_embarque)
    {
        try
        {
            var result = await _context.OCRequisitoForMailResponse.FromSqlInterpolated($"exec oc_get_orden_compra_requisito_for_email {p_ruc}, {p_orden_compra}, {p_embarque}").ToListAsync();
            if (result == null) return new List<OCRequisitoForMailResponse>();
            return result;
        }
        catch (System.Exception)
        {
            return new List<OCRequisitoForMailResponse>();
        }
    }

    public bool GetStatusPendientes(string p_ruc, string p_orden_compra, string p_embarque)
    {
        try
        {
            var result = _context.ExisteResponse.FromSqlInterpolated($"exec oc_get_orden_compra_requisitos_estatus_pendientes {p_ruc}, {p_orden_compra}, {p_embarque}").ToList();
            if (result == null) return false;
            return result[0].existe > 0;
        }
        catch (System.Exception)
        {
            return false;
        }
    }
    public async Task<int> SaveAsync(string p_ruc, string p_orden_compra, string p_embarque, int p_linea, int p_tipo_requisito, int p_item, string valor, string aceptado, string usersession)
    {
        try
        {
            bool existeOCEE = await new OrdenCompraEmbarqueEstadoService(_context).SaveIfNotExist(p_ruc, p_orden_compra, p_embarque, usersession);

            if (!existeOCEE) return 0;

            if (!OCRequisitoItemExists(p_orden_compra, p_embarque, p_tipo_requisito, p_item))
            {
                var ocrequisito = new OrdenCompraRequisito
                {
                    orden_compra = p_orden_compra,
                    embarque = p_embarque,
                    tipo_requisito = p_tipo_requisito,
                    linea_requisito = OCRequisitoItemTheLastLinea(p_orden_compra, p_embarque) + 1,
                    item = OCRequisitoItemTheLastItem(p_orden_compra, p_embarque, p_tipo_requisito) + 1,
                    aceptado = "S",
                    valor = valor,
                    active = "S",
                    created_by = usersession,
                    created_at = DateTime.Now
                };
                _context.OrdenCompraRequisito.Add(ocrequisito);

            }
            else
            {
                var ocr = await _context.OrdenCompraRequisito.Where(x => x.orden_compra == p_orden_compra && x.embarque == p_embarque && x.tipo_requisito == p_tipo_requisito && x.linea_requisito == p_linea && x.active == "S").FirstOrDefaultAsync();
                if (ocr != null)
                {
                    ocr.aceptado = aceptado;
                    ocr.valor = valor;
                    ocr.updated_by = usersession;
                    ocr.updated_at = DateTime.Now;
                    _context.OrdenCompraRequisito.Update(ocr);
                }
            }
            return await _context.SaveChangesAsync();
        }
        catch (System.Exception)
        {
            return 0;
        }
    }
    public async Task<int> UpdateRechazoAsync(NotificarRechazoBody p_notifBody, string p_usersession)
    {
        try
        {
            var existe = new OrdenCompraService(_context).GetAnyItem(p_notifBody.ruc, p_notifBody.orden_compra, p_notifBody.embarque);

            if (!existe) return 0;

            var ocembarque_e = await _context.OrdenCompraEmbarqueEstado.Where(x => x.orden_compra == p_notifBody.orden_compra && x.embarque == p_notifBody.embarque).FirstOrDefaultAsync();

            if (ocembarque_e == null)
            {
                _context.OrdenCompraEmbarqueEstado.Add(new OrdenCompraEmbarqueEstado()
                {
                    orden_compra = p_notifBody.orden_compra,
                    embarque = p_notifBody.embarque,
                    rechazado = p_notifBody.rechazado ? "S" : "S",
                    motivo_rechazo = p_notifBody.motivo_rechazo,
                    nota_credito = p_notifBody.solicita_nota_credito ? "S" : "N",
                    estado = "4",
                });
            }
            else
            {
                ocembarque_e.rechazado = p_notifBody.rechazado ? "S" : "S";
                ocembarque_e.motivo_rechazo = p_notifBody.motivo_rechazo;
                ocembarque_e.nota_credito = p_notifBody.solicita_nota_credito ? "S" : "N";
                ocembarque_e.estado = "4";
                _context.OrdenCompraEmbarqueEstado.Update(ocembarque_e);
            }


            int lineaBitacora = OCBitacoraItemTheLastLinea(p_notifBody.orden_compra, p_notifBody.embarque) + 1;
            var ocbitacora = new OrdenCompraBitacora
            {
                orden_compra = p_notifBody.orden_compra,
                embarque = p_notifBody.embarque,
                estado = "4",
                linea_bitacora = lineaBitacora,
                motivo_rechazo = p_notifBody.motivo_rechazo,
                comentario = p_notifBody.motivo_rechazo_descripcion,
                active = "S",
                created_by = p_usersession,
                created_at = DateTime.Now
            };

            _context.OrdenCompraBitacora.Add(ocbitacora);


            if (p_notifBody.requisitos != null)
            {
                if (p_notifBody.requisitos.Count > 0)
                {
                    var ocrequisitos = await _context.OrdenCompraRequisito.Where(x => x.orden_compra == p_notifBody.orden_compra && x.embarque == p_notifBody.embarque && x.active == "S").ToListAsync();
                    if (ocrequisitos != null)
                    {
                        ocrequisitos.ForEach(x =>
                        {
                            var reqBody = p_notifBody.requisitos.Where(n => n.tipo_requisito == x.tipo_requisito && n.linea_requisito == x.linea_requisito).FirstOrDefault();
                            if (reqBody != null)
                            {
                                x.aceptado = reqBody.aceptado;
                                x.updated_by = p_usersession;
                                x.updated_at = DateTime.Now;
                                _context.OrdenCompraRequisito.Update(x);

                                if (reqBody.aceptado == "N")
                                {
                                    var ocbitacorareq = new OrdenCompraBitacoraRequisito
                                    {
                                        orden_compra = x.orden_compra,
                                        embarque = x.embarque,
                                        linea_bitacora = lineaBitacora,
                                        linea_requisito = x.linea_requisito,
                                        tipo_requisito = x.tipo_requisito,
                                        active = "S",
                                        created_by = p_usersession,
                                        created_at = DateTime.Now
                                    };
                                    _context.OrdenCompraBitacoraRequisito.Add(ocbitacorareq);
                                }

                            }
                        });

                    }
                }
            }

            if (p_notifBody.sustentos != null)
            {
                if (p_notifBody.sustentos.Count > 0)
                {
                    var ocsustentos = await _context.OrdenCompraSustento
                                            .Where(x => x.orden_compra == p_notifBody.orden_compra && x.embarque == p_notifBody.embarque && x.active == "S")
                                            .ToListAsync();

                    if (ocsustentos != null)
                    {
                        ocsustentos.ForEach(x =>
                        {
                            var susBody = p_notifBody.sustentos.Where(n => n.linea_sustento == x.linea_sustento).FirstOrDefault();
                            if (susBody != null)
                            {
                                x.aceptado = susBody.aceptado;
                                x.updated_at = DateTime.Now;
                                x.updated_by = p_usersession;

                                _context.OrdenCompraSustento.Update(x);

                                if (susBody.aceptado == "N")
                                {
                                    // var ocbitacorareq = new OrdenCompraBitacoraRequisito
                                    // {
                                    //     orden_compra = x.orden_compra,
                                    //     embarque = x.embarque,
                                    //     linea_bitacora = lineaBitacora,
                                    //     linea_requisito = x.linea_requisito,
                                    //     tipo_requisito = x.tipo_requisito,
                                    //     active = "S",
                                    //     created_by = p_usersession,
                                    //     created_at = DateTime.Now
                                    // };
                                    // _context.OrdenCompraBitacoraRequisito.Add(ocbitacorareq);
                                }
                            }
                        });
                    }
                }
            }


            return await _context.SaveChangesAsync();
        }
        catch (System.Exception)
        {
            return 0;
        }
    }

    #region DELETE REQUISITOS
    public async Task<int> DeleteRequisito(EliminarRequisitoBody body)
    {
        try
        {
            var requisito = await _context.OrdenCompraRequisito
                                    .Where(x => x.orden_compra == body.p_orden_compra
                                    && x.embarque == body.p_embarque
                                    && x.tipo_requisito == body.tipo
                                    && x.item == body.item
                                    && x.valor == body.filename)
                                    .FirstOrDefaultAsync();
            if (requisito == null) return 0;

            _context.OrdenCompraRequisito.Remove(requisito);

            return await _context.SaveChangesAsync();
        }
        catch (System.Exception)
        {
            return 0;
        }
    }
    #endregion

    private bool OCRequisitoItemExists(string oc, string em, int tr, int item)
    {
        return _context.OrdenCompraRequisito.Any(x => x.orden_compra == oc && x.embarque == em && x.tipo_requisito == tr && x.item == item && x.active == "S");
    }

    private int OCRequisitoItemTheLastLinea(string oc, string em)
    {
        var linea = _context.OrdenCompraRequisito.Where(x => x.orden_compra == oc && x.embarque == em).OrderBy(x => x.linea_requisito).Select(x => x.linea_requisito).LastOrDefault();
        return linea ?? 0;
    }

    private int OCRequisitoItemTheLastItem(string oc, string em, int tr)
    {
        var items = _context.OrdenCompraRequisito
            .Where(x => x.orden_compra == oc && x.embarque == em && x.tipo_requisito == tr)
            .Select(x => x.item)
            .ToList();
        return items == null || items.Count == 0 ? 0 : items.Max();
    }

    private int OCBitacoraItemTheLastLinea(string oc, string em)
    {
        var linea = _context.OrdenCompraBitacora.Where(x => x.orden_compra == oc && x.embarque == em).OrderBy(x => x.linea_bitacora).Select(x => x.linea_bitacora).LastOrDefault();
        return linea ?? 0;
    }
}