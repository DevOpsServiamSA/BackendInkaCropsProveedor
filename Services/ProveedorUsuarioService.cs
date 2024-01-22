using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ProveedorApi.Models.ContentBody;
using ProveedorApi.Data;
using ProveedorApi.Helpers;
using ProveedorApi.Models;

namespace ProveedorApi.Services;

public class ProveedorUsuarioService : _BaseService
{
    public ProveedorUsuarioService(ProveedorContext context) : base(context) { }

    public async Task<object> GetAllAsync(string p_prov_ruc)
    {
        try
        {
            var query = from p in _context.ProveedorUsuario
                        where p.active == "S"
                        && p.ruc == p_prov_ruc
                        orderby p.nombre
                        select new
                        {
                            p.ruc,
                            p.username,
                            p.nombre,
                            p.apellido,
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
    public async Task<ProveedorUsuario?> GetItemAsync(string ruc, string usuario)
    {
        try
        {
            return await _context.ProveedorUsuario.Where(x => x.ruc == ruc && x.username == usuario).FirstOrDefaultAsync();
        }
        catch (System.Exception)
        {
            return null;
        }
    }
    public async Task<List<ProveedorUsuario>?> GetItemsAsync(string ruc)
    {
        try
        {
            return await _context.ProveedorUsuario.Where(x => x.ruc == ruc).ToListAsync();
        }
        catch (System.Exception)
        {
            return null;
        }
    }
    public async Task<ProveedorUsuario?> GetItemForTokenAsync(string token)
    {
        try
        {
            return await _context.ProveedorUsuario.Where(x => x.token_reset == token).FirstOrDefaultAsync();
        }
        catch (System.Exception)
        {
            return null;
        }
    }
    public async Task<int> Update(ProveedorUsuario provuser)
    {
        _context.ProveedorUsuario.Update(provuser);
        return await _context.SaveChangesAsync();
    }
    public async Task<int> ChangePassAsync(string p_rucprov, string passactual, string passnew, string usersession)
    {
        var provuser = await _context.ProveedorUsuario.Where(x => x.ruc == p_rucprov && x.username == usersession && x.active == "S").FirstOrDefaultAsync();
        if (provuser == null) throw new Exception("No existe sus datos usuario");

        byte[] passbyte = (SHA256.Create()).ComputeHash(Encoding.UTF8.GetBytes(passactual));
        if (Convert.ToBase64String(provuser.password) != Convert.ToBase64String(passbyte)) throw new Exception("La contraseña actual es incorrecta");

        provuser.password = (SHA256.Create()).ComputeHash(Encoding.UTF8.GetBytes(passnew));
        provuser.updated_at = DateTime.Now;
        provuser.updated_by = usersession;
        _context.ProveedorUsuario.Update(provuser);
        return await _context.SaveChangesAsync();
    }
    public async Task<Tuple<int, string>> ResetPassAsync(string p_rucprov, string usuario)
    {
        var provuser = _context.ProveedorUsuario.Where(x => x.ruc == p_rucprov && x.username == usuario && x.active == "S").FirstOrDefault();
        if (provuser == null) throw new Exception("No existe datos del usuario");

        string passRandow = UtilityHelper.CreateRandomPassword(10);
        provuser.password = (SHA256.Create()).ComputeHash(Encoding.UTF8.GetBytes(passRandow));
        provuser.updated_at = DateTime.Now;
        provuser.updated_by = usuario;
        _context.ProveedorUsuario.Update(provuser);
        var resInt = await _context.SaveChangesAsync();
        return new Tuple<int, string>(resInt, passRandow);
    }

    public async Task<int> ResetPassAsync(ProveedorUsuario p_provuser, string password)
    {
        p_provuser.password = (SHA256.Create()).ComputeHash(Encoding.UTF8.GetBytes(password));
        p_provuser.updated_at = DateTime.Now;
        p_provuser.updated_by = p_provuser.username;
        p_provuser.token_reset = null;
        p_provuser.token_reset_expire = null;
        p_provuser.token_reset_request = null;
        _context.ProveedorUsuario.Update(p_provuser);

        return await _context.SaveChangesAsync();

    }

    public async Task<int> EditPerfilAsync(PerfilBody body, string rucsession, string usersession)
    {
        var provuser = await _context.ProveedorUsuario.Where(x => x.ruc == rucsession && x.username == usersession && x.active == "S").FirstOrDefaultAsync();
        if (provuser == null) throw new Exception("No existe sus datos usuario");

        provuser.nombre = body.nombre;
        provuser.apellido = body.apellido;
        provuser.email = body.email;
        provuser.updated_at = DateTime.Now;
        provuser.updated_by = usersession;
        _context.ProveedorUsuario.Update(provuser);
        return await _context.SaveChangesAsync();
    }

    public async Task<object?> GetPerfilASync(string rucsession, string usersession)
    {
        return await _context.ProveedorUsuario.Where(x => x.ruc == rucsession && x.username == usersession && x.active == "S").Select(x => new { x.nombre, x.apellido, x.email }).FirstOrDefaultAsync();
    }
}