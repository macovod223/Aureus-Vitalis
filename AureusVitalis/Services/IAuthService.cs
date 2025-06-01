using System.Threading.Tasks;
using AureusVitalis.Data.Entities;  
using AureusVitalis.Models;         

namespace AureusVitalis.Services
{
    public interface IAuthService
    {
        Task<bool>     RegisterAsync(RegisterModel model);
        Task<AppUser?> ValidateUserAsync(LoginModel model);
    }
}