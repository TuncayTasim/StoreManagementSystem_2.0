using Microsoft.AspNetCore.Mvc;
using StoreManagementSystem.API.DTOs;
using StoreManagementSystem.API.Services;

namespace StoreManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDTO dto)
        {
            try
            {
                var result = await _service.RegisterAsync(dto);
                if (result == null) return BadRequest("User already exists");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO dto)
        {
            try
            {
                var result = await _service.LoginAsync(dto);
                if (result == null) return Unauthorized("Invalid credentials");
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Custom check for specific message handled by frontend
                if (ex.Message == "EMAIL_NOT_CONFIRMED")
                {
                    return BadRequest("EMAIL_NOT_CONFIRMED");
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> Confirm(string token)
        {
            try
            {
                var result = await _service.ConfirmEmailAsync(token);
                if (!result) return BadRequest("Invalid or expired token");
                return Ok("Email confirmed successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO dto)
        {
            try
            {
                var result = await _service.ForgotPasswordAsync(dto.Email);
                if (!result) return BadRequest("Email not found");
                return Ok("Password reset token sent to email");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
        {
            try
            {
                var result = await _service.ResetPasswordAsync(dto);
                if (!result) return BadRequest("Invalid token or passwords do not match");
                return Ok("Password reset successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
