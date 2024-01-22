using Microsoft.EntityFrameworkCore;
using ProveedorApi.Data;
using ProveedorApi.Models;

namespace ProveedorApi.Services;

public class OrdenCompraBitacoraService : _BaseService
{
    public OrdenCompraBitacoraService(ProveedorContext context) : base(context) { }

    public async Task<object> GetAllSPAsync(string p_ruc, string p_orden_compra, string p_embarque)
    {
        try
        {
            var result = await _context.OCBitacoraResponse.FromSqlInterpolated($"exec oc_get_orden_compra_bitacora {p_ruc}, {p_orden_compra}, {p_embarque}").ToListAsync();
            if (result == null) return new object[] { };
            return result;
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }
    public async Task<int> SaveAsync(string p_orden_compra, string p_embarque, string p_estado, string p_comentario, string p_usersession)
    {
        int linea = OCBitacoraItemTheLastLinea(p_orden_compra, p_embarque) + 1;
        try
        {
            // if (!OCBitacoraItemExists(p_orden_compra, p_embarque))
            // {
            //     _context.OrdenCompraBitacora.Add(new OrdenCompraBitacora
            //     {
            //         orden_compra = p_orden_compra,
            //         embarque = p_embarque,
            //         estado = "1",
            //         linea_bitacora = linea,
            //         comentario = "",
            //         active = "S",
            //         created_by = p_usersession,
            //         created_at = DateTime.Now
            //     });
            //     if (p_estado == "1")
            //     {
            //         return await _context.SaveChangesAsync();
            //     }

            //     linea++;
            // }



            var ocbitacora = new OrdenCompraBitacora
            {
                orden_compra = p_orden_compra,
                embarque = p_embarque,
                estado = p_estado,
                linea_bitacora = linea,
                comentario = p_comentario,
                active = "S",
                created_by = p_usersession,
                created_at = DateTime.Now
            };

            var itemwithestado = OCBitacoraItemWithEstado(p_orden_compra, p_embarque, p_estado);

            if (itemwithestado == null)
            {
                _context.OrdenCompraBitacora.Add(ocbitacora);
            }
            else
            {
                if (itemwithestado.linea_bitacora == linea)
                {
                    itemwithestado.estado = p_estado;
                    itemwithestado.updated_at = DateTime.Now;
                    itemwithestado.updated_by = p_usersession;
                    _context.OrdenCompraBitacora.Update(itemwithestado);
                }
                else
                {
                    _context.OrdenCompraBitacora.Add(ocbitacora);
                }
            }
            return await _context.SaveChangesAsync();
        }
        catch (System.Exception ex)
        {
            string msj = ex.Message;
            return 0;
        }
    }
    private OrdenCompraBitacora? OCBitacoraItemWithEstado(string oc, string em, string es)
    {
        return _context.OrdenCompraBitacora.Where(x => x.orden_compra == oc && x.embarque == em && x.estado == es).OrderBy(x => x.linea_bitacora).LastOrDefault();
    }
    private bool OCBitacoraItemExists(string oc, string em)
    {
        return _context.OrdenCompraBitacora.Any(x => x.orden_compra == oc && x.embarque == em && x.active == "S");
    }

    private bool OCBitacoraItemExists(string oc, string em, string estado)
    {
        return _context.OrdenCompraBitacora.Any(x => x.orden_compra == oc && x.embarque == em && x.estado == estado && x.active == "S");
    }
    private int OCBitacoraItemTheLastLinea(string oc, string em)
    {
        var linea = _context.OrdenCompraBitacora.Where(x => x.orden_compra == oc && x.embarque == em).OrderBy(x => x.linea_bitacora).Select(x => x.linea_bitacora).LastOrDefault();
        return linea ?? 0;
    }

}