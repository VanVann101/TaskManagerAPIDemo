using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Common.DTO;
using TaskManager.Application.Contracts;

namespace TaskManager.WebAPI.Controllers {
    [Route("api/users")]
    [ApiController]
    public class UserController : Controller {
        private readonly IUserService _userService;
        public UserController(IUserService userService) {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id) {
            var result = await _userService.GetUserByIdAsync(id);

            if (!result.IsSuccess || result.Data is null)
                return NotFound();

            return Ok(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers() {
            var result = await _userService.GetAllUsersAsync();
            return result.IsSuccess ? Ok(result.Data) : NotFound();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDto userDto) {
            var result = await _userService.CreateUserAsync(userDto);

            if (!result.IsSuccess) {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.AuthenticateAsync(loginDto.Email, loginDto.Password);
            if (!result.IsSuccess)
                return Unauthorized();

            return Ok();
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDto userDto) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.UpdateUserAsync(id, userDto);
            if (!result.IsSuccess)
                return NotFound();

            return Ok();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id) {
            var result = await _userService.DeleteUserAsync(id);
            if (!result.IsSuccess)
                return NotFound();

            return Ok();
        }

        public class LoginDto() { 
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
}
