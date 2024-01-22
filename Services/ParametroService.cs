using Microsoft.EntityFrameworkCore;
using ProveedorApi.Data;

namespace ProveedorApi.Services;

public class ParametroService : _BaseService
{
    public ParametroService(ProveedorContext context) : base(context) { }

    public async Task<object> GetAllAsync()
    {
        try
        {
            var query = from mr in _context.Parametro
                        where mr.active == "S"
                        select new
                        {
                            mr.parametro,
                            mr.nombre,
                            mr.valor
                        };

            var result = await query.ToListAsync();
            return result;
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }
}