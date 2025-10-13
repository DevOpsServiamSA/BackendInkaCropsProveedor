using Microsoft.AspNetCore.Mvc;
using ProveedorApi.Models.ContentBody;
using ProveedorApi.Data;
using ProveedorApi.Helpers;
using ProveedorApi.Models;
using ProveedorApi.Services;

namespace ProveedorApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SolicitudAccesoController : _BaseController
{
    public SolicitudAccesoController(ProveedorContext context) : base(context) { }

    [HttpGet("all/{estado}/{fecha_inicio}/{fecha_fin}")]
    public async Task<ActionResult<object>> GetItems(string estado, string fecha_inicio, string fecha_fin)
    {
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        try
        {
            if (roleSession == 2) return Conflict(new { msg = "No tiene permiso para realizar esta acción" });
            DateTime fi = DateTime.Parse(fecha_inicio);
            DateTime ff = DateTime.Parse(fecha_fin);
            string? _estado = estado == "T" ? null : estado;
            var result = await new SolicitudAccesoService(_context).GetAllAsync(fi, ff, _estado);
            return result;
        }
        catch (System.Exception)
        {
            return NotFound();
        }
    }
    [HttpPost("aprobarrechazar")]
    public async Task<ActionResult> AprobarRechazar(SolicitudAccesoBody body)
    {
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        string userNameSession = User.Claims.ToList()[2].Value;

        if (roleSession != 1 && roleSession !=3) return Conflict(new { msg = "No tiene permiso para realizar esta acción" });
        if (!(new[] { "S", "SS", "N" }.Contains(body.estado))) return Conflict(new { msg = "Estados no permitidos!" });
        try
        {
            if (body.estado != "SS")
            {
                if (body.estado == "S")
                {
                    var provuser = await new ProveedorUsuarioService(_context).GetItemAsync(body.ruc, body.usuario);
                    if (provuser != null) return Conflict(new { msg = "El usuario del este proveedor, ya existe. Debe rechazarlo" });
                }

                int statusIntUpdate = await new SolicitudAccesoService(_context).AprobarRechazarAsync(body, userNameSession);
                if (statusIntUpdate == 0) return Conflict(new { msg = "Error: No se pudo actualizar el estado" });

            }

            var solicitud = await new SolicitudAccesoService(_context).GetItemAsync(body.id, body.ruc, body.usuario);

            // string msj = AppConfig.Mensajes.MensajeBienvenida;

            if (solicitud == null) return Conflict(new { msg = $"No se pudo notificar {(body.estado == "S" ? "la aprobación" : "el rechazo")} la solicitud!" });

            string asunto = "Respuesta a la solicitud de Acceso a la plaforma";
            string msgCorreoHtml = "";

            if (body.estado == "N")
            {
                msgCorreoHtml = $@"<p>Hola {solicitud.nombre}</p>";
                msgCorreoHtml += $@"<p>Lamentamos informale que su solicitud de acceso a nuestra plataforma Órdenes de Compra INKACROPS ha sido rechazada.</p>";
                msgCorreoHtml += $@"<p>Si desea más información sobre el motivo de este rechazo, por favor, escríbanos a: {AppConfig.Configuracion.DestinoCompraMail}</p>";
                msgCorreoHtml += $@"<p>Atentamente</p>";
                msgCorreoHtml += $@"<p><b>INKACROPS</b></p>";
            }
            else
            {
                msgCorreoHtml = $@"<p>Enhorabuena, {solicitud.nombre}!. Es  un gusto informale que su solicitud de acceso a nuestra plataforma Órdenes de Compra INKACROPS ha sido aceptada.</p>";
                msgCorreoHtml += $@"<p>Ingrese a: <a href='{AppConfig.Configuracion.Website}login'>{AppConfig.Configuracion.Website}login</a></p>";
                msgCorreoHtml += $@"<p>Sus credenciales son:</p>";
                msgCorreoHtml += $@"<ul><li>RUC: {solicitud.ruc}</li><li>Usuario: {solicitud.usuario}</li><li>Contraseña: {solicitud.password}</li></ul>";
                msgCorreoHtml += $@"<p>La clave enviada es autogenerada por nuestro sistema. Por cuestiones de su seguridad, le recomendamos cambiarla al ingresar por primera vez a nuestra plataforma.";
                msgCorreoHtml += $@"<p>Si tiene problemas para acceder al sistema, por favor escríbanos a: {AppConfig.Configuracion.DestinoCompraMail}</p>";
                msgCorreoHtml += $@"<p>Atentamente</p>";
                msgCorreoHtml += $@"<p><b>INKACROPS</b></p>";
            }


            MailManagerHelper mail = new MailManagerHelper();
            bool status = await mail.EnviarCorreoAsync(solicitud?.email ?? "", asunto, msgCorreoHtml, null, true);
            // if (!status) return Conflict(new { msg = "No se envío el correo electrónico de notificación de rechazo!" });

        }
        catch (System.Exception)
        {
            return Conflict(new { msg = "Error: Hubo un incoveniente con el proceso de actualización de estado" });
        }

        return NoContent();
    }
}