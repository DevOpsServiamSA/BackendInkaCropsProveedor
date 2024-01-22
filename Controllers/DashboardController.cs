using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProveedorApi.Models.ContentBody;
using ProveedorApi.Data;
using ProveedorApi.Helpers;
using ProveedorApi.Services;

namespace ProveedorApi.Controllers;

[Route("api/[controller]")]
[ApiController]

public class DashboardController : _BaseController
{
    public DashboardController(ProveedorContext context) : base(context) { }
    [HttpGet("proveedores")]
    public async Task<ActionResult<object>> GetProveedores()
    {
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        if (roleSession == 2) return new Object[] { };
        try
        {
            var result = await new DashboardService(_context).GetProveedores();
            return result;
        }
        catch (System.Exception)
        {
            return new Object[] { };
        }
    }

    [HttpGet("facturasResumen/{ruc_prov}/{fecha_inicio}/{fecha_fin}")]
    public async Task<ActionResult<object>> GetFacturasResumen(string? ruc_prov, string fecha_inicio, string fecha_fin)
    {
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        if (roleSession == 2) return new Object[] { };
        try
        {
            DateTime fi = DateTime.Parse(fecha_inicio);
            DateTime ff = DateTime.Parse(fecha_fin);
            var result = await new DashboardService(_context).GetFacturasResumen(ruc_prov ?? "", fi.ToString("dd/MM/yyyy"), ff.ToString("dd/MM/yyyy"));
            return result;
        }
        catch (System.Exception)
        {
            return new Object[] { };
        }
    }

    [HttpGet("facturasPorEstado/{ruc_prov}/{fecha_inicio}/{fecha_fin}")]
    public async Task<ActionResult<object>> GetFacturasPorEstado(string? ruc_prov, string fecha_inicio, string fecha_fin)
    {
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        if (roleSession == 2) return new Object[] { };
        try
        {
            DateTime fi = DateTime.Parse(fecha_inicio);
            DateTime ff = DateTime.Parse(fecha_fin);
            var result = await new DashboardService(_context).GetFacturasPorEstado(ruc_prov ?? "", fi.ToString("dd/MM/yyyy"), ff.ToString("dd/MM/yyyy"));
            return result;
        }
        catch (System.Exception)
        {
            return new Object[] { };
        }
    }

    [HttpGet("cumplimientoFechaProgramadaDePago/{ruc_prov}/{fecha_inicio}/{fecha_fin}")]
    public async Task<ActionResult<object>> GetCumplimientoFechaProgramadaDePago(string? ruc_prov, string fecha_inicio, string fecha_fin)
    {
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        if (roleSession == 2) return new Object[] { };
        try
        {
            DateTime fi = DateTime.Parse(fecha_inicio);
            DateTime ff = DateTime.Parse(fecha_fin);
            var result = await new DashboardService(_context).GetCumplimientoFechaProgramadaDePago(ruc_prov ?? "", fi.ToString("dd/MM/yyyy"), ff.ToString("dd/MM/yyyy"));
            return result;
        }
        catch (System.Exception)
        {
            return new Object[] { };
        }
    }

    [HttpGet("facturasConRequisitosRechazados/{ruc_prov}/{fecha_inicio}/{fecha_fin}")]
    public async Task<ActionResult<object>> GetFacturasConRequisitosRechazados(string? ruc_prov, string fecha_inicio, string fecha_fin)
    {
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        if (roleSession == 2) return new Object[] { };
        try
        {
            DateTime fi = DateTime.Parse(fecha_inicio);
            DateTime ff = DateTime.Parse(fecha_fin);
            var result = await new DashboardService(_context).GetFacturasConRequisitosRechazados(ruc_prov ?? "", fi.ToString("dd/MM/yyyy"), ff.ToString("dd/MM/yyyy"));
            return result;
        }
        catch (System.Exception)
        {
            return new Object[] { };
        }
    }

    [HttpGet("pagosATiempo/{ruc_prov}/{fecha_inicio}/{fecha_fin}")]
    public async Task<ActionResult<object>> GetpagosATiempo(string? ruc_prov, string fecha_inicio, string fecha_fin)
    {
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        if (roleSession == 2) return new Object[] { };
        try
        {
            DateTime fi = DateTime.Parse(fecha_inicio);
            DateTime ff = DateTime.Parse(fecha_fin);
            var result = await new DashboardService(_context).GetpagosATiempo(ruc_prov ?? "", fi.ToString("dd/MM/yyyy"), ff.ToString("dd/MM/yyyy"));
            return result;
        }
        catch (System.Exception)
        {
            return new Object[] { };
        }
    }

    [HttpGet("facturasPagadasPorCondicionDePago/{ruc_prov}/{fecha_inicio}/{fecha_fin}")]
    public async Task<ActionResult<object>> GetFacturasPagadasPorCondicionDePago(string? ruc_prov, string fecha_inicio, string fecha_fin)
    {
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        if (roleSession == 2) return new Object[] { };
        try
        {
            DateTime fi = DateTime.Parse(fecha_inicio);
            DateTime ff = DateTime.Parse(fecha_fin);
            var result = await new DashboardService(_context).GetFacturasPagadasPorCondicionDePago(ruc_prov ?? "", fi.ToString("dd/MM/yyyy"), ff.ToString("dd/MM/yyyy"));
            return result;
        }
        catch (System.Exception)
        {
            return new Object[] { };
        }
    }

    [HttpGet("facturasPagadasPorProveedor/{ruc_prov}/{fecha_inicio}/{fecha_fin}")]
    public async Task<ActionResult<object>> GetFacturasPagadasPorProveedor(string? ruc_prov, string fecha_inicio, string fecha_fin)
    {
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        if (roleSession == 2) return new Object[] { };
        try
        {
            DateTime fi = DateTime.Parse(fecha_inicio);
            DateTime ff = DateTime.Parse(fecha_fin);
            var result = await new DashboardService(_context).GetFacturasPagadasPorProveedor(ruc_prov ?? "", fi.ToString("dd/MM/yyyy"), ff.ToString("dd/MM/yyyy"));
            return result;
        }
        catch (System.Exception)
        {
            return new Object[] { };
        }
    }    
}