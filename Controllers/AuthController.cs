using Microsoft.AspNetCore.Mvc;
using CommSchoolBankV2.Service;
using CommSchoolBankV2.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CommSchoolBankV2.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("auth/register")]
        public IActionResult Register()
        {
            // Возвращаем форму регистрации
            return View(new UserRegisterDto
            {
                IdNumber = string.Empty,
                Firstname = string.Empty,
                Lastname = string.Empty,
                Username = string.Empty,
                City = string.Empty,
                PhoneNumber = string.Empty,
                Password = string.Empty,
                ConfirmPassword = string.Empty,
            });
        }

        [HttpPost("auth/register")]
        public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
        {
            if (!ModelState.IsValid)
            {
                return View(userRegisterDto);
            }

            try
            {
                await _authService.Register(userRegisterDto);
                TempData["RegistrationSuccess"] = "User registered successfully!";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(userRegisterDto);
            }
        }

        [HttpGet("auth/login")]
        public IActionResult Login()
        {
            return View(new UserLoginDto());
        }

        [HttpPost("auth/login")]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            if (!ModelState.IsValid)
            {
                return View(userLoginDto);
            }

            try
            {
                var token = await _authService.Login(userLoginDto);
                if (string.IsNullOrEmpty(token))
                {
                    throw new Exception("Authentication failed.");
                }

                // Сохранение токена в cookie
                HttpContext.Response.Cookies.Append("Authorization", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false, // Установите в false если используете HTTP
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddDays(7)
                });

                // Определяем роль пользователя
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
                var roleClaim = jwtToken!.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value;

                if (roleClaim == "Admin")
                {
                    return RedirectToAction("Dashboard", "Accountant");
                }
                else
                {
                    return RedirectToAction("Dashboard", "User");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(userLoginDto);
            }
        }
    }
}