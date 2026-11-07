using HomeLocalStorage.Core.Abstractions;
using HomeLocalStorage.Core.DTO;
using HomeLocalStorage.Services.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace HomeLocalStorage.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController(AccountService accountService) : ControllerBase
    {

        [HttpPost("register")]

        public async Task<ActionResult<SharedDTO>> Register([FromBody] CreateUserDTO u)
        {
            var (id, error) = await accountService.Register(u.Login, u.Name, u.Password, u.SecPassword);
            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(new SharedDTO(id, error));
            }
            return Ok(new SharedDTO(id, error));
        }
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginUserRequest u)
        {
            var token = await accountService.Login(u.Login, u.Password);
            if (string.IsNullOrEmpty(token))
                return Unauthorized();
            return Ok(token);
        }

        /*private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPut("register")]
        public async Task<ActionResult<SharedDTO>> Register([FromBody] CreateUserDTO uDTO)
        {
            var (id, error) = await _userService.CreateUser(uDTO);
            if (string.IsNullOrEmpty(error))
            {
                return BadRequest((id, error));
            }
            return Ok(new SharedDTO(id, error));

        }

        [HttpPost("login")]
        public async Task<ActionResult<SharedDTO>> Login([FromBody] LoginUserRequest uDTO)
        {
            if (string.IsNullOrEmpty(uDTO.Login))
            {
                return BadRequest((Guid.Empty, "Login can not be empty!"));
            }
            if (string.IsNullOrEmpty(uDTO.Password))
            {
                return BadRequest((Guid.Empty, "Password can not be empty!"));
            }
            var (id, error) = await _userService.UserLogin(uDTO.Login, uDTO.Password);
            return Ok(new SharedDTO(id, error));
        }
*/
    }
}
