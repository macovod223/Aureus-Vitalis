using AureusVitalis.Models;
using AureusVitalis.Services;
using Microsoft.AspNetCore.Mvc;

namespace AureusVitalis.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _auth;
        public AuthController(IAuthService auth) => _auth = auth;

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var ok = await _auth.RegisterAsync(model);
            if (!ok)
            {
                ModelState.AddModelError("", "Email или Username уже заняты.");
                return View(model);
            }
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _auth.ValidateUserAsync(model);
            if (user == null)
            {
                ModelState.AddModelError("", "Неверный Email/Username или пароль.");
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}