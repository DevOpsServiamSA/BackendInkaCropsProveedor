using Microsoft.AspNetCore.Mvc;
using ProveedorApi.Data;

namespace ProveedorApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PublicController : ControllerBase
{
    private readonly ProveedorContext _context;
    public PublicController(ProveedorContext context)
    {
        _context = context;
    }

    [HttpGet("video/{p_name}")]
    public object getVideo(string p_name)
    {
        try
        {
            var tutorialVideo = _context.TutorialVideo.Where(x => x.publico && x.nombre == p_name).FirstOrDefault();
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
}