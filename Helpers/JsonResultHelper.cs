using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ProveedorApi.Data;
using ProveedorApi.Models;

namespace ProveedorApi.Helpers;
public class JsonResultHelper
{
    private readonly ProveedorContext _context = null!;
    private readonly TransporteContext _contextt = null!;
    private readonly ExactusExtContext _contexte = null!;
    public JsonResultHelper(ProveedorContext context) => _context = context;
    public JsonResultHelper(TransporteContext context) => _contextt = context;
    public JsonResultHelper(ExactusExtContext context) => _contexte = context;

    public async Task<T?> ToJsonResultAsync<T>(FormattableString execSP)
    {
        try
        {
            List<SpForJson>? result = null;

            if (_context != null)
            {
                result = await _context.SpForJson.FromSqlInterpolated(execSP).ToListAsync();
            }
            else if (_contexte != null)
            {
                result = await _contexte.SpForJson.FromSqlInterpolated(execSP).ToListAsync();
            }

            if (result == null) return default(T);
            var json_output = result[0].json_output;
            if (json_output == null) return default(T);
            return JsonSerializer.Deserialize<T>(json_output);
        }
        catch (System.Exception)
        {
            return default(T);
        }
    }

    public async Task<string> ToStringResultAsync<T>(FormattableString execSP)
    {
        try
        {
            List<SpForJson>? result = null;

            if (_context != null)
            {
                result = await _context.SpForJson.FromSqlInterpolated(execSP).ToListAsync();
            }
            else if (_contexte != null)
            {
                result = await _contexte.SpForJson.FromSqlInterpolated(execSP).ToListAsync();
            }

            if (result == null) return "";
            var json_output = result[0].json_output;
            return json_output ?? "";
        }
        catch (System.Exception)
        {
            return "";
        }
    }
}