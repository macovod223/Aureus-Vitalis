using System.ComponentModel.DataAnnotations;

namespace AureusVitalis.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email/Username обязателен")]
        public string EmailOrUsername { get; set; } = string.Empty;

        [Required(ErrorMessage = "Пароль обязателен")]
        public string Password { get; set; } = string.Empty;
    }
}