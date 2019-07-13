using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PoolServer.Configurations;
using PoolServer.Models;

namespace PoolServer.Controllers
{
    [ApiController]
    [Route("/")]
    public class ServerStatusApi : ControllerBase
    {

        private readonly IOptions<ServerSettings> _serverSettingsConfiguration;

        public ServerStatusApi(IOptions<ServerSettings> serverSettingsConfiguration)
        {
            _serverSettingsConfiguration = serverSettingsConfiguration;
        }

        public IActionResult GetServerStatus()
        {
            return Ok(new ServerStatusResponse(){
                Server = _serverSettingsConfiguration.Value.ServerName,
                Version = _serverSettingsConfiguration.Value.Version,
                Operational = true
            });
        }
        
    }
}