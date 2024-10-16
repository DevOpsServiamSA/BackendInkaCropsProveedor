using Microsoft.AspNetCore.Mvc;
using ProveedorApi.Data;
using ProveedorApi.Services;

namespace ProveedorApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ParametroController : _BaseController
{
    public ParametroController(ProveedorContext context) : base(context) { }

    [HttpGet("all")]
    public async Task<ActionResult<object>> GetItems()
    {
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        string rucProvSession = User.Claims.ToList()[0].Value;
        try
        {
            if (roleSession == 2) return new Object[] { };
            var result = await new ParametroService(_context).GetAllAsync();
            return result;
        }
        catch (System.Exception)
        {
            return NotFound();
        }
    }
}