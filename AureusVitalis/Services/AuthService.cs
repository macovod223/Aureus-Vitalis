using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AureusVitalis.Data;            // AppDbContext
using AureusVitalis.Data.Entities;   // AppUser
using AureusVitalis.Models;          // RegisterModel, LoginModel
using BC = BCrypt.Net.BCrypt;        

namespace AureusVitalis.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        public AuthService(AppDbContext db) => _db = db;

        public async Task<bool> RegisterAsync(RegisterModel model)
        {
            if (await _db.Users.AnyAsync(u =>
                    u.Email    == model.Email ||
                    u.Username == model.Username))
                return false;

            var user = new AppUser
            {
                Email        = model.Email,
                Username     = model.Username,
                PasswordHash = BC.HashPassword(model.Password),
                Gender       = model.Gender,
                CreatedAt    = DateTime.UtcNow
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<AppUser?> ValidateUserAsync(LoginModel model)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u =>
                u.Email    == model.EmailOrUsername ||
                u.Username == model.EmailOrUsername);

            if (user is null) return null;

            return BC.Verify(model.Password, user.PasswordHash)
                ? user
                : null;
        }
    }
}