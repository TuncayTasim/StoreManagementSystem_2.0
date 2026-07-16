using Microsoft.AspNetCore.Mvc;
using StoreManagementSystem.API.DTOs;
using StoreManagementSystem.API.Interfaces;

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

        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO dto)
        {
            try
            {
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                    return Unauthorized("Invalid user token");

                var result = await _service.ChangePasswordAsync(userId, dto);
                if (!result) return BadRequest("Invalid old password or error updating password");
                return Ok("Password changed successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
