using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Turmerik.AspNetCore.Controllers;
using Turmerik.AspNetCore.Infrastucture;
using Turmerik.Infrastucture;
using Turmerik.LocalFilesExplorer.AspNetCoreApp.Data;
using Turmerik.LocalFilesExplorer.AspNetCoreApp.Services;

namespace Turmerik.LocalFilesExplorer.AspNetCoreApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AppSettingsController : TrmrkControllerBase
    {
        private readonly IClientAppSettingsService clientAppSettingsService;

        public AppSettingsController(
            IClientAppSettingsService clientAppSettingsService)
        {
            this.clientAppSettingsService = clientAppSettingsService ?? throw new ArgumentNullException(nameof(clientAppSettingsService));
        }

        [HttpGet]
        public ClientAppSettings.IClnbl Get() => clientAppSettingsService.ClientAppSettings;
    }
}
