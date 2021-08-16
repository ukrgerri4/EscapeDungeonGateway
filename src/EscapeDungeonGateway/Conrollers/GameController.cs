using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EscapeDungeonGateway.Conrollers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GameController : ControllerBase
    {
        private readonly ILogger<GameController> logger;
        private readonly IHttpContextAccessor httpContextAccessor;

        public GameController(ILogger<GameController> logger, IHttpContextAccessor httpContextAccessor)
        {
            this.logger = logger;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("free-server")]
        public async Task<IActionResult> GetFreeServer()
        {
            try
            {
                DockerClient client = new DockerClientConfiguration(
                    new Uri("unix:///var/run/docker.sock")
                    ).CreateClient();

                logger.LogWarning($"Kovbanosi. {httpContextAccessor.HttpContext.TraceIdentifier}");

                var containers = await client.Containers.ListContainersAsync(
                    new ContainersListParameters()
                    {
                        Limit = 10,
                    }
                );

                return Ok(containers.Select(x => x.ID).ToArray());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
