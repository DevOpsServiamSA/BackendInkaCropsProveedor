using Microsoft.AspNetCore.Mvc;
using ProveedorApi.Models.ContentBody;
using ProveedorApi.Data;
using ProveedorApi.Helpers;
using ProveedorApi.Services;

namespace ProveedorApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SettingController : _BaseController
{
    public SettingController(ProveedorContext context) : base(context) { }

    [HttpPost("changepass")]
    public async Task<ActionResult> ChangePass(ChangePassBody body)
    {
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        string rucProvSession = User.Claims.ToList()[0].Value;
        string userNameSession = User.Claims.ToList()[2].Value;

        if (string.IsNullOrEmpty(body.passactual) || string.IsNullOrEmpty(body.passnew) || string.IsNullOrEmpty(body.passnewconfirm))
        {
            return Conflict(new { msg = "Ingrese todos los datos solicitados" });
        }

        if (body.passnew != body.passnewconfirm)
        {
            return Conflict(new { msg = "La confirmación de su nueva contraseña no coincide" });
        }

        if (body.passactual == body.passnewconfirm)
        {
            return Conflict(new { msg = "su contraseña nueva tiene que ser diferente al actual" });
        }

        switch (roleSession)
        {
            case 1:
                try
                {
                    int statusIntChange = await new UsuarioService(_context).ChangePassAsync(body.passactual, body.passnew, userNameSession);
                    if (statusIntChange == 0) return Conflict(new { msg = "No se pudo cambiar su contraseña." });
                }
                catch (System.Exception ex)
                {
                    return Conflict(new { msg = ex.Message });
                }
                break;
            case 2:
                try
                {
                    int statusIntChange = await new ProveedorUsuarioService(_context).ChangePassAsync(rucProvSession, body.passactual, body.passnew, userNameSession);
                    if (statusIntChange == 0) return Conflict(new { msg = "No se pudo cambiar su contraseña." });
                }
                catch (System.Exception ex)
                {
                    return Conflict(new { msg = ex.Message });
                }
                break;
            default:
                return Conflict(new { msg = "Su perfil no aplica esta  funcionalidad." });
        }

        return NoContent();
    }

    [HttpPost("perfil/edit")]
    public async Task<ActionResult> EditPerfil(PerfilBody body)
    {
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        string rucProvSession = User.Claims.ToList()[0].Value;
        string userNameSession = User.Claims.ToList()[2].Value;

        if (string.IsNullOrEmpty(body.nombre) || string.IsNullOrEmpty(body.apellido) || string.IsNullOrEmpty(body.email))
        {
            return Conflict(new { msg = "Ingrese todos los datos solicitados" });
        }

        if (!UtilityHelper.IsValidEmail(body.email))
        {
            return Conflict(new { msg = "Email no es válido" });
        }

        switch (roleSession)
        {
            case 1:
                try
                {
                    int statusIntChange = await new UsuarioService(_context).EditPerfilAsync(body, userNameSession);
                    if (statusIntChange == 0) return Conflict(new { msg = "No se pudo editar perfil" });
                }
                catch (System.Exception ex)
                {
                    return Conflict(new { msg = ex.Message });
                }
                break;
            case 2:
                try
                {
                    int statusIntChange = await new ProveedorUsuarioService(_context).EditPerfilAsync(body, rucProvSession, userNameSession);
                    if (statusIntChange == 0) return Conflict(new { msg = "No se pudo editar perfil" });
                }
                catch (System.Exception ex)
                {
                    return Conflict(new { msg = ex.Message });
                }
                break;
            default:
                return Conflict(new { msg = "Su perfil no aplica esta  funcionalidad." });
        }

        return NoContent();
    }

    [HttpGet("perfil")]
    public async Task<ActionResult<object>> GetPerfil()
    {
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        string rucProvSession = User.Claims.ToList()[0].Value;
        string userNameSession = User.Claims.ToList()[2].Value;

        switch (roleSession)
        {
            case 1:
                try
                {
                    var result = await new UsuarioService(_context).GetPerfilASync(userNameSession);
                    if (result == null) return new object { };
                    return result;
                }
                catch (System.Exception ex)
                {
                    return Conflict(new { msg = ex.Message });
                }
            case 2:
                try
                {
                    var result = await new ProveedorUsuarioService(_context).GetPerfilASync(rucProvSession, userNameSession);
                    if (result == null) return new object { };
                    return result;
                }
                catch (System.Exception ex)
                {
                    return Conflict(new { msg = ex.Message });
                }
            default:
                return Conflict(new { msg = "Su perfil no aplica esta  funcionalidad." });
        }
    }
}