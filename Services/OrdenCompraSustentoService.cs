using Microsoft.EntityFrameworkCore;
using ProveedorApi.Data;
using ProveedorApi.Models;

namespace ProveedorApi.Services;

public class OrdenCompraSustentoService : _BaseService
{
    public OrdenCompraSustentoService(ProveedorContext context) : base(context) { }

    public async Task<object> GetAllAsync(string p_orden_compra, string p_embarque)
    {
        try
        {
            var query = from ocs in _context.OrdenCompraSustento
                        where ocs.active == "S"
                        && ocs.orden_compra == p_orden_compra
                        && ocs.embarque == p_embarque
                        orderby ocs.linea_sustento
                        select new
                        {
                            ocs.orden_compra,
                            ocs.embarque,
                            ocs.linea_sustento,
                            ocs.nombre_archivo_original,
                            ocs.nombre_archivo,
                            ocs.aceptado,
                            loading = false
                        };

            var result = await query.ToListAsync();
            return result;
        }
        catch (Exception)
        {
            return new object[] { };
        }
    }
    public async Task<List<OCSustentoForMail>> GetForMailAsync(string p_orden_compra, string p_embarque)
    {
        List<OCSustentoForMail> ocsList = new List<OCSustentoForMail>();
        try
        {
            var query = from ocs in _context.OrdenCompraSustento
                        where ocs.active == "S"
                        && ocs.orden_compra == p_orden_compra
                        && ocs.embarque == p_embarque
                        orderby ocs.linea_sustento
                        select new
                        {
                            ocs.linea_sustento,
                            ocs.nombre_archivo,
                            ocs.nombre_archivo_original
                        };

            var result = await query.ToListAsync();

            if (result != null)
            {

                result.ForEach(x =>
                {
                    ocsList.Add(new OCSustentoForMail()
                    {
                        linea_sustento = x.linea_sustento ?? 0,
                        nombre_archivo = x.nombre_archivo,
                        nombre_archivo_original = x.nombre_archivo_original
                    });
                });
            }
        }
        catch (Exception)
        {
        }
        return ocsList;
    }
    public async Task<Tuple<int, int, string>> SaveAsync(string p_orden_compra, string p_embarque, string p_nombre_original, string usersession)
    {
        try
        {
            var ocsustento = new OrdenCompraSustento();
            ocsustento.orden_compra = p_orden_compra;
            ocsustento.embarque = p_embarque;
            ocsustento.linea_sustento = OCSustentoItemTheLastLinea(p_orden_compra, p_embarque) + 1;
            ocsustento.nombre_archivo_original = p_nombre_original;
            ocsustento.nombre_archivo = $"{p_orden_compra}__{p_embarque}__sustento__{ocsustento.linea_sustento}.{p_nombre_original.Substring(p_nombre_original.LastIndexOf(".") + 1)}";
            ocsustento.aceptado ="S";
            ocsustento.active = "S";
            ocsustento.created_by = usersession;
            ocsustento.created_at = DateTime.Now;

            _context.OrdenCompraSustento.Add(ocsustento);
            int saveInt = await _context.SaveChangesAsync();
            if (saveInt > 0)
            {
                return Tuple.Create(saveInt, ocsustento.linea_sustento ?? 0, ocsustento.nombre_archivo);
            }
            return Tuple.Create(0, 0, "");
        }
        catch (System.Exception)
        {
            return Tuple.Create(0, 0, "");
        }
    }

    public async Task<int> DeleteAsync(string p_orden_compra, string p_embarque, int p_linea, string usersession)
    {
        try
        {
            var ocs = await _context.OrdenCompraSustento.Where(x => x.orden_compra == p_orden_compra && x.embarque == p_embarque && x.linea_sustento == p_linea && x.active == "S").FirstOrDefaultAsync();
            if (ocs != null)
            {
                ocs.active = "N";
                ocs.deleted_by = usersession;
                ocs.deleted_at = DateTime.Now;
                _context.OrdenCompraSustento.Update(ocs);
                _context.OrdenCompraSustento.Update(ocs);

                return await _context.SaveChangesAsync();
            }

            return 0;
        }
        catch (System.Exception)
        {
            return 0;
        }
    }
    public async Task<int> DeleteTotalAsync(string p_orden_compra, string p_embarque, int p_linea, string usersession)
    {
        try
        {
            var ocs = await _context.OrdenCompraSustento.Where(x => x.orden_compra == p_orden_compra && x.embarque == p_embarque && x.linea_sustento == p_linea && x.active == "S").FirstOrDefaultAsync();
            if (ocs != null)
            {
                _context.OrdenCompraSustento.Remove(ocs);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }
        catch (System.Exception)
        {
            return 0;
        }
    }

    private int OCSustentoItemTheLastLinea(string oc, string em)
    {
        var linea = _context.OrdenCompraSustento.Where(x => x.orden_compra == oc && x.embarque == em).OrderBy(x => x.linea_sustento).Select(x => x.linea_sustento).LastOrDefault();
        return linea ?? 0;
    }
}