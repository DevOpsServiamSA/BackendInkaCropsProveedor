using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ProveedorApi.Data;

namespace ProveedorApi.Services;

public class PedidosService : _BaseService
{
    public PedidosService(ProveedorContext context) : base(context) { }

    public async Task<object> GetAllSPAsync()
    {
        try
        {
            var result = await _context.PedidosResponse.FromSqlInterpolated($"exec pe_get_pedidos_todos").ToListAsync();
            if (result == null) return new object[] { };
            return result;
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }
}