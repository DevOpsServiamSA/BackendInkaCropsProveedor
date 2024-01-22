using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProveedorApi.Models.ContentBody;
using ProveedorApi.Data;
using ProveedorApi.Helpers;
using ProveedorApi.Services;

namespace ProveedorApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProveedorController : _BaseController
{
    public ProveedorController(ProveedorContext context) : base(context) { }

    [HttpGet("all")]
    public async Task<ActionResult<object>> GetAll()
    {
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        string rucProvSession = User.Claims.ToList()[0].Value;
        try
        {
            string? ruc = roleSession == 2 ? rucProvSession : null;
            // if (roleSession == 2) return Conflict(new { msg = "No tiene permiso para realizar esta acción" });
            var result = await new ProveedorService(_context).GetAllAsync(ruc);
            return result;
        }
        catch (System.Exception)
        {
            return NotFound();
        }
    }

}