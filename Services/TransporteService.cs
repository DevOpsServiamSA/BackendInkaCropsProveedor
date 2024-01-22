using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ProveedorApi.Data;

namespace ProveedorApi.Services;

public class TransporteService : _BaseService
{
    public TransporteService(TransporteContext context) : base(context) { }

    public async Task<object> GetSegAllSPAsync(string fecha_solicitada_ini, string fecha_solicitada_fin, string fecha_requerida_ini, string fecha_requerida_fin, string fecha_programada_ini,
        string fecha_programada_fin, string fecha_entregada_ini, string fecha_entregada_fin, byte turno, byte estado, byte tipo_programacion)
    {
        try
        {
            var result = await _contextt.TransporteSegResponse.FromSqlInterpolated($"exec oc_get_requerimiento_transporte {fecha_solicitada_ini}, {fecha_solicitada_fin}, {fecha_requerida_ini}, {fecha_requerida_fin}, {fecha_programada_ini}, {fecha_programada_fin}, {fecha_entregada_ini}, {fecha_entregada_fin}, {turno}, {estado}, {tipo_programacion}").ToListAsync();
            if (result == null) return new object[] { };
            return result;
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }
}