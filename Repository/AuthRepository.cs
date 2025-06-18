using Microsoft.EntityFrameworkCore;
using ProductTask.Data;
using ProductTask.Model;
using System;

namespace ProductTask.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataEF _context;

        public AuthRepository(DataEF context)
        {
            _context = context;
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _context.users.AnyAsync(u => u.Email == email);
        }

        public async Task RegisterUserAsync(Users user)
        {
            _context.users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<Users?> GetUserByEmailAsync(string email)
        {
            return await _context.users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task UpdatePasswordAsync(Users user, byte[] newHash, byte[] newSalt)
        {
            user.passwordHash = newHash;
            user.passwordSalt = newSalt;
            _context.users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
