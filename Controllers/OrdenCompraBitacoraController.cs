using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProveedorApi.Data;
using ProveedorApi.Helpers;
using ProveedorApi.Models;
using ProveedorApi.Services;

namespace ProveedorApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdenCompraBitacoraController : _BaseController
{
    public OrdenCompraBitacoraController(ProveedorContext context) : base(context) { }


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
            var result = await new OrdenCompraBitacoraService(_context).GetAllSPAsync(p_ruc, p_orden_compra, p_embarque);
            return result;
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }
}