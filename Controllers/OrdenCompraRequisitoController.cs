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
                //contentType = "application/octet-stream";
                if (Path.GetExtension(_file).ToLower() == ".xml")
        contentType = "text/xml";
    else
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
    
    [HttpPost("validarenviar")]
    public async Task<ActionResult<object>> ValidarEnviar(ValidarEnviarBody valenv)
    {
        try
        {
            string rucProvSession = User.Claims.ToList()[0].Value;
            string rsProvSession = User.Claims.ToList()[1].Value;
            string userNameSession = User.Claims.ToList()[2].Value;
            
            // METODO GENERAR TOKEN -- BCTS
            // string tokenBcts = await _bctsService.GetAccessTokenBCTS();
            var adjuntos = new List<Adjunto>();

            string fileBase64 = ""; 
            string nombreArchivo = ""; 
            string subcarpeta = valenv.subcarpeta;
            string baseDirectory = AppConfig.Configuracion.CarpetaArchivosBCTS;
            string rucProvSessionPath = Path.Combine(baseDirectory.TrimEnd('\\'), rucProvSession);

            var _pathSource = Path.Combine(AppConfig.Configuracion.CarpetaArchivos, rucProvSession);
            var _pathBCTSDestination = Path.Combine(rucProvSessionPath, subcarpeta);
            string filePrefix = valenv.orden_compra + "__" + valenv.embarque;
            
            bool nota_credito = await new OrdenCompraService(_context).GetOrdenCompraEmbarque(valenv.orden_compra, valenv.embarque);
            var (_fileBase64, _nombreArchivo) = await ProcesarArchivosAsync(_pathSource, _pathBCTSDestination, filePrefix, adjuntos, rucProvSession, subcarpeta, nota_credito);

            // METODO DE VALIDAR -- BCTS
            // string error = await _bctsService.ValidaComprobanteBCTS(tokenBcts, rucProvSession, "", _nombreArchivo, _fileBase64, valenv.embarque);
            //
            // if (!string.IsNullOrEmpty(error))
            //     return Conflict(new { msg = "Error: " + error });

            // METODO DE ENVIO -- BCTS
            // string response = await _bctsService.EnviarComprobanteBCTS(tokenBcts, rucProvSession, _nombreArchivo, _fileBase64, valenv.embarque, adjuntos);
            //
            // if (!string.IsNullOrEmpty(response))
            //     return Conflict(new { msg = "Error: " + response });

            // Envio de correos, comletano el flujo
            try
            {
                bool existePendientes = new OrdenCompraRequisitoService(_context).GetStatusPendientes(valenv.ruc, valenv.orden_compra, valenv.embarque);

                // if (existePendientes) return Conflict(new { msg = "Aún tienes requisitos que completar" });

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
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development") emails = "jleon@serviam.com.pe";
            bool status = await mail.EnviarCorreoAsync(emails, asunto, msgCorreoHml, esHtml: true);
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
    
    [HttpPost("sendExactus")]
    public async Task<ActionResult<object>> sendExactus(ValidarEnviarBody valenv)
    {
        try
        {
            string rucProvSession = valenv.ruc;
            int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);

            if (roleSession == 2) return Conflict(new { msg = "Error: No tiene permiso para realizar esta acción" });
            
            // METODO GENERAR TOKEN -- BCTS
            string tokenBcts = await _bctsService.GetAccessTokenBCTS();
            var adjuntos = new List<Adjunto>();

            string fileBase64 = ""; 
            string nombreArchivo = ""; 
            string subcarpeta = valenv.subcarpeta;
            
            var _pathSource = Path.Combine(AppConfig.Configuracion.CarpetaArchivos, rucProvSession);
            string filePrefix = valenv.orden_compra + "__" + valenv.embarque;
            
            bool nota_credito = await new OrdenCompraService(_context).GetOrdenCompraEmbarque(valenv.orden_compra, valenv.embarque);
            var (_fileBase64, _nombreArchivo, _adjuntos) = await buildInformationExactusAsync(_pathSource, filePrefix, adjuntos, rucProvSession, subcarpeta, nota_credito);

            // METODO DE VALIDAR -- BCTS
            string error = await _bctsService.ValidaComprobanteBCTS(tokenBcts, rucProvSession, "", _nombreArchivo, _fileBase64, valenv.embarque);
            
            if (!string.IsNullOrEmpty(error))
                return Conflict(new { msg = "Error: " + error });

            // METODO DE ENVIO -- BCTS
            string response = await _bctsService.EnviarComprobanteBCTS(tokenBcts, rucProvSession, _nombreArchivo, _fileBase64, valenv.embarque, adjuntos);
            
            if (!string.IsNullOrEmpty(response))
                return Conflict(new { msg = "Error: " + response });
            
            return NoContent();
        }
        catch (Exception ex)
        {
            return Conflict(new { msg = "Error: " + ex.Message });
        }
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
            string filePrefix, List<Adjunto> adjuntos, string rucProvSession, string subcarpeta, bool nota_credito = false)
    {
        string fileBase64 = "";
        string nombreArchivo = "";
        string bucleFileBase64 = "";
        string bucleNombreArchivo = "";
        string[] tipoDocumento = new[] { (subcarpeta.Split('-'))[0] };
        
        try
        {
            await Task.Run(() =>
            {
               // Código de ProcesarArchivos 
                Directory.CreateDirectory(destinationPath);
                IEnumerable<string> files;
                
                if (nota_credito) {
                    files = Directory.EnumerateFiles(sourcePath, $"{filePrefix}__5__*.*")
                        .Concat(Directory.EnumerateFiles(sourcePath, $"{filePrefix}__6__*.*"))
                        .ToList();
                } else {
                    files = Directory.EnumerateFiles(sourcePath, $"{filePrefix}*.*");
                }
                
                int xmlCounter = 0; // Contador para archivos XML
                
                foreach (var file in files)
                {
                    string fileName = Path.GetFileName(file);

                    if (fileName.Contains(filePrefix))
                    {
                        var archivoInfo = new Adjunto {  };
                        if (Path.GetExtension(file).Equals(".xml", StringComparison.OrdinalIgnoreCase))
                        {
                            // Incrementamos el contador para archivos XML
                            xmlCounter++;

                            // Crear un nuevo nombre único para el XML
                            string newFileName = $"{rucProvSession}-{subcarpeta}-{xmlCounter}.xml";
                            string destinationFilePath = Path.Combine(destinationPath, newFileName);
                            bucleNombreArchivo = newFileName;   

                            byte[] fileContentBytes = System.IO.File.ReadAllBytes(file);
                            bucleFileBase64 = Convert.ToBase64String(fileContentBytes);

                            // Agregar información del XML al objeto archivoInfo
                            archivoInfo.NombreArchivo = bucleNombreArchivo;  // Nombre del archivo XML
                            archivoInfo.Data = bucleFileBase64;  // Contenido codificado en Base64
                            
                            if (xmlCounter == 1) { // Devolver la informacion de la factua Nombre y base64XML
                                fileBase64 = bucleFileBase64;
                                nombreArchivo = bucleNombreArchivo;  
                            }
                            else
                            {
                                // Agregar el archivo (ya sea XML o no) a la lista adjuntos
                                adjuntos.Add(archivoInfo);
                            }

                            // Sacar copia en la ruta destinationPath del XML
                            System.IO.File.Copy(file, destinationFilePath, true);
                        }
                        else
                        {
                            archivoInfo.NombreArchivo = renombrar(fileName);  // Nombre del archivo renombrado

                            string destinationFilePath = Path.Combine(destinationPath, fileName);

                            byte[] fileContentBytes = System.IO.File.ReadAllBytes(file);
                            archivoInfo.Data = Convert.ToBase64String(fileContentBytes);
                            
                            // Agregar el archivo (ya sea XML o no) a la lista adjuntos
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

    private async Task<(string fileBase64, string nombreArchivo, List<Adjunto>)> buildInformationExactusAsync(string sourcePath,
            string filePrefix, List<Adjunto> adjuntos, string rucProvSession, string subcarpeta, bool nota_credito = false)
    {
        string fileBase64 = "";
        string nombreArchivo = "";
        string bucleFileBase64 = "";
        string bucleNombreArchivo = "";

        try
        {
            await Task.Run(() =>
            {
                // Código de ProcesarArchivos 
                IEnumerable<string> files;

                if (nota_credito)
                {
                    files = Directory.EnumerateFiles(sourcePath, $"{filePrefix}__5__*.*")
                        .Concat(Directory.EnumerateFiles(sourcePath, $"{filePrefix}__6__*.*"))
                        .ToList();
                }
                else
                {
                    Console.WriteLine($"Ruta accedida: '{sourcePath}'");
                    Console.WriteLine($"Existe directorio? {Directory.Exists(sourcePath)}");

                    //files = Directory.EnumerateFiles(@"\\192.168.10.116\\INKATESTPROVEEDOR", $"{filePrefix}"); // localmente
                    files = Directory.EnumerateFiles(sourcePath, $"{filePrefix}*.*"); //produccion
                }

                int xmlCounter = 0; // Contador para archivos XML

                foreach (var file in files)
                {
                    string fileName = Path.GetFileName(file);

                    if (fileName.Contains(filePrefix))
                    {
                        var archivoInfo = new Adjunto { };
                        if (Path.GetExtension(file).Equals(".xml", StringComparison.OrdinalIgnoreCase))
                        {
                            // Incrementamos el contador para archivos XML
                            xmlCounter++;

                            // Crear un nuevo nombre único para el XML
                            string newFileName = $"{rucProvSession}-{subcarpeta}-{xmlCounter}.xml";
                            // string destinationFilePath = Path.Combine(destinationPath, newFileName);
                            bucleNombreArchivo = newFileName;

                            byte[] fileContentBytes = System.IO.File.ReadAllBytes(file);
                            bucleFileBase64 = Convert.ToBase64String(fileContentBytes);

                            // Agregar información del XML al objeto archivoInfo
                            archivoInfo.NombreArchivo = bucleNombreArchivo;  // Nombre del archivo XML
                            archivoInfo.Data = bucleFileBase64;  // Contenido codificado en Base64

                            if (xmlCounter == 1)
                            { // Devolver la informacion de la factua Nombre y base64XML
                                fileBase64 = bucleFileBase64;
                                nombreArchivo = bucleNombreArchivo;
                            }
                            else
                            {
                                // Agregar el archivo (ya sea XML o no) a la lista adjuntos
                                adjuntos.Add(archivoInfo);
                            }
                        }
                        else
                        {
                            archivoInfo.NombreArchivo = renombrar(fileName);  // Nombre del archivo renombrado
                            byte[] fileContentBytes = System.IO.File.ReadAllBytes(file);
                            archivoInfo.Data = Convert.ToBase64String(fileContentBytes);
                            adjuntos.Add(archivoInfo);
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
        return (fileBase64, nombreArchivo, adjuntos);
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
        // 8 CDR

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
                    documento = "DOCUMENTO__PDF";
                    break;
                case "2":
                    documento = "DOCUMENTO__XML";
                    break;
                case "3":
                    documento = "GUIA__PDF";
                    break;
                case "4":
                    documento = "OCP__DF";
                    break;
                case "8":
                    documento = "CDR__DF";
                    break;
                default:
                    documento = "SUSTENTO__DF";
                    break;
                // Agrega más casos según sea necesario para otros valores de tipoRequisito
            }

            // Crear el nuevo nombre con los valores actualizados
            nombreAsignado = $"{ordenCompra}__{embarque}__{documento}__{tipoArchivo}";
        }

        return nombreAsignado;
    }


}