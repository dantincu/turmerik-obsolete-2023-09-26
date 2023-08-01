using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Turmerik.AspNetCore.Controllers;
using Turmerik.AspNetCore.Infrastucture;
using Turmerik.Infrastucture;
using Turmerik.LocalFilesExplorer.AspNetCoreApp.Data;

namespace Turmerik.LocalFilesExplorer.AspNetCoreApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AppSettingsController : TrmrkControllerBase
    {
        [HttpGet]
        public ClientAppSettings.IClnbl Get()
        {
            return new ClientAppSettings.Mtbl
            {
                TrmrkPrefix = TurmerikPrefixes.TRMRK
            };
        }
    }
}
