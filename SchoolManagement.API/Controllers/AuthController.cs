using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Model.DTOs;
using SchoolManagement.Service.Interfaces;

namespace SchoolManagement.API.Controllers
{
    /// <summary>
    /// Handles User Authentication and JWT Token Generation using Asynchronous Programming.
    /// Demonstrates async/await for secure and efficient authentication processing.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Asynchronously authenticates a user and generates a JWT token.
        /// Demonstrates async/await for secure and non-blocking authentication operations.
        /// </summary>
        /// <param name="loginDto">User login credentials.</param>
        /// <returns>JWT token and authentication response.</returns>
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                var result = await _authService.LoginAsync(loginDto);

                if (result == null)
                {
                    return Unauthorized("Invalid Username or Password");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}