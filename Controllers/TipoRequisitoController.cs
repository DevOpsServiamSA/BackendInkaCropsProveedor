using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProveedorApi.Data;
using ProveedorApi.Helpers;

namespace ProveedorApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TipoRequisitoController : _BaseController
{
    public TipoRequisitoController(ProveedorContext context) : base(context) { }
}