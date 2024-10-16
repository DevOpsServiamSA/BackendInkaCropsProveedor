using Microsoft.EntityFrameworkCore;
using ProveedorApi.Data;
using ProveedorApi.Helpers;
using ProveedorApi.Models;

namespace ProveedorApi.Services;

public class DashboardService : _BaseService
{
    public DashboardService(ProveedorContext context) : base(context) { }

    public async Task<object> GetProveedores()
    {
         try
        {
            var result = await new JsonResultHelper(_context)
            .ToJsonResultAsync<object>($"exec oc_get_dashboard_proveedor");
            return result ?? new object[] { };
        }
        catch (System.Exception)
        {
            return new object[] { new { ruc = "%20", razonsocial = "TODOS" } };
        }
    }

    public async Task<object> GetFacturasResumen(string p_prov_ruc, string p_fecha_inicio, string p_fecha_fin)
    {
        try
        {
            var result = await new JsonResultHelper(_context)
            .ToJsonResultAsync<object>($"exec oc_get_dashboard_resumen_ordenes {p_fecha_inicio}, {p_fecha_fin}, {p_prov_ruc}");
            return result ?? new object[] { };
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }

    public async Task<object> GetFacturasPorEstado(string p_prov_ruc, string p_fecha_inicio, string p_fecha_fin)
    {
        try
        {
            var result = await new JsonResultHelper(_context)
            .ToJsonResultAsync<object>($"exec oc_get_dashboard_facturas_estado {p_fecha_inicio}, {p_fecha_fin}, {p_prov_ruc}");
            return result ?? new object[] { };
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }

    public async Task<object> GetCumplimientoFechaProgramadaDePago(string p_prov_ruc, string p_fecha_inicio, string p_fecha_fin)
    {
        try
        {
            var result = await new JsonResultHelper(_context)
            .ToJsonResultAsync<object>($"exec oc_get_dashboard_cumplimiento_fecha {p_fecha_inicio}, {p_fecha_fin}, {p_prov_ruc}");
            return result ?? new object[] { };
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }

    public async Task<object> GetFacturasConRequisitosRechazados(string p_prov_ruc, string p_fecha_inicio, string p_fecha_fin)
    {
        try
        {
            var result = await new JsonResultHelper(_context)
            .ToJsonResultAsync<object>($"exec oc_get_dashboard_requisitos_rechazados {p_fecha_inicio}, {p_fecha_fin}, {p_prov_ruc}");
            return result ?? new object[] { };
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }

    public async Task<object> GetpagosATiempo(string p_prov_ruc, string p_fecha_inicio, string p_fecha_fin)
    {
        try
        {
            var result = await new JsonResultHelper(_context)
            .ToJsonResultAsync<object>($"exec oc_get_dashboard_pagos_tiempo {p_fecha_inicio}, {p_fecha_fin}, {p_prov_ruc}");
            return result ?? new object[] { };
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }

    public async Task<object> GetFacturasPagadasPorCondicionDePago(string p_prov_ruc, string p_fecha_inicio, string p_fecha_fin)
    {
        try
        {
            var result = await new JsonResultHelper(_context)
            .ToJsonResultAsync<object>($"exec oc_get_dashboard_condicion_pago {p_fecha_inicio}, {p_fecha_fin}, {p_prov_ruc}");
            return result ?? new object[] { };
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }

    public async Task<object> GetFacturasPagadasPorProveedor(string p_prov_ruc, string p_fecha_inicio, string p_fecha_fin)
    {
        try
        {
            var result = await new JsonResultHelper(_context)
            // .ToJsonResultAsync<object>($"select oc_get_dashboard_facturas_pagadas_proveedor({p_fecha_inicio}, {p_fecha_fin}, {p_prov_ruc}) as json_output");
            .ToJsonResultAsync<object>($"exec oc_get_dashboard_facturas_pagadas_proveedor {p_fecha_inicio}, {p_fecha_fin}, {p_prov_ruc}");
            return result ?? new object[] { };
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }

    public async Task<object> GetComprasPorArticulos(string p_prov_ruc, string p_fecha_inicio, string p_fecha_fin)
    {
        try
        {
            var result = await new JsonResultHelper(_context)
            .ToJsonResultAsync<object>($"exec oc_get_dashboard_compras_articulos {p_fecha_inicio}, {p_fecha_fin}, {p_prov_ruc}");
            return result ?? new object[] { };
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }

    public async Task<object> GetComprasPorCategoria(string p_prov_ruc, string p_fecha_inicio, string p_fecha_fin)
    {
        try
        {
            var result = await new JsonResultHelper(_context)
            .ToJsonResultAsync<object>($"exec oc_get_dashboard_compras_categoria {p_fecha_inicio}, {p_fecha_fin}, {p_prov_ruc}");
            return result ?? new object[] { };
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }
}