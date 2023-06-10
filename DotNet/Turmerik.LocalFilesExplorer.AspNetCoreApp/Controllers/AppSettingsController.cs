using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Turmerik.AspNetCore.Infrastucture;
using Turmerik.Infrastucture;
using Turmerik.LocalFilesExplorer.AspNetCoreApp.Data;

namespace Turmerik.LocalFilesExplorer.AspNetCoreApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AppSettingsController : ControllerBase
    {
        [HttpGet]
        public IClientAppSettings Get()
        {
            return new ClientAppSettingsMtbl
            {
                TrmrkPrefix = TurmerikPrefixes.TRMRK
            };
        }
    }
}
