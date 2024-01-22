using Microsoft.AspNetCore.Mvc;
using ProveedorApi.Models.ContentBody;
using ProveedorApi.Data;
using ProveedorApi.Helpers;
using ProveedorApi.Services;

namespace ProveedorApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransporteController : _BaseController
{
    public TransporteController(TransporteContext context) : base(context) { }

    [HttpGet("segall/{fecha_solicitada_ini}/{fecha_solicitada_fin}/{fecha_requerida_ini}/{fecha_requerida_fin}/{fecha_programada_ini}/{fecha_programada_fin}/{fecha_entregada_ini}/{fecha_entregada_fin}/{turno}/{estado}/{tipo_programacion}")]
    public async Task<ActionResult<object>> GetSegAll(string? fecha_solicitada_ini, string? fecha_solicitada_fin, string? fecha_requerida_ini, string? fecha_requerida_fin, string? fecha_programada_ini,
        string? fecha_programada_fin, string? fecha_entregada_ini, string? fecha_entregada_fin, byte turno, byte estado, byte tipo_programacion)
    {
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        string rucProvSession = User.Claims.ToList()[0].Value;
        try
        {
            // DateTime fi = DateTime.Parse(fecha_inicio);
            // DateTime ff = DateTime.Parse(fecha_fin);
            // string rucProv = roleSession == 2 ? rucProvSession : "";
            // string _provrs = roleSession == 2 ? "" : provrs ?? "".ToUpper();
            fecha_solicitada_ini = fecha_solicitada_ini ?? "";
            fecha_solicitada_fin = fecha_solicitada_fin ?? "";
            fecha_requerida_ini = fecha_requerida_ini ?? "";
            fecha_requerida_fin = fecha_requerida_fin ?? "";
            fecha_programada_ini = fecha_programada_ini ?? "";
            fecha_programada_fin = fecha_programada_fin ?? "";
            fecha_entregada_ini = fecha_entregada_ini ?? "";
            fecha_entregada_fin = fecha_entregada_fin ?? "";

            var result = await new TransporteService(_contextt).GetSegAllSPAsync(fecha_solicitada_ini, fecha_solicitada_fin, fecha_requerida_ini, fecha_requerida_fin, fecha_programada_ini, fecha_programada_fin, fecha_entregada_ini, fecha_entregada_fin, turno, estado, tipo_programacion);
            return result;
        }
        catch (System.Exception)
        {
            return NotFound();
        }
    }
}