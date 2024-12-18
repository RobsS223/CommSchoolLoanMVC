using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CommSchoolBankV2.Models;
using CommSchoolBankV2.Service;

namespace CommSchoolBankV2.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class AccountantController : ControllerBase
{
    private readonly IAccountantService _accountantService;

    public AccountantController(IAccountantService accountantService)
    {
        _accountantService = accountantService;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var users = await _accountantService.GetAllUsersAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("users/{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] User updatedUser)
    {
        try
        {
            var user = await _accountantService.UpdateUserAsync(id, updatedUser);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("users/{id}/block")]
    public async Task<IActionResult> BlockUser(int id)
    {
        try
        {
            await _accountantService.BlockUserAsync(id);
            return Ok(new { message = "User successfully blocked." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("users/{id}/unblock")]
    public async Task<IActionResult> UnblockUser(int id)
    {
        try
        {
            await _accountantService.UnblockUserAsync(id);
            return Ok(new { message = "User successfully unblocked." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("users/{id}/loans")]
    public async Task<IActionResult> GetUserLoans(int id)
    {
        try
        {
            var loans = await _accountantService.GetUserLoansAsync(id);
            return Ok(loans);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("loans/{loanId}")]
    public async Task<IActionResult> UpdateLoan(int loanId, [FromBody] Loan updatedLoan)
    {
        try
        {
            var loan = await _accountantService.UpdateLoanAsync(loanId, updatedLoan);
            return Ok(loan);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
