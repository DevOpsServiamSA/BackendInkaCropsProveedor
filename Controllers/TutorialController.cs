using Microsoft.AspNetCore.Mvc;
using ProveedorApi.Data;
using ProveedorApi.Services;

namespace ProveedorApi.Controllers;

[Route("api/[controller]")]
[ApiController]

public class TutorialController : _BaseController
{
    public TutorialController(ProveedorContext context) : base(context) { }
    [HttpGet("videos")]
    public async Task<ActionResult<object>> GetVideos()
    {
        try
        {
            int perfil = Int32.Parse(User.Claims.ToList()[4].Value);
            var result = await new TutorialService(_context).GetVideos(perfil);
            return result;
        }
        catch (System.Exception)
        {
            return new Object[] { };
        }
    }

    [HttpGet("video/{p_perfil}/{p_id}")]
    public IActionResult GetVideo(int p_perfil, string p_id)
    {
        try
        {
            byte[] b64Id = Convert.FromBase64String(p_id);
            int id = 0;
            bool statusId = Int32.TryParse(System.Text.Encoding.UTF8.GetString(b64Id), out id);
            if (!statusId || id == 0) return NotFound();

            int perfil = Int32.Parse(User.Claims.ToList()[4].Value);

            if (perfil == 2 && perfil != p_perfil) return NotFound();

            var tutorialVideo = perfil == 1 ? _context.TutorialVideo.Where(x => x.id == id).FirstOrDefault() :
            _context.TutorialVideo.Where(x => x.id == id && x.perfil_id == perfil).FirstOrDefault();

            if (tutorialVideo == null) return NotFound();

            string _path = Path.Combine(AppConfig.Configuracion.CarpetaArchivos, "videos", tutorialVideo.nombre);
            if (!System.IO.File.Exists(_path)) return NotFound();

            var filestream = System.IO.File.OpenRead(_path);
            return File(filestream, contentType: "video/mp4", fileDownloadName: tutorialVideo.nombre, enableRangeProcessing: true);
        }
        catch (System.Exception)
        {
            return NotFound();
        }
    }

    [HttpGet("apps")]
    public async Task<ActionResult<object>> GetApps()
    {
        try
        {
            var result = await new TutorialService(_context).GetApps();
            return result;
        }
        catch (System.Exception)
        {
            return new Object[] { };
        }
    }

    [HttpGet("apps/link/{nombre}")]
    public IActionResult GetAppLink(string nombre)
    {
        try
        {
            string _path = Path.Combine(AppConfig.Configuracion.CarpetaArchivos, "apps", nombre);
            if (!System.IO.File.Exists(_path)) return NotFound();

            var filestream = System.IO.File.OpenRead(_path);
            return File(filestream, contentType: "application/apk", fileDownloadName: nombre, enableRangeProcessing: true);
        }
        catch (System.Exception)
        {
            return NotFound();
        }
    }
}