using Microsoft.AspNetCore.Mvc;
using ProveedorApi.Models.ContentBody;
using ProveedorApi.Data;
using ProveedorApi.Helpers;
using ProveedorApi.Services;

namespace ProveedorApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PedidosController : _BaseController
{
    public PedidosController(ProveedorContext context) : base(context) { }

    [HttpGet("all")]
    public async Task<ActionResult<object>> GetItems()
    {
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        string rucProvSession = User.Claims.ToList()[0].Value;
        try
        {
            var result = await new PedidosService(_context).GetAllSPAsync();
            return result;
        }
        catch (System.Exception)
        {
            return NotFound();
        }
    }
}