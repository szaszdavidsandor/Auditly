using Application.DTO.Request;
using Application.DTO.Response;
using Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("/GetAllUsers")]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("/GetUserById/{id:int}")]
        public async Task<ActionResult<UserResponse>> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost("/CreateUser")]
        public async Task<ActionResult<UserResponse>> CreateUser([FromBody] CreateUserRequest requestDto)
        {
            var createdUser = await _userService.CreateUserAsync(requestDto);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }

        [HttpPut("/UpdateUser/{id:int}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest requestDto)
        {
            var result = await _userService.UpdateUserAsync(id, requestDto);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("/SoftDeleteUser/{id:int}")]
        public async Task<IActionResult> SoftDeleteUser(int id)
        {
            var result = await _userService.SoftDeleteUserAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}