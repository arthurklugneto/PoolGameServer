using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PoolServer.Services;
using PoolServer.Models;
using System.Threading.Tasks;
using PoolGameServer.Entities;

namespace PoolServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]TokenRequest tokenRequest)
        {
            var user = await _userService.Authenticate(tokenRequest);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterRequest registerRequest)
        {            
            var user = await _userService.Register(registerRequest);
            return Ok(user);
        }

    }
}