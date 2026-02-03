using CSTS.Api.Dtos;
using CSTS.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CSTS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("admins")]
        public async Task<IActionResult> GetAdminUsers()
        {
            var adminUsers = await _userService.GetAdminUsersAsync();
            return Ok(adminUsers);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequest loginRequest) // Return type changed
        {
            if (string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Username and password are required.");
            }

            var loginResponse = await _userService.LoginAsync(loginRequest.Username, loginRequest.Password);

            if (loginResponse == null)
            {
                return Unauthorized("Invalid credentials."); // More specific message
            }

            return Ok(loginResponse);
        }
    }
}
