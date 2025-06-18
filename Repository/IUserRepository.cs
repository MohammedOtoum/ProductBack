using ProductTask.Model;

namespace ProductTask.Repository
{
    public interface IUserRepository
    {
        public bool SaveChanges();
        public bool RemoveEntitys<T>(T entityToRemove)
            where T : class;
        public IEnumerable<Users> GetUsers();
        public Task<Users?> GetSingleUserAsync(int userId);
        public Task<string?> GetUserNameByUserIdAsync(int userId);
    }
}
