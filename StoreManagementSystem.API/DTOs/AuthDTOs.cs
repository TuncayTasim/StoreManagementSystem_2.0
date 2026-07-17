namespace StoreManagementSystem.API.DTOs
{
    public record UserRegisterDTO(string UserName, string FirstName, string LastName, string Email, string PhoneNumber, string Password, int RoleId);

    public record UserLoginDTO(string UserName, string Password);

    public record UserLoginResponseDTO(int UserId, string UserName, string Token, int RoleId);

    public record ForgotPasswordDTO(string Email);

    public record ResetPasswordDTO(string Token, string NewPassword, string ConfirmPassword);

    public record ChangePasswordDTO(string OldPassword, string NewPassword);
    
}
