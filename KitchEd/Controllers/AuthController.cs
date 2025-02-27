using KitchEd.Models.ViewModels.Auth;
using KitchEd.Data.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using KitchEd.Models.Entities;
using KitchEd.Services;

namespace KitchEd.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RecaptchaService _recaptchaService;
        private readonly IConfiguration _configuration;

        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RecaptchaService recaptchaService,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _recaptchaService = recaptchaService;
            _configuration = configuration;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.RecaptchaSiteKey = _configuration["reCAPTCHA:SiteKey"];
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            ViewBag.RecaptchaSiteKey = _configuration["reCAPTCHA:SiteKey"];

            // Verify reCAPTCHA
            var recaptchaVerified = await _recaptchaService.VerifyRecaptchaAsync(model.RecaptchaResponse);
            if (!recaptchaVerified)
            {
                ModelState.AddModelError(string.Empty, "Моля, потвърдете, че не сте робот.");
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Role == UserRoles.Admin)
            {
                TempData["ErrorMessage"] = "Не можете да се регистрирате като администратор.";
                return View(model);
            }

            var user = new User
            {
                UserName = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                ShortBio = model.ShortBio
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Assign role
                var roleResult = await _userManager.AddToRoleAsync(user, model.Role.ToString());

                if (roleResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in roleResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine("Result error: " + error.Description);
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.RecaptchaSiteKey = _configuration["reCAPTCHA:SiteKey"];
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            ViewBag.RecaptchaSiteKey = _configuration["reCAPTCHA:SiteKey"];

            // Verify reCAPTCHA
            var recaptchaVerified = await _recaptchaService.VerifyRecaptchaAsync(model.RecaptchaResponse);
            if (!recaptchaVerified)
            {
                ModelState.AddModelError(string.Empty, "Моля, потвърдете, че не сте робот.");
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Невалидно потребителско име или парола.");
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
