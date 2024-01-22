using Microsoft.AspNetCore.Authorization;
using ProveedorApi.Data;

namespace ProveedorApi.Services;
[Authorize]
public class _BaseService
{
    protected readonly ProveedorContext _context = null!;
    protected readonly TransporteContext _contextt = null!;
    protected readonly ExactusExtContext _contexte = null!;
    public _BaseService(ProveedorContext context) => _context = context;
    public _BaseService(TransporteContext context) => _contextt = context;
    public _BaseService(ExactusExtContext context) => _contexte = context;
}