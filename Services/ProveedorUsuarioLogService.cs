using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ProveedorApi.Data;
using ProveedorApi.Helpers;
using ProveedorApi.Models;

namespace ProveedorApi.Services;

public class ProveedorUsuarioLogService : _BaseService
{
    public ProveedorUsuarioLogService(ProveedorContext context) : base(context) { }
    public async Task<int> New(string p_ruc, string p_username)
    {
        try
        {
            _context.ProveedorUsuarioLog.Add(new ProveedorUsuarioLog()
            {
                id = 0,
                fecha = DateTime.Now,
                ruc = p_ruc,
                username = p_username,
                audfecha = DateTime.Now,
            });
            return await _context.SaveChangesAsync();
        }
        catch (System.Exception)
        {
            return 0;
        }
    }
}