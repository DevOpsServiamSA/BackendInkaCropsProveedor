using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ProveedorApi.Models.ContentBody;
using ProveedorApi.Data;

namespace ProveedorApi.Services;

public class UsuarioService : _BaseService
{
    public UsuarioService(ProveedorContext context) : base(context) { }

    public async Task<int> ChangePassAsync(string passactual, string passnew, string usersession)
    {
        var usuario = await _context.Usuario.Where(x => x.username == usersession && x.active == "S").FirstOrDefaultAsync();
        if (usuario == null) throw new Exception("No existe sus datos");

        byte[] passbyte = (SHA256.Create()).ComputeHash(Encoding.UTF8.GetBytes(passactual));
        if (Convert.ToBase64String(usuario.password) != Convert.ToBase64String(passbyte)) throw new Exception("La contraseña actual es incorrecta");

        usuario.password = (SHA256.Create()).ComputeHash(Encoding.UTF8.GetBytes(passnew));
        usuario.updated_at = DateTime.Now;
        usuario.updated_by = usersession;
        _context.Usuario.Update(usuario);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> EditPerfilAsync(PerfilBody body, string usersession)
    {
        var usuario = await _context.Usuario.Where(x => x.username == usersession && x.active == "S").FirstOrDefaultAsync();
        if (usuario == null) throw new Exception("No existe sus datos");

        usuario.nombre = body.nombre;
        usuario.apellido = body.apellido;
        usuario.email = body.email;
        usuario.updated_at = DateTime.Now;
        usuario.updated_by = usersession;
        _context.Usuario.Update(usuario);
        return await _context.SaveChangesAsync();
    }

    public async Task<object?> GetPerfilASync(string usersession)
    {
        return await _context.Usuario.Where(x => x.username == usersession && x.active == "S").Select(x => new { x.nombre, x.apellido, x.email }).FirstOrDefaultAsync();
    }
}