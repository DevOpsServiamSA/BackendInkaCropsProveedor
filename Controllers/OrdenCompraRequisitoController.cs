using System.Diagnostics.Tracing;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProveedorApi.Models.ContentBody;
using ProveedorApi.Data;
using ProveedorApi.Helpers;
using ProveedorApi.Models;
using ProveedorApi.Models.ContentResponse;
using ProveedorApi.Services;


namespace ProveedorApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdenCompraRequisitoController : _BaseController
{
    private readonly ProveedorBCTSService _bctsService;

    public OrdenCompraRequisitoController(ProveedorContext context, ProveedorBCTSService bctsService) : base(context)
    {
        _bctsService = bctsService;
    }
    
    [HttpGet("all/{p_ruc}/{p_orden_compra}/{p_embarque}")]
    public async Task<ActionResult<object>> GetItems(string p_ruc, string p_orden_compra, string p_embarque)
    {
        try
        {
            int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
            string rucProvSession = User.Claims.ToList()[0].Value;

            if (roleSession == 2 && rucProvSession != p_ruc)
            {
                return new object[] { };
            }

            var result = await new OrdenCompraRequisitoService(_context).GetAllSPAsync(p_ruc, p_orden_compra, p_embarque);
            return result;
        }
        catch (System.Exception)
        {
            return NotFound();
        }
    }

    [HttpGet("requisitos/tipo/{p_ruc}/{p_oc}/{p_embarque}")]
    public async Task<ActionResult<object>> GetTiposRequisitos(string p_ruc, string p_oc, string p_embarque)
    {
        try
        {
            int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
            string rucProvSession = User.Claims.ToList()[0].Value;

            if (roleSession == 2 && rucProvSession != p_ruc)
            {
                return new object[] { };
            }

            var result = await new OrdenCompraRequisitoService(_context).GetTipoRequisitosSPAsync(p_ruc, p_oc, p_embarque);
            return result;
        }
        catch (System.Exception)
        {
            return NotFound();
        }
    }

    [HttpPost("uploadfile")]
    public async Task<ActionResult> UploadFile()
    {
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        string rucProvSession = User.Claims.ToList()[0].Value;
        string userNameSession = User.Claims.ToList()[2].Value;
        string p_ruc = string.Empty;
        string p_orden_compra = string.Empty;
        string p_embarque = string.Empty;
        int p_linea = 0;
        int p_tipo_requisito = 0;
        int p_item = 0;

        var dict = Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString());

        try
        {
            p_ruc = dict["ruc"];
            p_orden_compra = dict["orden_compra"];
            p_embarque = dict["embarque"];
            p_tipo_requisito = Int32.Parse(dict["tipo_requisito"]);
            p_item = Int32.Parse(dict["item"]);
            if (!Int32.TryParse(dict["linea"], out p_linea))
            {
                p_linea = 0;
            }
        }
        catch (Exception)
        {
            return Conflict(new { msg = "Error: Parámetros incompletos" });
        }

        if (string.IsNullOrEmpty(p_ruc) || string.IsNullOrEmpty(p_orden_compra) || string.IsNullOrEmpty(p_embarque) || p_tipo_requisito == 0)
        {
            return Conflict(new { msg = "Error: Parámetros vacíos" });
        }

        if (roleSession == 2 && rucProvSession != p_ruc)
        {
            return Conflict(new { msg = "Error: No tiene permiso para hacer esto" });
        }

        if (!Request.Form.Files.Any()) return Conflict(new { msg = "No se encontraron archivos para subir" });

        var _file = Request.Form.Files.First();
        var _filename = _file.FileName;
        var _path = Path.Combine(AppConfig.Configuracion.CarpetaArchivos, rucProvSession);

        bool status = await UtilityHelper.UploadFormFileAsync(_path, _filename, _file);
        
        if (!status) return Conflict(new { msg = "Error: No se pudo guardar el archivo" });

        int saveInt = await new OrdenCompraRequisitoService(_context).SaveAsync(p_ruc, p_orden_compra, p_embarque, p_linea, p_tipo_requisito, p_item, _filename, "S", userNameSession);

        if (saveInt == 0) return Conflict(new { msg = "Error: No hubo registro del archivo" });

        return NoContent();
    }

    [HttpPost("saveNoFile")]
    public async Task<ActionResult> SaveNoFile(SaveRequisitoNoFileBody sqnf)
    {
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        string rucProvSession = User.Claims.ToList()[0].Value;
        string userNameSession = User.Claims.ToList()[2].Value;

        if (string.IsNullOrEmpty(sqnf.orden_compra) || string.IsNullOrEmpty(sqnf.embarque) || sqnf.tipo_requisito == 0 || string.IsNullOrEmpty(sqnf.valor))
        {
            return Conflict(new { msg = "Error: Parámetros vacíos" });
        }
        if (roleSession == 2 && !new OrdenCompraService(_context).GetAnyItem(rucProvSession, sqnf.orden_compra, sqnf.embarque))
        {
            return Conflict(new { msg = "Error: No tiene permiso para hacer esto" });
        }

        try
        {
            int saveInt = await new OrdenCompraRequisitoService(_context).SaveAsync(rucProvSession, sqnf.orden_compra, sqnf.embarque, sqnf.linea, sqnf.tipo_requisito, sqnf.item, sqnf.valor, "S", userNameSession);
            if (saveInt == 0) return Conflict(new { msg = "Error: No se pudo guardar el cambio" });
        }
        catch (System.Exception ex)
        {
            return Conflict(new { msg = ex.Message });
        }
        return NoContent();
    }

    [HttpGet("fileDownload/{p_ruc}/{p_orden_compra}/{p_embarque}/{p_tipo_requisito}/{p_valor}")]
    public async Task<ActionResult<object>> FileDownload(string p_ruc, string p_orden_compra, string p_embarque, string p_tipo_requisito, string p_valor)
    {
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        string rucProvSession = User.Claims.ToList()[0].Value;
        try
        {
            if (roleSession == 2 && rucProvSession != p_ruc)
            {
                return NotFound();
            }

            string _file = Path.Combine(AppConfig.Configuracion.CarpetaArchivos, p_ruc, p_valor ?? "");
            if (!System.IO.File.Exists(_file))
            {
                return NotFound();
            }

            var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
            string? contentType;
            if (!provider.TryGetContentType(_file, out contentType))
            {
                contentType = "application/octet-stream";
            }

            byte[] filebyte = await System.IO.File.ReadAllBytesAsync(_file);

            if (filebyte.Length == 0)
            {
                return NotFound();
            }

            string base64 = Convert.ToBase64String(filebyte);

            return $"data:{contentType};base64,{base64}";

        }
        catch (System.Exception)
        {
            return NotFound();
        }
    }

    [HttpPost("delete")]
    public async Task<ActionResult<object>> Eliminar(EliminarRequisitoBody body)
    {
        string rucProvSession = User.Claims.ToList()[0].Value;
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);


        if (roleSession == 2 && rucProvSession != body.p_ruc)
        {
            return Conflict(new { msg = "No tienes autorización para realizar esta acción" });
        }

        int saveInt = await new OrdenCompraRequisitoService(_context).DeleteRequisito(body);
        if (saveInt == 0) return Conflict(new { msg = "Error: No se pudo eliminar la cotización" });


        string _path = Path.Combine(AppConfig.Configuracion.CarpetaArchivos, rucProvSession);
        string fileFullName = Path.Combine(_path, body.filename);
        UtilityHelper.DeleteFile(fileFullName);

        return NoContent();
    }

    // [HttpPost("validarenviar")]
    // public async Task<ActionResult<object>> ValidarEnviar(ValidarEnviarBody valenv)
    // {
    //     // -------------------------------------------------------------------------------------------------------------
    //     // Obtener token BTCS
    //     // -------------------------------------------------------------------------------------------------------------
    //     string tokenBcts = await _bctsService.GetAccessTokenBCTS();
    //     List<Adjunto> listAdjuntos = new List<Adjunto>();
    //     // -------------------------------------------------------------------------------------------------------------
    //     
    //     string fileBase64 = ""; 
    //     string nombreArchivo = ""; 
    //     string rucProvSession = User.Claims.ToList()[0].Value;
    //     string rsProvSession = User.Claims.ToList()[1].Value;
    //     string userNameSession = User.Claims.ToList()[2].Value;
    //     string subcarpeta = valenv.subcarpeta;
    //     string baseDirectory = AppConfig.Configuracion.CarpetaArchivosBCTS;
    //     string rucProvSessionPath = baseDirectory.TrimEnd('\\') + "\\" + rucProvSession; // Ajusta según el sistema operativo
    //
    //     var _pathSource = Path.Combine(AppConfig.Configuracion.CarpetaArchivos, rucProvSession);
    //     var _pathBCTSDestination = Path.Combine(rucProvSessionPath, subcarpeta);
    //     string filePrefix = valenv.orden_compra + "__" + valenv.embarque;
    //
    //     try
    //     {
    //         // Asegura que la ruta de destino exista antes de copiar los archivos
    //         Directory.CreateDirectory(_pathBCTSDestination);
    //
    //         // Obtener todos los archivos en el directorio origen que coinciden con el patrón
    //         var files = Directory.EnumerateFiles(_pathSource, $"{filePrefix}*.*");
    //
    //         foreach (var file in files)
    //         {
    //             // Obtener el nombre del archivo sin la ruta
    //             string fileName = Path.GetFileName(file);
    //             
    //             // Filtrar archivos que comienzan con el patrón específico
    //             if (fileName.Contains(filePrefix))
    //             {
    //                 // Verificar si el archivo es XML
    //                 if (Path.GetExtension(file).Equals(".xml", StringComparison.OrdinalIgnoreCase))
    //                 {
    //                     // Cambiar el nombre del archivo XML como se requiera
    //                     string newFileName = rucProvSession + "-" + subcarpeta + ".xml"; // Aquí puedes personalizar el nombre como lo necesites
    //                     string destinationPath = Path.Combine(_pathBCTSDestination, newFileName);
    //                     
    //                     // Leer el contenido del archivo y convertirlo a Base64
    //                     byte[] fileContentBytes = System.IO.File.ReadAllBytes(file);
    //                     fileBase64 = Convert.ToBase64String(fileContentBytes);
    //                     
    //                     // Copiar el archivo XML renombrado
    //                     System.IO.File.Copy(file, destinationPath, true);
    //                 }
    //                 else
    //                 {
    //                     var archivoInfo = new Adjunto { NombreArchivo = fileName };
    //                     // Copiar los demás archivos tal como están
    //                     string destinationPath = Path.Combine(_pathBCTSDestination, fileName);
    //                     
    //                     // Leer el contenido del archivo y convertirlo a Base64
    //                     byte[] fileContentBytes = System.IO.File.ReadAllBytes(file);
    //                     archivoInfo.Data = Convert.ToBase64String(fileContentBytes);
    //                     
    //                     // Agregar el objeto ArchivoInfo a la lista
    //                     listAdjuntos.Add(archivoInfo);
    //                     
    //                     System.IO.File.Copy(file, destinationPath, true);
    //                 }
    //             }
    //         }
    //
    //         Console.WriteLine("Los archivos han sido procesados y copiados correctamente.");
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine("Ocurrió un error: " + e.Message);
    //     }
    //     
    //     // -------------------------------------------------------------------------------------------------------------
    //     // Agregar la validacion del XML contra el ENDPOINT BCTS
    //     // -------------------------------------------------------------------------------------------------------------
    //     // Llamada a la función ValidaComprobanteBCTS con dynamic
    //     string error = await _bctsService.ValidaComprobanteBCTS(
    //         tokenBcts,
    //         rucProvSession,
    //         "",
    //         nombreArchivo,
    //         fileBase64,
    //         valenv.embarque
    //     );
    //
    //     if (!error.Equals(""))
    //     {
    //         return Conflict(new { msg = "Error: " + error });
    //     }
    //     else
    //     {
    //         
    //         // METODO DE ENVIO -- IMPLEMENTADO
    //         string response = await _bctsService.EnviarComprobanteBCTS(tokenBcts,
    //             rucProvSession,
    //             nombreArchivo,
    //             fileBase64,
    //             valenv.embarque,
    //             listAdjuntos);
    //
    //         if (!response.Equals(""))
    //         {
    //             return Conflict(new { msg="Error: " + error });
    //         }
    //
    //     }
    //
    //     // -------------------------------------------------------------------------------------------------------------
    //     
    //     try
    //     {
    //         bool existePendientes = new OrdenCompraRequisitoService(_context).GetStatusPendientes(valenv.ruc, valenv.orden_compra, valenv.embarque);
    //
    //         if (existePendientes) return Conflict(new { msg = "Aún tienes requisitos que completar" });
    //
    //         int saveIntEstadoEmbarque = await new OrdenCompraEmbarqueEstadoService(_context).SaveEstadoAsync(valenv.ruc, valenv.orden_compra, valenv.embarque, "2", userNameSession);
    //         int saveIntBitacora = await new OrdenCompraBitacoraService(_context).SaveAsync(valenv.orden_compra, valenv.embarque, "2", "Los requisitos fueron enviados", userNameSession);
    //
    //         List<OCRequisitoForMailResponse> ocrequisitos = await new OrdenCompraRequisitoService(_context).GetForMailAsync(valenv.ruc, valenv.orden_compra, valenv.embarque);
    //         List<OCSustentoForMail> ocsustentos = await new OrdenCompraSustentoService(_context).GetForMailAsync(valenv.orden_compra, valenv.embarque);
    //
    //         bool status = await EnviarCorreoRequisitosAsync(ocrequisitos, ocsustentos, rucProvSession, $"Embarque -  {valenv.embarque} - {rsProvSession}");
    //
    //         if (!status) return Conflict(new { msg = "Error: No se envío el correo electrónico con los requisitos. Por favor, intente nuevamente" });
    //     }
    //     catch (System.Exception)
    //     {
    //         return Conflict(new { msg = "Error: No se envío el correo electrónico con los requisitos. Por favor, intente nuevamente" });
    //     }
    //
    //     return NoContent();
    // }
    
    [HttpPost("validarenviar")]
    public async Task<ActionResult<object>> ValidarEnviar(ValidarEnviarBody valenv)
    {
        try
        {
            string rucProvSession = User.Claims.ToList()[0].Value;
            string rsProvSession = User.Claims.ToList()[1].Value;
            string userNameSession = User.Claims.ToList()[2].Value;

            string tokenBcts = await _bctsService.GetAccessTokenBCTS();
            var adjuntos = new List<Adjunto>();

            string fileBase64 = ""; 
            string nombreArchivo = ""; 
            string subcarpeta = valenv.subcarpeta;
            string baseDirectory = AppConfig.Configuracion.CarpetaArchivosBCTS;
            string rucProvSessionPath = Path.Combine(baseDirectory.TrimEnd('\\'), rucProvSession);

            var _pathSource = Path.Combine(AppConfig.Configuracion.CarpetaArchivos, rucProvSession);
            var _pathBCTSDestination = Path.Combine(rucProvSessionPath, subcarpeta);
            string filePrefix = valenv.orden_compra + "__" + valenv.embarque;

            var (_fileBase64, _nombreArchivo) = await ProcesarArchivosAsync(_pathSource, _pathBCTSDestination, filePrefix, adjuntos, rucProvSession, subcarpeta);
            string error = await _bctsService.ValidaComprobanteBCTS(tokenBcts, rucProvSession, "", _nombreArchivo, _fileBase64, valenv.embarque);

            if (!string.IsNullOrEmpty(error))
                return Conflict(new { msg = "Error: " + error });

            // METODO DE ENVIO -- IMPLEMENTADO
            string response = await _bctsService.EnviarComprobanteBCTS(tokenBcts, rucProvSession, _nombreArchivo, _fileBase64, valenv.embarque, adjuntos);

            if (!string.IsNullOrEmpty(response))
                return Conflict(new { msg = "Error: " + response });

            // Envio de correos, comletano el flujo
            try
            {
                bool existePendientes = new OrdenCompraRequisitoService(_context).GetStatusPendientes(valenv.ruc, valenv.orden_compra, valenv.embarque);

                if (existePendientes) return Conflict(new { msg = "Aún tienes requisitos que completar" });

                int saveIntEstadoEmbarque = await new OrdenCompraEmbarqueEstadoService(_context).SaveEstadoAsync(valenv.ruc, valenv.orden_compra, valenv.embarque, "2", userNameSession);
                int saveIntBitacora = await new OrdenCompraBitacoraService(_context).SaveAsync(valenv.orden_compra, valenv.embarque, "2", "Los requisitos fueron enviados", userNameSession);

                List<OCRequisitoForMailResponse> ocrequisitos = await new OrdenCompraRequisitoService(_context).GetForMailAsync(valenv.ruc, valenv.orden_compra, valenv.embarque);
                List<OCSustentoForMail> ocsustentos = await new OrdenCompraSustentoService(_context).GetForMailAsync(valenv.orden_compra, valenv.embarque);

                bool status = await EnviarCorreoRequisitosAsync(ocrequisitos, ocsustentos, rucProvSession, $"Embarque -  {valenv.embarque} - {rsProvSession}");

                if (!status) return Conflict(new { msg = "Error: No se envío el correo electrónico con los requisitos. Por favor, intente nuevamente" });
            }
            catch (System.Exception)
            {
                return Conflict(new { msg = "Error: No se envío el correo electrónico con los requisitos. Por favor, intente nuevamente" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return Conflict(new { msg = "Error: " + ex.Message });
        }
    }

    [HttpPost("notificarRechazo")]
    public async Task<ActionResult<object>> NotificarRechazo(NotificarRechazoBody notif)
    {
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        string userNameSession = User.Claims.ToList()[2].Value;

        if (roleSession == 2) return Conflict(new { msg = "Error: No tiene permiso para realizar esta acción" });
        try
        {
            // string asunto = AppConfig.Mensajes.AsuntoRechazoRequisitos;
            // string msgCorreoHml = $"<h4>Orden de compra: {notif.orden_compra} - Embarque {notif.embarque}</h4>{notif.mensaje_notificacion}";

            string asunto = "";
            string msgCorreoHml = "";
            string detalle = "";

            var proveedor = await new ProveedorService(_context).GetItemAsync(notif.ruc);
            var provusers = await new ProveedorUsuarioService(_context).GetItemsAsync(notif.ruc);

            List<string> toEmails = new List<string>();

            if (provusers != null) toEmails = provusers.Select(x => x.email).ToList();
            else toEmails.Add(proveedor?.email ?? AppConfig.Configuracion.DestinoCompraMail);

            // if (notif.requisitos != null)
            // {
            //     if (notif.requisitos.Count > 0)
            //     {
            //         msgCorreoHml += "<p>Detalle de los requisitos que fueron rechazados:</p>";
            //         msgCorreoHml += "<ul>";
            //         notif.requisitos.ForEach(x =>
            //         {
            //             msgCorreoHml += $"<li>{x.tipo_requisito_descripcion} : {x.valor}</li>";
            //         });
            //         msgCorreoHml += "</ul>";
            //     }
            // }

            if (notif.motivo_rechazo == 1)
            {
                asunto = AppConfig.Mensajes.AsuntoRechazoRequisitos;
                msgCorreoHml = AppConfig.Mensajes.MensajeRechazoRetencion.Replace("{ordencompra}", notif.orden_compra).Replace("{embarque}", notif.embarque).Replace("{razonsocial}", proveedor?.razonsocial ?? "").Replace("{website}", AppConfig.Configuracion.Website).Replace("{mensajeadicional}", notif.mensaje_notificacion); ;
            }
            else if (notif.motivo_rechazo == 2)
            {
                asunto = AppConfig.Mensajes.AsuntoRechazoRequisitos;
                msgCorreoHml = AppConfig.Mensajes.MensajeRechazoDetraccion.Replace("{ordencompra}", notif.orden_compra).Replace("{embarque}", notif.embarque).Replace("{razonsocial}", proveedor?.razonsocial ?? "").Replace("{website}", AppConfig.Configuracion.Website).Replace("{mensajeadicional}", notif.mensaje_notificacion); ;
            }
            else if (notif.motivo_rechazo == 3)
            {
                asunto = AppConfig.Mensajes.AsuntoBloqueoPorDeuda;
                msgCorreoHml = AppConfig.Mensajes.MensajeBloqueoPorDeuda.Replace("{ordencompra}", notif.orden_compra).Replace("{embarque}", notif.embarque).Replace("{razonsocial}", proveedor?.razonsocial ?? "").Replace("{website}", AppConfig.Configuracion.Website).Replace("{mensajeadicional}", notif.mensaje_notificacion); ;
            }
            else if (notif.motivo_rechazo == 4)
            {
                if (notif.requisitos == null || notif.requisitos.Count == 0)
                    return Conflict(new { msg = "Error: Marque al menos un requisito que se encuentra con error" });

                detalle += "<p>Detalle de los requisitos que fueron rechazados:</p>";
                detalle += "<ul>";
                notif.requisitos.ForEach(x =>
                {
                    String _item = "";
                    if (notif.requisitos.Where(y => y.tipo_requisito == x.tipo_requisito).Count() > 1)
                    {
                        _item = $" - ${x.item.ToString().PadLeft(3, '0')}";
                    }
                    detalle += $"<li>{x.tipo_requisito_descripcion}{_item} : {x.valor}</li>";
                });
                detalle += "</ul>";

                asunto = AppConfig.Mensajes.AsuntoRechazoRequisitos;
                msgCorreoHml = AppConfig.Mensajes.MensajeRechazoRequisitos.Replace("{ordencompra}", notif.orden_compra).Replace("{embarque}", notif.embarque).Replace("{razonsocial}", proveedor?.razonsocial ?? "").Replace("{website}", AppConfig.Configuracion.Website).Replace("{detalle}", detalle).Replace("{mensajeadicional}", notif.mensaje_notificacion);
            }
            else if (notif.motivo_rechazo == 5)
            {
                if (notif.sustentos == null || notif.sustentos.Count == 0)
                    return Conflict(new { msg = "Error: Marque al menos un sustento que se encuentra con error" });

                detalle += "<p>Detalle de los sustentos que fueron rechazados:</p>";
                detalle += "<ul>";
                notif.sustentos.ForEach(x =>
                {
                    detalle += $"<li>{x.nombre_archivo_original}</li>";
                });
                detalle += "</ul>";

                asunto = AppConfig.Mensajes.AsuntoRechazoSustento;
                msgCorreoHml = AppConfig.Mensajes.MensajeRechazoSustento.Replace("{ordencompra}", notif.orden_compra).Replace("{embarque}", notif.embarque).Replace("{razonsocial}", proveedor?.razonsocial ?? "").Replace("{website}", AppConfig.Configuracion.Website).Replace("{detalle}", detalle).Replace("{mensajeadicional}", notif.mensaje_notificacion);
            }

            int statusIntUpdate = await new OrdenCompraRequisitoService(_context).UpdateRechazoAsync(notif, userNameSession);

            if (statusIntUpdate == 0) return Conflict(new { msg = "Error: No se puede actualizar la notificación de rechazo." });


            MailManagerHelper mail = new MailManagerHelper();
            String emails = string.Join(",", toEmails);
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development") emails = "nfrankches@gmail.com";
            bool status = await mail.EnviarCorreoAsync(emails, asunto, msgCorreoHml, esHtlm: true);
            if (!status) return Conflict(new { msg = "Error: No se envío el correo electrónico de notificación de rechazo" });
        }
        catch (System.Exception)
        {
            return Conflict(new { msg = "Error: Hubo un incoveniente con el proceso de notificación de rechazo" });
        }

        return NoContent();
    }

    [HttpPost("reenviarEmail")]
    public async Task<ActionResult<object>> ReenviarEmail(ValidarEnviarBody body)
    {
        try
        {
            bool existePendientes = new OrdenCompraRequisitoService(_context).GetStatusPendientes(body.ruc, body.orden_compra, body.embarque);

            if (existePendientes) return Conflict(new { msg = "Hay requisitos que faltan completar" });

            Proveedor? provedor = await new ProveedorService(_context).GetItemAsync(body.ruc);

            List<OCRequisitoForMailResponse> ocrequisitos = await new OrdenCompraRequisitoService(_context).GetForMailAsync(body.ruc, body.orden_compra, body.embarque);
            List<OCSustentoForMail> ocsustentos = await new OrdenCompraSustentoService(_context).GetForMailAsync(body.orden_compra, body.embarque);

            bool status = await EnviarCorreoRequisitosAsync(ocrequisitos, ocsustentos, body.ruc, $"Embarque -  {body.embarque} - {provedor?.razonsocial ?? ""}");

            if (!status) return Conflict(new { msg = "Error: No se envío el correo electrónico con los requisitos. Por favor, intente nuevamente" });
        }
        catch (System.Exception)
        {
            return Conflict(new { msg = "Error: No se envío el correo electrónico con los requisitos. Por favor, intente nuevamente" });
        }
        return NoContent();
    }

    private async Task<bool> EnviarCorreoRequisitosAsync(List<OCRequisitoForMailResponse> pathAdjuntosList, List<OCSustentoForMail> pathSustentosAtt, string rucProv, string asunto)
    {
        bool status = false;
        try
        {
            MailManagerHelper mail = new MailManagerHelper();
            List<System.Net.Mail.Attachment>? attachList = null;

            string htmlBody = "";

            if (pathAdjuntosList.Count > 0)
            {
                attachList = new List<System.Net.Mail.Attachment>();
                pathAdjuntosList.ForEach(x =>
                           {
                               //Se comentan los requisitos:
                               //htmlBody += $"<p>{x.tipo_requisito_descripcion} ({(string.IsNullOrEmpty(x.extension) ? "" : "(Adjunto)")}) : {x.valor}</p>";

                               if (!string.IsNullOrEmpty(x.extension))
                               {
                                   string _file = Path.Combine(AppConfig.Configuracion.CarpetaArchivos, rucProv, x.valor ?? "");
                                   if (System.IO.File.Exists(_file))
                                   {
                                       attachList.Add(new System.Net.Mail.Attachment(_file));
                                   }
                               }
                           });
            }

            if (pathSustentosAtt.Count > 0)
            {
                if (attachList == null)
                {
                    attachList = new List<System.Net.Mail.Attachment>();
                }
                htmlBody += "<br><br>";
                pathSustentosAtt.ForEach(x =>
                {
                    // htmlBody += $"<p>SUSTENTO # {x.linea_sustento}: ({x.nombre_archivo_original})</p>";

                    if (!string.IsNullOrEmpty(x.nombre_archivo))
                    {
                        string _file = Path.Combine(AppConfig.Configuracion.CarpetaArchivos, rucProv, x.nombre_archivo ?? "");
                        if (System.IO.File.Exists(_file))
                        {
                            attachList.Add(new System.Net.Mail.Attachment(_file));
                        }
                    }
                });
            }
            status = await mail.EnviarCorreoAsync(AppConfig.Configuracion.DestinoRobotMail, asunto, htmlBody, attachList, true);
            attachList = null;
        }
        catch (Exception)
        {
            status = false;
        }

        return status;
    }
    
    private async Task<(string fileBase64, string nombreArchivo)> ProcesarArchivosAsync(string sourcePath, string destinationPath, 
            string filePrefix, List<Adjunto> adjuntos, string rucProvSession, string subcarpeta)
    {
        string fileBase64 = "";
        string nombreArchivo = "";

        try
        {
            await Task.Run(() =>
            {
               // Código de ProcesarArchivos 
                Directory.CreateDirectory(destinationPath);

                var files = Directory.EnumerateFiles(sourcePath, $"{filePrefix}*.*");

                foreach (var file in files)
                {
                    string fileName = Path.GetFileName(file);

                    if (fileName.Contains(filePrefix))
                    {
                        if (Path.GetExtension(file).Equals(".xml", StringComparison.OrdinalIgnoreCase))
                        {
                            string newFileName = rucProvSession + "-" + subcarpeta + ".xml";
                            string destinationFilePath = Path.Combine(destinationPath, newFileName);
                            nombreArchivo = newFileName;   
                            
                            byte[] fileContentBytes = System.IO.File.ReadAllBytes(file);
                            fileBase64 = Convert.ToBase64String(fileContentBytes);
                            
                            // Sacar copia en la ruta destinationPath del XML
                            System.IO.File.Copy(file, destinationFilePath, true);
                        }
                        else
                        {
                            var archivoInfo = new Adjunto { NombreArchivo = renombrar(fileName) };
                            string destinationFilePath = Path.Combine(destinationPath, fileName);

                            byte[] fileContentBytes = System.IO.File.ReadAllBytes(file);
                            archivoInfo.Data = Convert.ToBase64String(fileContentBytes);

                            adjuntos.Add(archivoInfo);
                            
                            // Sacar copia en la ruta destinationPath de los PDF
                            System.IO.File.Copy(file, destinationFilePath, true);
                        }
                    }
                }

                Console.WriteLine("Los archivos han sido procesados y copiados correctamente.");
            });
        }
        catch (Exception e)
        {
            Console.WriteLine("Ocurrió un error: " + e.Message);
            throw; // Lanzar la excepción para manejarla en el código que llama a este método
        }

        // Devolver los valores como una tupla
        return (fileBase64, nombreArchivo);
    }

    
    // private async Task<bool> EnviarCorreo2Async(string asunto, string htmlBody)
    // {
    //     bool status = false;
    //     try
    //     {
    //         MailManagerHelper mail = new MailManagerHelper();
    //         status = await mail.EnviarCorreoAsync(AppConfig.Configuracion.DestinoMail, asunto, htmlBody, null, true);
    //     }
    //     catch (Exception)
    //     {
    //         status = false;
    //     }

    //     return status;
    // }
    
    private string renombrar(string name)
    {
        // 1 DOCUMENTO (PDF)
        // 2 DOCUMENTO (XML)
        // 3 GUÍA DE REMISIÓN (PDF)
        // 4 OC (PDF)
        // 5 NOTA DE CRÉDITO (PDF)
        // 6 NOTA DE CRÉDITO (XML)
        // 7 FACTURA DE REFERENCIA

        // Los nombres que llegan con esta estructura: OC00051224__EM00069538__1__1 OrdenCompra__Embarque__TipoRequisito_TipoArchivo

        string nombreAsignado = "";

        // Dividir el nombre en partes usando el doble guion bajo como separador
        string[] partes = name.Split("__");

        if (partes.Length == 4)
        {
            // Obtener las partes relevantes del nombre
            string ordenCompra = partes[0];
            string embarque = partes[1];
            string documento = "";
            string tipoArchivo = partes[3];
            
            // Mapear los valores de tipoRequisito y tipoArchivo según tus indicaciones
            switch (partes[2])
            {
                case "1":
                    documento = "DOCUMENTOPDF";
                    break;
                case "2":
                    documento = "DOCUMENTOXML";
                    break;
                case "3":
                    documento = "GUIAPDF";
                    break;
                case "4":
                    documento = "OCPDF";
                    break;
                // Agrega más casos según sea necesario para otros valores de tipoRequisito
            }

            // Crear el nuevo nombre con los valores actualizados
            nombreAsignado = $"{ordenCompra}__{embarque}__{documento}__{tipoArchivo}";
        }

        return nombreAsignado;
    }


}