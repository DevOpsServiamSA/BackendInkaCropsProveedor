using Microsoft.EntityFrameworkCore;
using ProveedorApi.Data;
using ProveedorApi.Models;

namespace ProveedorApi.Services;

public class OrdenCompraEmbarqueEstadoService : _BaseService
{
    public OrdenCompraEmbarqueEstadoService(ProveedorContext context) : base(context) { }

    public async Task<int> SaveEstadoAsync(string p_ruc, string p_orden_compra, string p_embarque, string p_estado, string usersession)
    {

        try
        {
            var existe = new OrdenCompraService(_context).GetAnyItem(p_ruc, p_orden_compra, p_embarque);

            if (!existe) return 0;

            var ocee = await _context.OrdenCompraEmbarqueEstado.Where(x => x.orden_compra == p_orden_compra && x.embarque == p_embarque).FirstOrDefaultAsync();
            if (ocee == null)
            {
                _context.OrdenCompraEmbarqueEstado.Add(new OrdenCompraEmbarqueEstado()
                {
                    orden_compra = p_orden_compra,
                    embarque = p_embarque,
                    estado = p_estado,
                    created_at = DateTime.Now,
                    created_by = usersession
                });
            }
            else
            {
                ocee.estado = p_estado;
                ocee.updated_at = DateTime.Now;
                ocee.updated_by = usersession;

                _context.OrdenCompraEmbarqueEstado.Update(ocee);
            }
            return await _context.SaveChangesAsync();
        }
        catch (System.Exception)
        {
            return 0;
        }
    }

    public bool GetAnyItem(string p_orden_compra, string p_embarque)
    {
        try
        {
            var result = _context.ExisteResponse.FromSqlInterpolated($"exec oc_get_orden_compra_embarque_estado_any {p_orden_compra}, {p_embarque}").ToList();
            if (result == null) return false;
            return result[0].existe > 0;
        }
        catch (System.Exception)
        {
            return false;
        }
    }

    public async Task<bool> SaveIfNotExist(string p_ruc, string p_orden_compra, string p_embarque, string usersession)
    {
        var existeOCEE = new OrdenCompraEmbarqueEstadoService(_context).GetAnyItem(p_orden_compra, p_embarque);
        if (!existeOCEE)
        {
            var existeOC = new OrdenCompraService(_context).GetAnyItem(p_ruc, p_orden_compra, p_embarque);

            if (existeOC)
            {
                int saveIntEstadoEmbarque = await new OrdenCompraEmbarqueEstadoService(_context).SaveEstadoAsync(p_ruc, p_orden_compra, p_embarque, "1", usersession);
                if (saveIntEstadoEmbarque == 0)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        return true;
    }
}