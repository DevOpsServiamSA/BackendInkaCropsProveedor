using Microsoft.EntityFrameworkCore;
using ProveedorApi.Data;
using ProveedorApi.Helpers;
using ProveedorApi.Models;

namespace ProveedorApi.Services;

public class TutorialService : _BaseService
{
    public TutorialService(ProveedorContext context) : base(context) { }

    public async Task<object> GetVideos(int perfil)
    {
        try
        {
            var result = await new JsonResultHelper(_context)
            .ToJsonResultAsync<object>($"exec oc_get_tutorial_videos {perfil}");
            return result ?? new object[] { };
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }

        public async Task<object> GetApps()
    {
        try
        {
            var result = await new JsonResultHelper(_context)
            .ToJsonResultAsync<object>($"exec oc_get_apps");
            return result ?? new object[] { };
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }
}