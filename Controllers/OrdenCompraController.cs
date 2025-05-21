using Microsoft.AspNetCore.Mvc;
using ProveedorApi.Models.ContentBody;
using ProveedorApi.Data;
using ProveedorApi.Helpers;
using ProveedorApi.Services;
using Microsoft.AspNetCore.Authorization;

namespace ProveedorApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdenCompraController : _BaseController
{
    public OrdenCompraController(ProveedorContext context) : base(context) { }

    [HttpGet("all/{estado}/{filtrosadd}/{provrs}/{fecha_inicio}/{fecha_fin}/{fecha_inicio_doc}/{fecha_fin_doc}")]
    public async Task<ActionResult<object>> GetItems(int estado, int filtrosadd, string? provrs, string fecha_inicio, string fecha_fin, 
                                                    string fecha_inicio_doc, string fecha_fin_doc)
    {
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        string rucProvSession = User.Claims.ToList()[0].Value;
        try
        {
            DateTime fi = DateTime.Parse(fecha_inicio);
            DateTime ff = DateTime.Parse(fecha_fin);
            DateTime fid = DateTime.Parse(fecha_inicio_doc);
            DateTime ffd = DateTime.Parse(fecha_fin_doc);
            string rucProv = roleSession == 2 ? rucProvSession : "";
            string _provrs = roleSession == 2 ? "" : provrs ?? "".ToUpper();

            // var result = await new OrdenCompraService(_context).GetAllAsync(fi, ff, estado, _provrs, rucProv);
            var result = await new OrdenCompraService(_context).GetAllSPAsync(fi, ff, estado, filtrosadd, 
                fid, ffd,_provrs, rucProv);
            return result;
        }
        catch (System.Exception)
        {
            return NotFound();
        }
    }
}