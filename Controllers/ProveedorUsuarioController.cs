using Microsoft.AspNetCore.Mvc;
using ProveedorApi.Models.ContentBody;
using ProveedorApi.Data;
using ProveedorApi.Helpers;
using ProveedorApi.Services;

namespace ProveedorApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProveedorUsuarioController : _BaseController
{
    public ProveedorUsuarioController(ProveedorContext context) : base(context) { }

    [HttpGet("all/{rucprov}")]
    public async Task<ActionResult<object>> GetAll(string rucprov)
    {
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        string rucProvSession = User.Claims.ToList()[0].Value;
        try
        {
            rucprov = roleSession == 2 ? rucProvSession : rucprov;
            // if (roleSession == 2) return Conflict(new { msg = "No tiene permiso para realizar esta acción" });
            var result = await new ProveedorUsuarioService(_context).GetAllAsync(rucprov);
            return result;
        }
        catch (System.Exception)
        {
            return NotFound();
        }
    }
    [HttpPost("resetpass")]
    public async Task<ActionResult> ResetPass(ResetPassBody body)
    {
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        string userNameSession = User.Claims.ToList()[2].Value;

        if (roleSession == 2) return Conflict(new { msg = "No tiene permiso para realizar esta acción" });

        try
        {
            var provuser = await new ProveedorUsuarioService(_context).GetItemAsync(body.ruc, body.usuario);
            if (provuser == null) return Conflict(new { msg = $"No hay datos del usuario {body.usuario}" });

            Tuple<int, string> tupleResult = await new ProveedorUsuarioService(_context).ResetPassAsync(provuser.ruc, provuser.username);
            if (tupleResult.Item1 == 0) return Conflict(new { msg = "No se pudo resetear la contraseña" });
            string passnew = tupleResult.Item2;

            string asunto = AppConfig.Mensajes.AsuntoReseteoClave;
            string msgCorreoHtml = "";

            msgCorreoHtml = AppConfig.Mensajes.MensajeReseteoClave.Replace("{nombre}", provuser.nombre).Replace("{apellido}", provuser.apellido).Replace("{website}", AppConfig.Configuracion.Website).Replace("{ruc}", provuser.ruc).Replace("{username}", provuser.username).Replace("{passnew}", passnew);

            MailManagerHelper mail = new MailManagerHelper();
            bool status = await mail.EnviarCorreoAsync(provuser.email, asunto, msgCorreoHtml, null, true);
            if (tupleResult.Item1 == 0) return Conflict(new { msg = $"Se reseteó la contraseña, pero no se pudo notificar a {provuser.nombre} ${provuser.apellido}" });
        }
        catch (System.Exception ex)
        {
            return Conflict(new { msg = ex.Message });
        }
        return NoContent();
    }
}