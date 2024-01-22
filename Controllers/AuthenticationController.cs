using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProveedorApi.Auth;
using ProveedorApi.Models.ContentBody;
using ProveedorApi.Data;
using ProveedorApi.Helpers;
using ProveedorApi.Models;
using ProveedorApi.Services;

namespace ProveedorApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly ProveedorContext _context;
    public AuthenticationController(IConfiguration configuration, ProveedorContext context)
    {
        _config = configuration;
        _context = context;
    }
    [HttpPost]
    public async Task<IActionResult> PostAuth(Authentication auth)
    {
        string message = Validar(auth);
        if (message.Trim().Length > 0) return Ok(new { message });
        //

        string[] lAuthResult = await Authenticate(auth);
        //

        if (lAuthResult.Length == 0)
        {
            return Ok(new { message = "Intente nuevamente" });
        }
        if (!string.IsNullOrEmpty(lAuthResult[0]))
        {
            return Ok(new { message = lAuthResult[0] });
        }
        if (string.IsNullOrEmpty(lAuthResult[1]))
        {
            return Ok(new { message = "Intente nuevamente" });
        }
        return Ok(new { token = lAuthResult[1] });
    }

    private string Validar(Authentication auth)
    {
        if (auth.isproveedor && string.IsNullOrEmpty(auth.ruc))
        {
            return "Ingrese el RUC de su empresa";
        }
        if (string.IsNullOrEmpty(auth.username))
        {
            return "Ingrese su nombre usuario";
        }
        if (string.IsNullOrEmpty(auth.password))
        {
            return "Ingrese su contraseña";
        }
        return "";
    }

    private async Task<string[]> Authenticate(Authentication auth)
    {
        String[] lresult = new string[2];

        string userMater = _config["MasterUser"];
        string passMater = _config["MasterPassword"];
        string emailMater = _config["MasterEmail"];

        if (auth.username == userMater && auth.password == passMater)
        {
            var _tokenModel = new TokenModel();
            _tokenModel.perfil_id = 1;
            if (auth.isproveedor)
            {
                var proveedor = await _context.Proveedor.Where(x => x.ruc == auth.ruc && x.active == "S").FirstOrDefaultAsync();
                if (proveedor == null)
                {
                    lresult[0] = "Proveedor no habilitado";
                    return lresult;
                }

                _tokenModel.ruc = proveedor.ruc;
                _tokenModel.razonsocial = proveedor.razonsocial;
                _tokenModel.perfil_id = 2;
            }
            _tokenModel.username = userMater;
            _tokenModel.nombre = "Master";
            _tokenModel.apellido = "";
            _tokenModel.email = emailMater;
            _tokenModel.isproveedor = auth.isproveedor;
            _tokenModel.read_only = false;
            lresult[1] = new TokenAuth(_config).Token(_tokenModel);

            return lresult;
        }

        byte[] _password = (SHA256.Create()).ComputeHash(Encoding.UTF8.GetBytes(auth.password));
        if (!auth.isproveedor)
        {
            var user = await _context.Usuario.Where(x => x.username == auth.username && x.active == "S").FirstOrDefaultAsync();
            if (user == null)
            {
                lresult[0] = "Usuario no habilitado";
                return lresult;
            }
            if (Convert.ToBase64String(user.password) != Convert.ToBase64String(_password))
            {
                lresult[0] = "Contraseña incorrecta";
                return lresult;
            }



            lresult[1] = new TokenAuth(_config).Token(
                new TokenModel
                {
                    username = user.username,
                    nombre = user.nombre,
                    apellido = user.apellido,
                    email = user.email,
                    isproveedor = auth.isproveedor,
                    perfil_id = user.perfil_id,
                    read_only = user.read_only,

                });
        }
        else
        {
            var user = await _context.ProveedorUsuario.Where(x => x.ruc == auth.ruc && x.username == auth.username && x.active == "S").FirstOrDefaultAsync();
            if (user == null)
            {
                lresult[0] = "Usuario no habilitado";
                return lresult;
            }
            if (Convert.ToBase64String(user.password) != Convert.ToBase64String(_password))
            {
                lresult[0] = "Contraseña incorrecta";
                return lresult;
            }

            var proveedor = await _context.Proveedor.Where(x => x.ruc == auth.ruc && x.active == "S").FirstOrDefaultAsync();

            if (proveedor == null)
            {
                lresult[0] = "Proveedor no habilitado";
                return lresult;
            }

            lresult[1] = new TokenAuth(_config).Token(
                new TokenModel
                {
                    ruc = user.ruc,
                    razonsocial = proveedor.razonsocial,
                    username = user.username,
                    nombre = user.nombre,
                    apellido = user.apellido,
                    email = user.email,
                    isproveedor = auth.isproveedor,
                    perfil_id = user.perfil_id,
                    read_only = false
                });

            await new ProveedorUsuarioLogService(_context).New(user.ruc, user.username);
        }
        return lresult;
    }


    #region Solicitar acceso a la plataforma
    [HttpGet("requestAccess/getproveedor/{ruc}")]
    public async Task<string> GetProveedorForRequesstAccesoAsync(string ruc) => await GetValidarProveedorAsync(ruc);


    [HttpPost("requestAccess")]
    public async Task<ActionResult> ResquestAccesAsync(RequestAccessBody req)
    {
        string msg = await ValidarReqAccessAsync(req);
        if (msg.Trim().Length > 0) return Conflict(new { msg });
        try
        {
            var _solicitudes = _context.SolicitudAcceso.Where(x => x.ruc == req.ruc && x.usuario == req.usuario && x.active == "S").ToList();

            if (_solicitudes != null)
            {
                if (_solicitudes.Any(x => x.estado == "R"))
                {
                    msg = "Ya hemos recibido tu solicitud de acceso a la plataforma anteriormente, por favor, espera la respuesta.";
                    return Conflict(new { msg });
                }
                if (_solicitudes.Any(x => x.estado == "S"))
                {
                    msg = $"Ese nombre de usuario {req.usuario} de {req.razonsocial} ya está en uso.";
                    return Conflict(new { msg });
                }
            }


            var solicitud = new SolicitudAcceso
            {
                id = (int)0,
                ruc = req.ruc,
                razonsocial = req.razonsocial,
                nombre = req.nombre,
                apellido = req.apellido,
                usuario = req.usuario,
                email = req.email,
                estado = "R",
                active = "S",
                created_at = DateTime.Now
            };

            _context.SolicitudAcceso.Add(solicitud);
            await _context.SaveChangesAsync();
        }
        catch (System.Exception)
        {
            return Conflict(new { msg = "Hubo un problema al intentar registrar la solicitud de acceso" });
        }

        try
        {
            string htmlBody = AppConfig.Mensajes.MensajeSolicitudAcceso.Replace("{nombre}", req.nombre).Replace("{apellido}", req.apellido).Replace("{razonsocial}", req.razonsocial);
            _ = await new MailManagerHelper().EnviarCorreoAsync(req.email ?? "", AppConfig.Mensajes.AsuntoSolicitudAcceso, htmlBody, null, true);
        }
        catch (Exception)
        {
        }
        try
        {
            string htmlBody = AppConfig.Mensajes.MensajeSolicitudAccesoAdmin.Replace("{nombreproveedor}", req.razonsocial).Replace("{website}", AppConfig.Configuracion.Website);
            _ = await new MailManagerHelper().EnviarCorreoAsync(AppConfig.Configuracion.DestinoCompraMail, AppConfig.Mensajes.AsuntoSolicitudAccesoAdmin, htmlBody, null, true);
        }
        catch (Exception)
        {
        }
        return NoContent();
    }

    private async Task<string> ValidarReqAccessAsync(RequestAccessBody reqA)
    {
        #region Validación de vacíos
        if (reqA.ruc.Length == 0)
        {
            return "Ingrese su número de RUC";
        }

        if (reqA.razonsocial.Length == 0)
        {
            return "Ingrese la razón social";
        }

        string razonsocial = await GetValidarProveedorAsync(reqA.ruc);


        if ((razonsocial ?? "").Length == 0)
        {
            return $"El proveedor no existe";
        }

        if (reqA.nombre.Length == 0)
        {
            return "Ingrese su nombre";
        }

        if (reqA.apellido.Length == 0)
        {
            return "Ingrese su apellido";
        }

        if (reqA.usuario.Length == 0)
        {
            return "Ingrese su usuario";
        }

        if (reqA.email.Length == 0)
        {
            return "Ingrese su email";
        }
        #endregion
        #region Otras validaciones
        if (reqA.ruc.Length != 11)
        {
            return "El número de RUC debe tener 11 dígitos";
        }
        if (!UtilityHelper.IsValidEmail(reqA.email))
        {
            return "El email no es válido";
        }
        #endregion

        return "";
    }

    private async Task<string> GetValidarProveedorAsync(string ruc)
    {
        return await new JsonResultHelper(_context).ToStringResultAsync<string>($"exec oc_get_get_proveedor_for_new_request {ruc}");
    }
    #endregion
}