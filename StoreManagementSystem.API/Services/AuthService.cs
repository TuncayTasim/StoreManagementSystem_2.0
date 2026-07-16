using StoreManagementSystem.API.DTOs;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using StoreManagementSystem.API.Helpers;
using StoreManagementSystem.API.Interfaces;
using StoreManagementSystem.API.Data.Entities;

namespace StoreManagementSystem.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repository;
        private readonly IConfiguration _configuration;

        public AuthService(IAuthRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        public async Task<UserResponseDTO?> RegisterAsync(UserRegisterDTO dto)
        {
            if (await _repository.UserExistsAsync(dto.UserName, dto.Email)) return null;

            var user = new User
            {
                UserName = dto.UserName,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                RoleId = dto.RoleId,
                StatusId = 2,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                PhoneNumber = dto.PhoneNumber,
                ActionToken = Guid.NewGuid().ToString().Substring(0, 8)
            };

            await _repository.AddUserAsync(user);
            await _repository.SaveChangesAsync();

            string message = $"<h1>Confirm Your Email</h1><p>Welcome {user.FirstName}!</p><p>Your confirmation token is: <strong>{user.ActionToken}</strong></p>";
            await EmailSender.SendEmailAsync(_configuration, user.Email, "Confirm your email - Store System", message);

            return new UserResponseDTO { 
                UserId = user.UserId, 
                UserName = user.UserName, 
                Token = "CONFIRMATION_SENT",
                RoleId = user.RoleId
            };
        }

        public async Task<UserResponseDTO?> LoginAsync(UserLoginDTO dto)
        {
            var user = await _repository.GetUserByUserNameAsync(dto.UserName);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash)) return null;

            if (user.StatusId != 1)
            {
                throw new Exception("EMAIL_NOT_CONFIRMED");
            }

            return new UserResponseDTO { 
                UserId = user.UserId, 
                UserName = user.UserName, 
                Token = GenerateJwtToken(user),
                RoleId = user.RoleId
            };
        }

        public async Task<bool> ConfirmEmailAsync(string token)
        {
            var user = await _repository.GetUserByTokenAsync(token);
            if (user == null) return false;

            user.StatusId = 1;
            user.ActionToken = "";
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var user = await _repository.GetUserByEmailAsync(email);
            if (user == null) return false;

            user.ActionToken = Guid.NewGuid().ToString().Substring(0, 8);
            await _repository.SaveChangesAsync();

            string message = $"<h1>Reset Your Password</h1><p>Hello {user.FirstName},</p><p>You requested a password reset. Use this token: <strong>{user.ActionToken}</strong></p>";
            await EmailSender.SendEmailAsync(_configuration, user.Email, "Reset Password - Store System", message);

            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDTO dto)
        {
            if (dto.NewPassword != dto.ConfirmPassword) return false;

            var user = await _repository.GetUserByTokenAsync(dto.Token);
            if (user == null) return false;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            user.ActionToken = "";
            await _repository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDTO dto)
        {
            var user = await _repository.GetUserByIdAsync(userId);
            if (user == null) return false;

            if (!BCrypt.Net.BCrypt.Verify(dto.OldPassword, user.PasswordHash))
                return false;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            await _repository.SaveChangesAsync();
            return true;
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtKey = _configuration["Jwt:Key"];
            var key = Encoding.ASCII.GetBytes(jwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] 
                { 
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Role, user.Role?.Name ?? "Staff")
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
