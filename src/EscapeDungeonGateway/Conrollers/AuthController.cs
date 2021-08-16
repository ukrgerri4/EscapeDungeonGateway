using System.Linq;
using EscapeDungeonGateway.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace EscapeDungeonGateway.Conrollers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService tokenService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthController(ITokenService tokenService, IHttpContextAccessor httpContextAccessor)
        {
            this.tokenService = tokenService;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Token(string login, string password)
        {
            var tokenResult = await tokenService.GetEscapeDungeonTokenAsync(login, password);
            return tokenResult.IsSuccess ? Ok(tokenResult.Value) : BadRequest(tokenResult.Error);
        }
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Introspect()
        {
            httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues authValues);
            var token = authValues.FirstOrDefault()?.Replace("Bearer ", "");
            var introspectResult = await tokenService.IntrospectEscapeDungeonAsync(token);
            return introspectResult.IsSuccess ? Ok(introspectResult.Value) : BadRequest(introspectResult.Error);
        }
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UserInfo()
        {
            httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues authValues);
            var token = authValues.FirstOrDefault()?.Replace("Bearer ", "");
            var userInfoResult = await tokenService.UserInfoAsync(token);
            return userInfoResult.IsSuccess ? Ok(userInfoResult.Value) : BadRequest(userInfoResult.Error);
        }
    }
}
