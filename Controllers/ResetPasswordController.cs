using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using ProveedorApi.Models.ContentBody;
using ProveedorApi.Data;
using ProveedorApi.Helpers;
using ProveedorApi.Services;

namespace ProveedorApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ResetPasswordController : ControllerBase
{

    private readonly IConfiguration _config;
    private readonly ProveedorContext _context;
    public ResetPasswordController(IConfiguration configuration, ProveedorContext context)
    {
        _config = configuration;
        _context = context;
    }
    [HttpPost("requesNewPass")]
    public async Task<ActionResult> RequesNewPass(ResetPassBody body)
    {
        try
        {
            var provuser = await new ProveedorUsuarioService(_context).GetItemAsync(body.ruc, body.usuario);

            if (provuser == null) return Conflict(new { msg = "La información proporcionada no están en nuestro sistema." });

            string token = "";

            for (int i = 0; i < 2; i++)
            {
                token += Convert.ToBase64String((SHA512.Create()).ComputeHash(Encoding.ASCII.GetBytes(Guid.NewGuid().ToString()))).Replace("==", "").Replace("/", "o").Replace("+", "o");
            }

            DateTime expire_token = UtilityHelper.ExpireToken(_config["appConfig:Configuracion:ExpireResetToken"]);

            provuser.token_reset = token;
            provuser.token_reset_request = DateTime.Now;
            provuser.token_reset_expire = expire_token;

            int saveInt = await new ProveedorUsuarioService(_context).Update(provuser);
            if (saveInt == 0) return Conflict(new { msg = "No se pudo enviar registrar la solicitud. Intente nuevamente." });

            string asunto = "Solicitud de Cambio de contraseña";
            string msgCorreoHtml = "";

            msgCorreoHtml = $@"<p>Hola {provuser.nombre},</p>";
            msgCorreoHtml += "<p>Haz solicitado actualizar sus credenciales de acceso a nuestra plataforma</p>";
            msgCorreoHtml += $@"<p>Por favor has click en el botón";
            msgCorreoHtml += $@"<div><a href='{AppConfig.Configuracion.Website}resetpass/{token}' 
                                style='background-color: #3e3b5a;color: #fff;
                                padding: 0.6rem 0.8rem; 
                                border-radius: 0.5rem;
                                text-decoration: none;border: none;
                                font-size: .9rem;'>Restablecer contraseña</a></div>.";

            MailManagerHelper mail = new MailManagerHelper();
            bool status = await mail.EnviarCorreoAsync(provuser.email, asunto, msgCorreoHtml, null, true);
            if (!status) return Conflict(new { msg = $"No se pudo enviar el correo electrónico para restablecer su contraseña. Intenta nuevamente." });
        }
        catch (System.Exception)
        {
            return Conflict(new { msg = "No se pudo enviar registrar la solicitud. Intente nuevamente." });
        }
        return NoContent();
    }

    [HttpPost("existerequest")]
    public async Task<ActionResult> ExisteRequest(ExisteTokenBody body)
    {
        var provuser = await new ProveedorUsuarioService(_context).GetItemForTokenAsync(body.token);
        if (provuser == null) return NotFound();
        if (DateTime.Now >= provuser.token_reset_expire) return NotFound();
        return NoContent();
    }

    [HttpPost("resetpass")]
    public async Task<ActionResult> ResetPass(ResetPassPrvBody body)
    {
        try
        {
            if (string.IsNullOrEmpty(body.password) || string.IsNullOrEmpty(body.confirmPassword) || string.IsNullOrEmpty(body.token)) return Conflict(new { msg = "Tiene parámetros vacíos" });
            if (body.password != body.confirmPassword) return Conflict(new { msg = "La constraseña y su confirmación no coinciden" });

            var provuser = await new ProveedorUsuarioService(_context).GetItemForTokenAsync(body.token);
            if (provuser == null) return Conflict(new { msg = "El enlace ha expirado." });

            int updateInt = await new ProveedorUsuarioService(_context).ResetPassAsync(provuser, body.password);
            if (updateInt == 0) return Conflict(new { msg = "No se pudo actualizar su contraseña. Intente nuevamente." });
        }
        catch (System.Exception ex)
        {
            return Conflict(new { msg = ex.Message });
        }

        return NoContent();
    }
}