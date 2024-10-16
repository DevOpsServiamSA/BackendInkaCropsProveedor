using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProveedorApi.Data;
using ProveedorApi.Helpers;

namespace ProveedorApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly ProveedorContext _context;
    public TestController(ProveedorContext context)
    {
        _context = context;
    }
    [HttpGet("msg")]
    public ActionResult<object> GetMsg()
    {
        try
        {
            return new { msg = "Message ProveedorApi" };
        }
        catch (Exception e)
        {
            return new { e.Message };
        }
    }
    [HttpGet("values")]
    public async Task<ActionResult<object>> GetValuesAsync()
    {
        try
        {
            string idConn = Request.HttpContext.Connection.Id.ToString();
            string plataforma = System.Environment.OSVersion.Platform.ToString();

            var estados = await _context.Estado.Select(x => x).FirstOrDefaultAsync();
            return new { idConn, plataforma, estados };
        }
        catch (Exception e)
        {
            return new { e.Message };
        }
    }
}