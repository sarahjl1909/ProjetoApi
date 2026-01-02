using Microsoft.AspNetCore.Mvc;
using ProjetoApi.Services;
using ProjetoApi.Dtos;
using System.Threading.Tasks;

namespace ProjetoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerDto)
        {
            try {
                var newUser = await _userService.CreateUserAsync(registerDto);
                return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
            } catch (System.Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userService.ValidateUserCredentialsAsync(loginDto.UserName, loginDto.Password);
            if (user == null) return Unauthorized(new { message = "Credenciais inválidas." });
            return Ok(new { message = "Login realizado com sucesso!", user.UserName });
        }

        [HttpPost("roles")]
        public async Task<IActionResult> CreateRole([FromBody] string roleName)
        {
            var role = await _userService.CreateRoleAsync(roleName);
            return Ok(role);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return user == null ? NotFound() : Ok(user);
        }
    }
}