using System.Security.Claims;
using CommSchoolBankV2.Models;
using Microsoft.AspNetCore.Mvc;
using CommSchoolBankV2.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace CommSchoolBankV2.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        [Route("user/dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                _logger.LogWarning("User ID not found in claims");
                return Unauthorized("User ID not found in claims");
            }

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User not found for ID: {UserId}", userId);
                return NotFound("User not found");
            }

            ViewBag.Firstname = user.FirstName;
            ViewBag.Lastname = user.LastName;
            ViewBag.Username = user.Username;
            ViewBag.Email = user.Email!;
            ViewBag.PhoneNumber = user.PhoneNumber;
            ViewBag.City = user.City;

            var loans = await _userService.GetUserLoansAsync(userId);
            ViewBag.ActiveLoans = loans.Where(l => l.Status == "Active").ToList();

            return View();
        }

        [HttpGet("user/applyloan")]
        public IActionResult ApplyLoan()
        {
            var loan = new Loan();
            return View(loan);
        }

        [HttpPost("user/applyloan")]
        public async Task<IActionResult> ApplyLoan(Loan loan)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                _logger.LogWarning("User ID not found in claims");
                return Unauthorized("User ID not found in claims");
            }

            try
            {
                var result = await _userService.ApplyForLoanAsync(userId, loan);
                _logger.LogInformation("Loan applied for User ID: {UserId}", userId);
                TempData["LoanSuccessMessage"] = "Your loan application has been submitted. You will receive a response within 3 business days.";
                return RedirectToAction("Dashboard");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error applying for loan for User ID: {UserId}", userId);
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(loan);
            }
        }

        [HttpGet("user/loans")]
        public async Task<IActionResult> Loans()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                _logger.LogWarning("User ID not found in claims");
                return Unauthorized("User ID not found in claims");
            }

            var loans = await _userService.GetUserLoansAsync(userId);
            _logger.LogInformation("Fetching loans for User ID: {UserId}", userId);

            // Pass loans to the view
            return View(loans);
        }
        
        [HttpPost("auth/logout")]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("Authorization");

            return RedirectToAction("Login", "Auth");
        }
    }
}
