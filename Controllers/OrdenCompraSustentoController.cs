using Microsoft.AspNetCore.Mvc;
using ProveedorApi.Models.ContentBody;
using ProveedorApi.Data;
using ProveedorApi.Helpers;
using ProveedorApi.Models;
using ProveedorApi.Services;

namespace ProveedorApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdenCompraSustentoController : _BaseController
{
    public OrdenCompraSustentoController(ProveedorContext context) : base(context) { }

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

            var result = await new OrdenCompraSustentoService(_context).GetAllAsync(p_orden_compra, p_embarque);
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

        var dict = Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString());

        try
        {
            p_ruc = dict["ruc"];
            p_orden_compra = dict["orden_compra"];
            p_embarque = dict["embarque"];
        }
        catch (Exception)
        {
            return Conflict(new { msg = "Parámetros incompletos" });
        }

        if (string.IsNullOrEmpty(p_ruc) || string.IsNullOrEmpty(p_orden_compra) || string.IsNullOrEmpty(p_embarque))
        {
            return Conflict(new { msg = "Parámetros vacíos" });
        }


        if (roleSession == 2 && rucProvSession != p_ruc)
        {
            return Conflict(new { msg = "No tiene permiso para hacer esto" });
        }

        if (!Request.Form.Files.Any()) return Conflict(new { msg = "No hay archivos que subir" });

        var _file = Request.Form.Files.First();
        var _filename_original = _file.FileName;

        Tuple<int, int, string> saveInt = await new OrdenCompraSustentoService(_context).SaveAsync(p_orden_compra, p_embarque, _filename_original, userNameSession);
        if (saveInt.Item1 == 0) return Conflict(new { msg = "No hubo registro del archivo" });

        var _path = Path.Combine(AppConfig.Configuracion.CarpetaArchivos, p_ruc);
        bool status = await UtilityHelper.UploadFormFileAsync(_path, saveInt.Item3, _file);
        if (!status)
        {
            await new OrdenCompraSustentoService(_context).DeleteTotalAsync(p_orden_compra, p_embarque, saveInt.Item2, userNameSession);
            return Conflict(new { msg = "No se pudo guardar el archivo" });
        }

        return NoContent();
    }
    [HttpGet("fileDownload/{p_ruc}/{p_orden_compra}/{p_embarque}/{filename}")]
    public async Task<ActionResult<object>> FileDownload(string p_ruc, string p_orden_compra, string p_embarque, string filename)
    {
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        string rucProvSession = User.Claims.ToList()[0].Value;
        try
        {
            if (roleSession == 2 && rucProvSession != p_ruc)
            {
                return NotFound();
            }

            string _file = Path.Combine(AppConfig.Configuracion.CarpetaArchivos, p_ruc, filename ?? "");
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

    [HttpPut("deleteItem")]
    public async Task<ActionResult> DeleteItem(DeleteItemSustentoBody body)
    {
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        string userNameSession = User.Claims.ToList()[2].Value;        
        string rucProvSession = User.Claims.ToList()[0].Value;

        if (roleSession == 2 && rucProvSession != body.p_ruc)
        {
            return Conflict(new { msg = "No tiene permiso para hacer esto" });
        }

        var delInt = await new OrdenCompraSustentoService(_context).DeleteAsync(body.p_orden_compra, body.p_embarque, body.linea, userNameSession);
        if (delInt == 0) return Conflict(new { msg = "No hubo eliminación de registro" });
        return NoContent();
    }
}
