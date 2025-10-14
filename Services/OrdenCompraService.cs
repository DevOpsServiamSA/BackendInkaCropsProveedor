using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ProveedorApi.Data;

namespace ProveedorApi.Services;

public class OrdenCompraService : _BaseService
{
    public OrdenCompraService(ProveedorContext context) : base(context) { }

    public async Task<object> GetAllSPAsync(DateTime p_fi, DateTime p_ff, 
                                            int p_estado,int p_filtrosadd,
                                            DateTime p_fid, DateTime p_ffd,
                                            string? p_prov_rs = null, 
                                            string? p_prov_ruc = null,
                                            string? tipoEmbarque = null
                                            )
    {
        try
        {
            var result = await _context.OrdenCompraResponse.FromSqlInterpolated($"exec oc_get_orden_compra {p_fi.Date}, {p_ff.Date}, {p_fid.Date}, {p_ffd.Date}, {p_estado}, {p_filtrosadd}, {p_prov_rs}, {p_prov_ruc}, {tipoEmbarque}").ToListAsync();
            if (result == null) return new object[] { };
            return result;
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }
    public bool GetAnyItem(string p_ruc, string p_orden_compra, string p_embarque)
    {
        try
        {
            var result = _context.ExisteResponse.FromSqlInterpolated($"exec oc_get_orden_compra_any {p_ruc}, {p_orden_compra}, {p_embarque}").ToList();
            if (result == null) return false;
            return result[0].existe > 0;
        }
        catch (System.Exception)
        {
            return false;
        }
    }
    
    public async Task<bool> GetOrdenCompraEmbarque(string p_orden_compra, string p_embarque)
    {
        try
        {
            var result = await _context.ExisteResponse.FromSqlInterpolated($"exec oc_get_orden_compra_embarque {p_orden_compra}, {p_embarque}").ToListAsync();
            if (result == null || result.Count == 0)
                return false;

            return result[0].existe > 0;
        }
        catch (System.Exception)
        {
            return false;
        }
    }
}