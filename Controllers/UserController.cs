using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoolServer.Services;

namespace PoolServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetUserDetails()
        {
            var userIdentification = User.Identity.Name;
            if( userIdentification == null )return BadRequest("No user id provided");
            
            var user = await _userService.GetUserDetails(userIdentification);

            if( user == null ) return BadRequest("Failed to obtain user details");

            user.Password = null;

            return Ok(user);
        }

    }
}