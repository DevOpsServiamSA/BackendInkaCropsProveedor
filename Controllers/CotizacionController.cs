using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ProveedorApi.Models.ContentBody;
using ProveedorApi.Data;
using ProveedorApi.Helpers;
using ProveedorApi.Services;

namespace ProveedorApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CotizacionController : _BaseController
{

    public CotizacionController(ExactusExtContext context) : base(context) { }

    [HttpGet("all/{grupo_estado}/{fecha_inicio}/{fecha_fin}")]
    public async Task<ActionResult<object>> GetAll(int grupo_estado, string fecha_inicio, string fecha_fin)
    {
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        if (roleSession == 2) return new Object[] { };

        try
        {
            DateTime p_fi = DateTime.Parse(fecha_inicio);
            DateTime p_ff = DateTime.Parse(fecha_fin);
            byte? p_ge = grupo_estado == -1 ? null : (byte)grupo_estado;

            var result = await new CotizacionService(_contexte).GetAllASync(p_ge, p_fi, p_ff);
            return result;
        }
        catch (System.Exception)
        {
            return new Object[] { };
        }
    }
    [HttpGet("bitacora/{cot}")]
    public async Task<ActionResult<object>> GetBitacora(int cot)
    {
        // int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        // if (roleSession == 2) return new Object[] { };

        try
        {
            var result = await new CotizacionService(_contexte).GetBitacoraAsync(cot);
            return result;
        }
        catch (System.Exception)
        {
            return new Object[] { };
        }
    }

    [HttpGet("grupo_estado/all")]
    public async Task<ActionResult<object>> GetGrupoEstado()
    {
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        if (roleSession == 2) return new Object[] { };

        try
        {
            return await new CotizacionService(_contexte).GetGrupoEstadoAllASync();
        }
        catch (System.Exception)
        {
            return new Object[] { };
        }
    }
    [HttpGet("archivo/download/{cot}/{filename}")]
    public async Task<ActionResult<object>> ArchivoDownload(int cot, string filename)
    {
        filename = Path.Combine(AppConfig.Configuracion.CarpetaArchivos, "cotizacion", cot.ToString(), filename);
        string URLb64 = await UtilityHelper.GetFileUrlB64Async(filename);

        if (URLb64 == string.Empty) return NotFound();

        return URLb64;
    }

    [HttpGet("archivos/download/{cot}/{filenames}")]
    public async Task<ActionResult<object>> ArchivosDownload(int cot, string filenames)
    {
        List<object>? obj = null;
        try
        {
            string[] names = filenames.Split(";");
            if (names.Length > 0)
            {
                obj = new List<object>();
                for (int i = 0; i < names.Length; i++)
                {
                    string name = Path.Combine(AppConfig.Configuracion.CarpetaArchivos, "cotizacion", cot.ToString(), names[i]);
                    string URLb64 = await UtilityHelper.GetFileUrlB64Async(name);
                    if (URLb64 == string.Empty) continue;
                    obj.Add(new { b64 = URLb64, name = names[i] });
                }

                return obj;
            }

        }
        catch (System.Exception)
        {
            return NotFound();
        }
        return NotFound();
    }
    [HttpGet("categoria/all")]
    public async Task<ActionResult<object>> GetCategoriaAll()
    {
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        if (roleSession == 2) return new Object[] { };

        try
        {
            var result = await new CotizacionService(_contexte).GetCategoriaAllASync();
            return result;
        }
        catch (System.Exception)
        {
            return new Object[] { };
        }
    }

    [HttpGet("condicionpago/all")]
    public async Task<ActionResult<object>> GetCondicionPagoAll()
    {
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        try
        {
            var result = await new CotizacionService(_contexte).GetCondicionPagoAllASync();
            return result;
        }
        catch (System.Exception)
        {
            return new Object[] { };
        }
    }

    [HttpGet("monedas/all")]
    public async Task<ActionResult<object>> GetMonedasAll()
    {
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        try
        {
            var result = await new CotizacionService(_contexte).GetMonedasAllASync();
            return result;
        }
        catch (System.Exception)
        {
            return new Object[] { };
        }
    }

    [HttpGet("solicitudes/{cotcodigo}")]
    public async Task<ActionResult<object>> GetSolicitudes(int cotcodigo)
    {
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        if (roleSession == 2) return new Object[] { };

        try
        {
            var result = await new CotizacionService(_contexte).GetSolicitudesSync(cotcodigo);
            return result;
        }
        catch (System.Exception)
        {
            return new Object[] { };
        }
    }

    [HttpGet("requerimiento/")]
    public async Task<ActionResult<object>> GetRequerimiento()
    {
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        if (roleSession == 2) return new Object[] { };

        try
        {
            var result = await new CotizacionService(_contexte).GetRequerimientoSync();
            return result;
        }
        catch (System.Exception)
        {
            return new Object[] { };
        }
    }

    [HttpGet("proveedores/{cot}")]
    public async Task<ActionResult<object>> GetProveedores(int cot)
    {
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        if (roleSession == 2) return new Object[] { };

        try
        {
            var result = await new CotizacionService(_contexte).GetProveedoresASync(cot);
            return result;
        }
        catch (System.Exception)
        {
            return new Object[] { };
        }
    }

    [HttpGet("solicitudes/proveedor/{fecha_inicio}/{fecha_fin}")]
    public async Task<ActionResult<object>> GetSolicitudesProveedor(string fecha_inicio, string fecha_fin)
    {
        string rucProvSession = User.Claims.ToList()[0].Value;
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        if (roleSession == 1) return new Object[] { };

        try
        {
            DateTime fi = DateTime.Parse(fecha_inicio);
            DateTime ff = DateTime.Parse(fecha_fin);

            var result = await new CotizacionService(_contexte).GetSolicitudesProveedorSync(rucProvSession, fi, ff);
            return result;
        }
        catch (System.Exception)
        {
            return new Object[] { };
        }
    }

    [HttpGet("solicitudes/proveedor/detalle/{cot}/{cos}/{csp}/{moneda}")]
    public async Task<ActionResult<object>> GetSolicitudesProveedorDetalle(int cot, int cos, int csp, string moneda)
    {
        string rucProvSession = User.Claims.ToList()[0].Value;
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        if (roleSession == 1) return new Object[] { };

        try
        {
            var result = await new CotizacionService(_contexte).GetSolicitudesProveedorDetalleSync(rucProvSession, cot, cos, csp, moneda);
            return result;
        }
        catch (System.Exception)
        {
            return new Object[] { };
        }
    }
    [HttpGet("solicitudes/proveedor/attachments/{cot}/{cos}/{csp}")]
    public async Task<ActionResult<object>> GetSolicitudesProveedorAttachment(int cot, int cos, int csp)
    {
        string rucProvSession = User.Claims.ToList()[0].Value;
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        if (roleSession == 1) return new Object[] { };

        try
        {
            var result = await new CotizacionService(_contexte).GetSolicitudesProveedorAttachmentsSync(rucProvSession, cot, cos, csp);
            return result;
        }
        catch (System.Exception)
        {
            return new Object[] { };
        }
    }

    [HttpPost("solicitudes/proveedor/attachment/uploadfile")]
    public async Task<ActionResult> UploadFileAttachment()
    {
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        string rucProvSession = User.Claims.ToList()[0].Value;
        string userNameSession = User.Claims.ToList()[2].Value;

        string p_cot = string.Empty;
        string p_cos = string.Empty;
        string p_csp = string.Empty;

        var dict = Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString());

        try
        {
            p_cot = dict["cot"];
            p_cos = dict["cos"];
            p_csp = dict["csp"];
        }
        catch (Exception)
        {
            return Conflict(new { msg = "Parámetros incompletos" });
        }

        if (string.IsNullOrEmpty(p_cot) || string.IsNullOrEmpty(p_cos) || string.IsNullOrEmpty(p_csp))
        {
            return Conflict(new { msg = "Parámetros vacíos" });
        }


        if (roleSession != 2)
        {
            return Conflict(new { msg = "No tiene permiso para hacer esto" });
        }

        if (!Request.Form.Files.Any()) return Conflict(new { msg = "No hay archivos que subir" });

        var _file = Request.Form.Files.First();
        var _filename_original = _file.FileName.Length > 500 ? _file.FileName.Substring(1, 497) + "..." + _file.FileName.Substring(_file.FileName.LastIndexOf(".")) : _file.FileName;

        CotizacionService servicio = new CotizacionService(_contexte);

        Tuple<int, int, string> saveInt = await servicio.UploadFileAttachmentAsync(Int32.Parse(p_cot), Int32.Parse(p_cos), Int32.Parse(p_csp), _filename_original, userNameSession);
        if (saveInt.Item1 == 0) return Conflict(new { msg = "No hubo registro del archivo" });

        bool status = await UtilityHelper.UploadFormFileAsync(Path.GetDirectoryName(saveInt.Item3) ?? "", saveInt.Item3, _file);
        if (!status)
        {
            await new CotizacionService(_contexte).DeleteTotalAttachmentAsync(Int32.Parse(p_cot), Int32.Parse(p_cos), Int32.Parse(p_csp), saveInt.Item2, userNameSession);
            return Conflict(new { msg = "No se pudo guardar el archivo" });
        }

        return NoContent();
    }

    [HttpPut("solicitudes/proveedor/attachment/delete")]
    public async Task<ActionResult> DeleteFileAttachment(CotizacionSPAttachementDeleteBody body)
    {
        try
        {
            int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
            string rucProvSession = User.Claims.ToList()[0].Value;
            string userNameSession = User.Claims.ToList()[2].Value;

            if (roleSession != 2)
            {
                return Conflict(new { msg = "No tiene permiso para hacer esto" });
            }

            int saveInt = await new CotizacionService(_contexte).DeleteTotalAttachmentAsync(body.cot, body.cos, body.csp, body.item, userNameSession);

            string filenameFull = Path.Combine(AppConfig.Configuracion.CarpetaArchivos, "cotizacion", body.cot.ToString(), body.cos.ToString(), body.csp.ToString(), body.filename);

            if (System.IO.File.Exists(filenameFull)) System.IO.File.Delete(filenameFull);
        }
        catch (System.Exception ex)
        {
            return Conflict(new { msg = ex.Message });
        }


        return NoContent();
    }

    [HttpGet("solicitudes/proveedor/attachment/download/{cot}/{cos}/{csp}/{filename}")]
    public async Task<ActionResult<object>> AttachmentDownload(int cot, int cos, int csp, string filename)
    {
        // string rucProvSession = User.Claims.ToList()[0].Value;
        // int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);

        filename = Path.Combine(AppConfig.Configuracion.CarpetaArchivos, "cotizacion", cot.ToString(), cos.ToString(), csp.ToString(), filename);
        string URLb64 = await UtilityHelper.GetFileUrlB64Async(filename);

        if (URLb64 == string.Empty) return NotFound();

        return URLb64;
    }

    [HttpGet("cuadrocomparativo/{cot}/{moneda}")]
    public async Task<ActionResult<object>> GetCuadroComparativo(int cot, string moneda)
    {
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        if (roleSession == 2) return new Object[] { };

        try
        {
            string? _moneda = moneda == "-" ? null : moneda;
            var result = await new CotizacionService(_contexte).GetCuadroComparativoSync(cot, _moneda);
            return result;
        }
        catch (System.Exception)
        {
            return new Object[] { };
        }
    }

    [HttpGet("cuadrocomparativo/ultimacompraarticulo/{articulo}")]
    public async Task<ActionResult<object>> GetUltimaCompraArticuloSync(string articulo)
    {
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        if (roleSession == 2) return new Object[] { };

        try
        {
            var result = await new CotizacionService(_contexte).GetUltimaCompraArticuloSync(articulo);
            return result;
        }
        catch (System.Exception)
        {
            return new Object[] { };
        }
    }


    [HttpGet("pendientesxgeneraoc")]
    public async Task<ActionResult<object>> GetPendientesxGeneraOC()
    {
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        if (roleSession == 2) return new Object[] { };

        try
        {
            var result = await new CotizacionService(_contexte).GetPendientesxGeneraOC();
            return result;
        }
        catch (System.Exception)
        {
            return new Object[] { };
        }
    }

    #region POST METODO
    [HttpPost("new")]
    public async Task<ActionResult<object>> SaveCotizacion(CotizacionBody body)
    {
        string username = User.Claims.ToList()[2].Value;
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        if (roleSession == 2) return Conflict(new { msg = "Error: Tu perfil no permite realizar esta acción." });

        try
        {
            int saveInt = await new CotizacionService(_contexte).SaveCotizacionSync(body, Request.Form.Files, username);
            if (saveInt == 0) return Conflict(new { msg = "Error: No se pudo guardar la cotización" });
        }
        catch (System.Exception ex)
        {
            return Conflict(new { msg = ex.Message });
        }

        return NoContent();
    }
    [HttpPost("new2")]
    public async Task<ActionResult<object>> SaveCotizacion()
    {
        string username = User.Claims.ToList()[2].Value;
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        if (roleSession == 2) return Conflict(new { msg = "Error: Tu perfil no permite realizar esta acción." });

        var dict = Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString());
        string data = dict["data"];
        var body = JsonSerializer.Deserialize<CotizacionBody>(data);

        if (body == null) return Conflict(new { msg = "Error: No se enviaron los datos para su registro." });

        try
        {
            int saveInt = await new CotizacionService(_contexte).SaveCotizacionSync(body, Request.Form.Files, username);
            if (saveInt == 0) return Conflict(new { msg = "Error: No se pudo guardar la cotización" });
        }
        catch (System.Exception ex)
        {
            return Conflict(new { msg = ex.Message });
        }

        return NoContent();
    }

    [HttpPut("finalizar/{codigo}")]
    public async Task<ActionResult<object>> FinalizarCotizacion(int codigo)
    {
        string username = User.Claims.ToList()[2].Value;
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        if (roleSession != 1) return Conflict(new { msg = "Error: Tu perfil no permite realizar esta acción." });

        var servicio = new CotizacionService(_contexte);
        int saveInt = await servicio.FinalizarCotizacion(codigo, username);
        await servicio.UpdateStatusDetalleOriginalCasoAccionFinalizar(codigo);
        if (saveInt == 0) return Conflict(new { msg = "Error: No se pudo finalizar la cotización" });

        return NoContent();
    }

    [HttpPost("solicitud/new/{cotcodigo}")]
    public async Task<ActionResult<object>> SaveSolicitud(CotizacionSolicitudBody body, int cotcodigo)
    {
        string username = User.Claims.ToList()[2].Value;
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        if (roleSession == 2) return Conflict(new { msg = "Error: Tu perfil no permite realizar esta acción." });

        if (body.cotcodigo != cotcodigo) return Conflict(new { msg = "Error: El código de solicitud no coincide." });
        try
        {
            CotizacionService service = new CotizacionService(_contexte);

            int saveInt = await service.SaveCotizacionSolicitudSync(body, cotcodigo, username);
            if (saveInt == 0) return Conflict(new { msg = "Error: No se pudo guardar la solicitud" });

            await service.UpdateStatusCotizacionJob(body.cotcodigo);


            #region ARCHIVOS ADJUNTOS

            var cotizacion = await new CotizacionService(_contexte).GetOneSync(body.cotcodigo);
            string[] _archivos = cotizacion?.archivos?.Split(";") ?? new String[] { };
            List<System.Net.Mail.Attachment>? attachList = null;

            if (_archivos.Length > 0)
            {
                attachList = new List<System.Net.Mail.Attachment>();
                for (int i = 0; i < _archivos.Length; i++)
                {
                    _archivos[i] = Path.Combine(AppConfig.Configuracion.CarpetaArchivos, "cotizacion", body.cotcodigo.ToString(), _archivos[i]);
                    if (System.IO.File.Exists(_archivos[i]))
                    {
                        attachList.Add(new System.Net.Mail.Attachment(_archivos[i]));
                    }
                }
            }

            #endregion


            string htmlString = await service.GetHtmlForSendSolicitudSync(body.cotcodigo);

            htmlString = htmlString.Replace("{{website}}", AppConfig.Configuracion.Website);
            htmlString = htmlString.Replace("{{vigencia_ini}}", body.fecha_vigencia_ini.Date.ToString("dd/MM/yyyy"));
            htmlString = htmlString.Replace("{{vigencia_fin}}", body.fecha_vigencia_fin.Date.ToString("dd/MM/yyyy"));

            await Parallel.ForEachAsync(body.proveedores, new ParallelOptions() { MaxDegreeOfParallelism = 20 }, async (x, CancellationToken) =>
            {
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development") x.email = "nfrankches@gmail.com";
                if (x.email != null)
                {
                    await new MailManagerHelper().EnviarCorreoAsync(x.email, cotizacion?.cot_descripcion ?? $"Solicitud de Cotización", htmlString.Replace("{{ruc}}", x.proveedor).Replace("{{razonsocial}}", x.razonsocial), attachList, true);
                }
                // if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development") x.email = "nfrankches@gmail.com";
                // if (x.email != null && UtilityHelper.IsValidEmail(x.email)) await new MailManagerHelper().EnviarCorreoAsync(x.email, "Solicitud de nueva cotización", htmlString.Replace("{{ruc}}", x.proveedor).Replace("{{razonsocial}}", x.razonsocial), attachList, true);
            });
        }
        catch (System.Exception ex)
        {
            return Conflict(new { msg = ex.Message });
        }

        return NoContent();
    }

    [HttpPut("solicitud/changevigencia")]
    public async Task<ActionResult<object>> SaveSolicitudChangeVigencia(CotizacionSolicitudChangeVigencia body)
    {

        string username = User.Claims.ToList()[2].Value;
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        if (roleSession == 2) return Conflict(new { msg = "Error: Tu perfil no permite realizar esta acción." });

        try
        {
            var service = new CotizacionService(_contexte);
            int saveInt = await service.SaveSolicitudChangeVigenciaSync(body, username);
            if (saveInt == 0) return Conflict(new { msg = "Error: No se pudo modificar la  vigencia" });

            await service.UpdateStatusCotizacionJob(body.cot);
            
            string htmlString = await service.GetHtmlForChangeVigenciaSolicitudSync(body.cot);

            var cotizacion = await new CotizacionService(_contexte).GetOneSync(body.cot);

            htmlString = htmlString.Replace("{{website}}", AppConfig.Configuracion.Website);
            htmlString = htmlString.Replace("{{vigencia_ini}}", body.vigencia_ini.Date.ToString("dd/MM/yyyy"));
            htmlString = htmlString.Replace("{{vigencia_fin}}", body.vigencia_fin.Date.ToString("dd/MM/yyyy"));

            await Parallel.ForEachAsync(body.proveedores, new ParallelOptions() { MaxDegreeOfParallelism = 20 }, async (x, CancellationToken) =>
            {
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development") x.email = "nfrankches@gmail.com";
                if (x.email != null)
                {
                    await new MailManagerHelper().EnviarCorreoAsync(x.email, cotizacion?.cot_descripcion ?? $"Solicitud de Cotización", htmlString.Replace("{{ruc}}", x.proveedor).Replace("{{razonsocial}}", x.razonsocial), null, true);
                }
                // if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development") x.email = "nfrankches@gmail.com";
                // if (x.email != null && UtilityHelper.IsValidEmail(x.email)) await new MailManagerHelper().EnviarCorreoAsync(x.email, "Solicitud de nueva cotización", htmlString.Replace("{{ruc}}", x.proveedor).Replace("{{razonsocial}}", x.razonsocial), attachList, true);
            });
        }
        catch (System.Exception ex)
        {
            return Conflict(new { msg = ex.Message });
        }

        return NoContent();
    }

    [HttpPost("solicitud/proveedor/responder/{cotcodigo}/{coscodigo}/{cspcodigo}")]
    public async Task<ActionResult<object>> ResponderSolicitud(CotizacionSolicitudResponderBody body, int cotcodigo, int coscodigo, int cspcodigo)
    {
        string userRuc = User.Claims.ToList()[0].Value;
        string userRazonSocial = User.Claims.ToList()[1].Value;
        string username = User.Claims.ToList()[2].Value;
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);

        if (roleSession == 1) return Conflict(new { msg = "Error: Tu perfil no permite realizar esta acción." });

        if (body.cotcodigo != cotcodigo) return Conflict(new { msg = "Error: El código de cotización no coincide." });
        if (body.coscodigo != coscodigo) return Conflict(new { msg = "Error: El código de solicitud no coincide." });
        if (body.cspcodigo != cspcodigo) return Conflict(new { msg = "Error: El código de solicitud del provedor no coincide." });

        try
        {
            int saveInt = await new CotizacionService(_contexte).ResponderCotizacionSolicitudSync(body, username);
            if (saveInt == 0) return Conflict(new { msg = "Error: No se pudo responder la solicitud. Verifique que aún este en fecha" });

            var cotizacion = await new CotizacionService(_contexte).GetOneSync(body.cotcodigo);

            #region VARIABLES TEMPORALES DE CORREO

            // string to = "jchavez@filasur.com"; // obtener el nombre y correo del proveedor
            string htmlBody = @$"<p>Estimado administrador, el proveedor <strong>{userRazonSocial}</strong> con  RUC: <strong>{userRuc}</strong></p>
                                <p>ha respondido la solicitud de la cotización {cotizacion?.cot_descripcion} 
                                #{cotizacion?.cot_codigo}-{body.coscodigo}-{body.cspcodigo}, por favor ingrese a la plataforma 
                                <a href='{AppConfig.Configuracion.Website}'>{AppConfig.Configuracion.Website}</a> si desea verlo.</p>
                                <p>Gracias</p>";
            await new MailManagerHelper().EnviarCorreoAsync(AppConfig.Configuracion.DestinoCompraMail, "Respuesta a solicitud de nueva cotización", htmlBody, null, true);

            #endregion
        }
        catch (System.Exception ex)
        {
            return Conflict(new { msg = ex.Message });
        }

        return NoContent();
    }


    [HttpPut("cuadrocomparativo/save/precioelegido")]
    public async Task<ActionResult<object>> SavePrecioElegido(CotizacionPrecioElegido body)
    {
        string username = User.Claims.ToList()[2].Value;
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        if (roleSession == 2) return Conflict(new { msg = "Error: Tu perfil no permite realizar esta acción." });

        try
        {
            CotizacionService service = new CotizacionService(_contexte);
            int saveInt = await service.SavePrecioElegidoAsync(body, username);
            if (saveInt == 0) return Conflict(new { msg = "Error: No se guardar el precio seleccionado" });
        }
        catch (System.Exception ex)
        {
            return Conflict(new { msg = ex.Message });
        }

        return NoContent();
    }

    [HttpPut("pendientesxgeneraoc/generaroc")]
    public async Task<ActionResult<object>> GenerarOC(List<GenerarOCBody> body)
    {
        string username = User.Claims.ToList()[2].Value;
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        if (roleSession != 1) return Conflict(new { msg = "Error: Tu perfil no permite realizar esta acción." });
        int saveInt = await new CotizacionService(_contexte).GenerarOC(body, username);
        if (saveInt == 0) return Conflict(new { msg = "Error: No se pudo generar la orden de compra" });

        return NoContent();
    }

    #endregion


    #region DELETE METHOD
    [HttpDelete("delete/{codigo}")]
    public async Task<ActionResult<object>> DeleteCotizacion(int codigo)
    {
        string username = User.Claims.ToList()[2].Value;
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        if (roleSession != 1) return Conflict(new { msg = "Error: Tu perfil no permite realizar esta acción." });
        int saveInt = await new CotizacionService(_contexte).DeleteCotizacion(codigo, username);
        if (saveInt == 0) return Conflict(new { msg = "Error: No se pudo eliminar la cotización" });

        return NoContent();
    }
    #endregion
}