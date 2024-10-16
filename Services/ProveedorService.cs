using Microsoft.EntityFrameworkCore;
using ProveedorApi.Data;
using ProveedorApi.Models;

namespace ProveedorApi.Services;

public class ProveedorService : _BaseService
{
    public ProveedorService(ProveedorContext context) : base(context) { }

    public async Task<object> GetAllAsync(string? p_prov_ruc)
    {
        try
        {
            var query = from p in _context.Proveedor
                        where p.active == "S"
                        && p.ruc == ((p_prov_ruc ?? "").Trim().Length == 0 ? p.ruc : p_prov_ruc)
                        orderby p.razonsocial
                        select new
                        {
                            p.ruc,
                            p.razonsocial,
                            p.direccion,
                            p.email,
                            p.created_at
                        };

            var result = await query.ToListAsync();
            return result;
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }
    public async Task<Proveedor?> GetItemAsync(string ruc)
    {
        try
        {
            return await _context.Proveedor.Where(x => x.ruc == ruc && x.active == "S").FirstOrDefaultAsync();
        }
        catch (System.Exception)
        {
            return null;
        }
    }
}