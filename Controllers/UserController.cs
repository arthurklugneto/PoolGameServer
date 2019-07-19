using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoolGameServer.Facades;
using PoolServer.Services;

namespace PoolServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly IUserFacade _userFacade;

        public UserController(IUserService userService,IUserFacade userFacade)
        {
            _userService = userService;
            _userFacade = userFacade;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetUserDetails()
        {
            var userIdentification = User.Identity.Name;
            if( userIdentification == null )return BadRequest("No user id provided");
            
            var user = await _userFacade.GetUserDetails(userIdentification);

            if( user == null ) return BadRequest("Failed to obtain user details");

            user.Password = null;

            return Ok(user);
        }

        [HttpPost]
        [Route("/addFriend")]
        public async Task<IActionResult> AddFriend(){
            
        }

    }
}