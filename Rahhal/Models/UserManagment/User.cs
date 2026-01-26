using Microsoft.AspNetCore.Identity;
using Rahhal.Common.Enum;
using System.ComponentModel.DataAnnotations;

namespace Rahhal.Models.UserManagment
{
    public class User : BaseModel
    {
        [Required]
        [MaxLength(100)]
        public string Fname { get; set; }
        [Required]
        [MaxLength(100)]
        public string Lname { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }    


        [Required]
        [MaxLength(200)]
        public string Email { get; set; }

        public string? ProfilePicture { get; set; }

        [MaxLength(250)]
        public string? Bio { get; set; }

        public string? Phone { get; set; }   

        public string? HashedPassword { get; set; }

        public string? SaltPassword { get; set; }

        public Role Role { get; set; }
    }
}
