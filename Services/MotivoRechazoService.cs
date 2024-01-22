using Microsoft.EntityFrameworkCore;
using ProveedorApi.Data;

namespace ProveedorApi.Services;

public class MotivoRechazoService : _BaseService
{
    public MotivoRechazoService(ProveedorContext context) : base(context) { }

    public async Task<object> GetAllAsync()
    {
        try
        {
            var query = from mr in _context.MotivoRechazo
                        where mr.active == "S"
                        select new
                        {
                            mr.motivo_rechazo,
                            mr.descripcion,
                            mr.mensaje_plantilla
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