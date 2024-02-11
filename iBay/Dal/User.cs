using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dal
{
    public enum UserRole
    {
        seller,
        buyer
    }
    [Index(nameof(Email),IsUnique = true)]
    public class User
    {
        public int UserId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Email { get; set; }
        public string Pseudo { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
}
