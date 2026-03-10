using StoreManagementSystem.API.DTOs;

namespace StoreManagementSystem.API.Services
{
    public interface IAuthService
    {
        Task<UserResponseDTO?> RegisterAsync(UserRegisterDTO dto);
        Task<UserResponseDTO?> LoginAsync(UserLoginDTO dto);
        Task<bool> ConfirmEmailAsync(string token);
        Task<bool> ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(ResetPasswordDTO dto);
    }
}
