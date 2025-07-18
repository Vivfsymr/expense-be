using Microsoft.AspNetCore.Mvc;
using ExpenseBe.Core.Models;
using ExpenseBe.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using ExpenseBe.API.DTOs;

namespace ExpenseBe.API.Controllers
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

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<User>>>> GetAll()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(new ApiResponse<IEnumerable<User>>
                {
                    Success = true,
                    Message = "Users fetched successfully",
                    Data = users
                });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse<IEnumerable<User>>
                {
                    Success = false,
                    Message = e.Message,
                    Data = new List<User>()
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<User>>> GetById(string id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                    return NotFound();

                return Ok(new ApiResponse<User>
                {
                    Success = true,
                    Message = "User fetched successfully",
                    Data = user
                });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse<User>
                {
                    Success = false,
                    Message = e.Message,
                    Data = null
                });
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<User>>> Register(User user)
        {
            try
            {
                var existingUser = await _userService.GetUserByUsernameAsync(user.Username);
                if (existingUser != null)
                    return BadRequest("Username already exists");

                var createdUser = await _userService.CreateUserAsync(user);
                return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse<User>
                {
                    Success = false,
                    Message = e.Message,
                    Data = null
                });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<User>>> Login([FromBody] User loginUser)
        {
            try
            {
                var user = await _userService.LoginAsync(loginUser.Username, loginUser.Password);
                if (user == null)
                    return Unauthorized();
                return Ok(new ApiResponse<User>
                {
                    Success = true,
                    Message = "User logged in successfully",
                    Data = user
                });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse<User>
                {
                    Success = false,
                    Message = e.Message,
                    Data = null
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<User>>> Update(string id, User user)
        {
            try
            {
                var success = await _userService.UpdateUserAsync(id, user);
                if (!success)
                    return NotFound();

                return Ok(new ApiResponse<User>
                {
                    Success = true,
                    Message = "User updated successfully",
                    Data = user
                });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse<User>
                {
                    Success = false,
                    Message = e.Message,
                    Data = null
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<User>>> Delete(string id)
        {
            try
            {
                var success = await _userService.DeleteUserAsync(id);
                if (!success)
                    return NotFound();

                return Ok(new ApiResponse<User>
                {
                    Success = true,
                    Message = "User deleted successfully",
                    Data = null
                });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse<User>
                {
                    Success = false,
                    Message = e.Message,
                    Data = null
                });
            }
        }
    }
}