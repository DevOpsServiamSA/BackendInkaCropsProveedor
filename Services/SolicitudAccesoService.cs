using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ProveedorApi.Data;
using ProveedorApi.Helpers;
using ProveedorApi.Models;

namespace ProveedorApi.Services;

public class SolicitudAccesoService : _BaseService
{
    public SolicitudAccesoService(ProveedorContext context) : base(context) { }

    public async Task<object> GetAllAsync(DateTime fi, DateTime ff, string? estado)
    {
        try
        {
            var result = await new JsonResultHelper(_context)
            .ToJsonResultAsync<object>($"exec oc_get_solicitud_acceso {fi.Date}, {ff.Date}, {estado}");
            return result ?? new object[] { };
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }

    public async Task<int> AprobarRechazarAsync(SolicitudAccesoBody body, string username)
    {
        try
        {
            var solicitud = await _context.SolicitudAcceso.Where(x => x.id == body.id && x.ruc == body.ruc && x.usuario == body.usuario).FirstOrDefaultAsync();

            if (solicitud == null) return 0;
            solicitud.estado = body.estado;
            solicitud.updated_by = username;
            solicitud.updated_at = DateTime.Now;
            solicitud.password = UtilityHelper.CreateRandomPassword(10);
            _context.SolicitudAcceso.Update(solicitud);

            if (solicitud.estado == "N")
            {
                return await _context.SaveChangesAsync();
            }

            var prov = _context.Proveedor.Any(x => x.ruc == body.ruc && x.active == "S");

            if (!prov)
            {
                var proveedor = new Proveedor();
                proveedor.ruc = solicitud.ruc;
                proveedor.razonsocial = solicitud.razonsocial;
                proveedor.email = solicitud.email;
                proveedor.active = "S";
                proveedor.created_at = DateTime.Now;
                proveedor.created_by = username;

                _context.Proveedor.Add(proveedor);
            }


            var provusuario = new ProveedorUsuario();
            provusuario.ruc = solicitud.ruc;
            provusuario.username = solicitud.usuario;
            provusuario.nombre = solicitud.nombre;
            provusuario.apellido = solicitud.apellido;
            provusuario.email = solicitud.email;
            provusuario.password = (SHA256.Create()).ComputeHash(Encoding.UTF8.GetBytes(solicitud.password));
            provusuario.perfil_id = 2;
            provusuario.active = "S";
            provusuario.created_at = DateTime.Now;
            provusuario.created_by = username;

            _context.ProveedorUsuario.Add(provusuario);

            return await _context.SaveChangesAsync();

        }
        catch (System.Exception)
        {
            return 0;
        }
    }
    public async Task<SolicitudAcceso?> GetItemAsync(string ruc, string usuario)
    {
        try
        {
            return await _context.SolicitudAcceso.Where(x => x.ruc == ruc && x.usuario == usuario).FirstOrDefaultAsync();
        }
        catch (System.Exception)
        {
            return null;
        }
    }
    public async Task<SolicitudAcceso?> GetItemAsync(int id, string ruc, string usuario)
    {
        try
        {
            return await _context.SolicitudAcceso.Where(x => x.id == id && x.ruc == ruc && x.usuario == usuario).FirstOrDefaultAsync();
        }
        catch (System.Exception)
        {
            return null;
        }
    }
}