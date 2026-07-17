using StoreManagementSystem.API.DTOs;

namespace StoreManagementSystem.API.Interfaces
{
    public interface IAuthService
    {
        Task<UserLoginResponseDTO?> RegisterAsync(UserRegisterDTO dto);
        Task<UserLoginResponseDTO?> LoginAsync(UserLoginDTO dto);
        Task<bool> ConfirmEmailAsync(string token);
        Task<bool> ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(ResetPasswordDTO dto);
        Task<bool> ChangePasswordAsync(int userId, ChangePasswordDTO dto);
    }
}
