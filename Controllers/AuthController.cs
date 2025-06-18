using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductTask.AuthHelper;
using ProductTask.Data;
using ProductTask.Dto;
using ProductTask.Model;
using ProductTask.Repository;
using System.Security.Claims;
using System.Security.Cryptography;

namespace ProductTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _config;
        private readonly AuthHelper.AuthHelper _authHelper;
        private readonly DataEF _context;

        public AuthController(IAuthRepository authRepository, IConfiguration config, DataEF context)
        {
            _authRepository = authRepository;
            _config = config;
            _authHelper = new AuthHelper.AuthHelper(config);
            _context = context;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(UserSignUp dto)
        {
            if (dto.Password != dto.PasswordConfirm)
                return BadRequest("Passwords do not match.");

            if (await _authRepository.UserExistsAsync(dto.Email))
                return BadRequest("User already exists.");

            byte[] salt = new byte[120 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetNonZeroBytes(salt);
            }

            var passwordHash = _authHelper.GetPasswordHash(dto.Password, salt);

            var user = new Users
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Gender = dto.Gender,
                passwordHash = passwordHash,
                passwordSalt = salt
            };

            await _authRepository.RegisterUserAsync(user);
            return Ok(new { message = "User registered successfully." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLogin dto)
        {
            var user = await _authRepository.GetUserByEmailAsync(dto.Email);
            if (user == null) return Unauthorized("User not found.");

            var hash = _authHelper.GetPasswordHash(dto.Password, user.passwordSalt);
            if (!hash.SequenceEqual(user.passwordHash))
                return Unauthorized("Incorrect password.");

            string token = _authHelper.CreateToken(user.UserId);
            return Ok(new { token });
        }

        [Authorize]
        [HttpGet("RefreshToken")]
        public IActionResult RefreshToken()
        {
            string? userIdClaim = User.FindFirst("UserID")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("Invalid or missing user ID claim.");
            }

            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(new Dictionary<string, string> { { "userId", user.UserId.ToString() } });
        }



        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var user = await _authRepository.GetUserByEmailAsync(dto.Email);
            if (user == null) return NotFound("User not found.");

            var oldHash = _authHelper.GetPasswordHash(dto.OldPassword, user.passwordSalt);
            if (!oldHash.SequenceEqual(user.passwordHash))
                return Unauthorized("Incorrect old password.");

            byte[] newSalt = new byte[120 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetNonZeroBytes(newSalt);
            }

            var newHash = _authHelper.GetPasswordHash(dto.NewPassword, newSalt);
            await _authRepository.UpdatePasswordAsync(user, newHash, newSalt);

            return Ok("Password changed successfully.");
        }
    }
}
