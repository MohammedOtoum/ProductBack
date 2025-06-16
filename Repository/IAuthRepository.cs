using ProductTask.Model;

namespace ProductTask.Repository
{
    public interface IAuthRepository
    {
        public Task<bool> UserExistsAsync(string email);
        public Task RegisterUserAsync(Users user);
        public Task<Users?> GetUserByEmailAsync(string email);
        public Task UpdatePasswordAsync(Users user, byte[] newHash, byte[] newSalt);

    }
}
