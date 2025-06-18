using System.ComponentModel.DataAnnotations;

namespace ProductTask.Model
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }

        public byte[] passwordSalt { get; set; }
        public byte[] passwordHash { get; set; }

        public Users()
        {
            passwordSalt = new byte[0];
            passwordHash = new byte[0];
        }

    }
}
