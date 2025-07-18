﻿using ProductTask.Data;
using ProductTask.Model;

namespace ProductTask.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataEF _entityFramework;

        public UserRepository(DataEF entityFramework) // Use dependency injection directly
        {
            _entityFramework = entityFramework;
        }

        public bool SaveChanges()
        {
            return _entityFramework.SaveChanges() > 0;
        }

        public bool RemoveEntitys<T>(T entityToRemove)
            where T : class
        {
            if (entityToRemove != null)
            {
                _entityFramework.Remove(entityToRemove);
                return true;
            }
            return false;
        }

        public IEnumerable<Users> GetUsers()
        {
            return _entityFramework.Users.ToList(); // No need for explicit type here
        }

        public async Task<Users?> GetSingleUserAsync(int userId)
        {
            return await _entityFramework.Users.FindAsync(userId); // Use FindAsync for better performance
        }

        public async Task<string?> GetUserNameByUserIdAsync(int userId)
        {
            var user = await _entityFramework.Users.FindAsync(userId);
            return user != null ? $"{user.FirstName} {user.LastName}" : null;
        }
    }
}
