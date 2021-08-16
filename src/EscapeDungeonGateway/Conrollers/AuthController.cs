using EscapeDungeonGateway.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EscapeDungeonGateway.Conrollers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService tokenService;

        public AuthController(ITokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Token(string login, string password)
        {
            var tokenResult = await tokenService.GetEscapeDungeonTokenAsync(login, password);
            return tokenResult.IsSuccess ? Ok(tokenResult.Value) : BadRequest(tokenResult.Error);
        }
    }
}
