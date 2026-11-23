using Application.DTO.Request;
using Application.DTO.Response;
using Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("GetUserById/{id:int}")]
        public async Task<ActionResult<UserResponse>> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            return Ok(user);
        }

        [HttpPost("CreateUser")]
        public async Task<ActionResult<UserResponse>> CreateUser([FromBody] CreateUserRequest requestDto)
        {
            if(requestDto is null)
            {

            }

            var createdUser = await _userService.CreateUserAsync(requestDto);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }

        [HttpPut("UpdateUser/{id:int}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest requestDto)
        {
            var result = await _userService.UpdateUserAsync(id, requestDto);

            return NoContent();
        }

        [HttpDelete("SoftDeleteUser/{id:int}")]
        public async Task<IActionResult> SoftDeleteUser(int id)
        {
            var result = await _userService.SoftDeleteUserAsync(id);

            return NoContent();
        }
    }
}