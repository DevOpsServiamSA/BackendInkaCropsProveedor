using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProveedorApi.Data;

namespace ProveedorApi.Controllers;
[Authorize]
public class _BaseController : ControllerBase
{
    protected readonly ProveedorContext _context = null!;
    protected readonly TransporteContext _contextt = null!;
    protected readonly ExactusExtContext _contexte = null!;
    public _BaseController(ProveedorContext context) => _context = context;
    public _BaseController(TransporteContext context) => _contextt = context;
    public _BaseController(ExactusExtContext context) => _contexte = context;
}