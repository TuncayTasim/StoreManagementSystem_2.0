using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagementSystem.API.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public Role? Role { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        public string? ActionToken { get; set; }

        [ForeignKey("Status")]
        public int StatusId { get; set; }
        public Status? Status { get; set; }
    }
}
