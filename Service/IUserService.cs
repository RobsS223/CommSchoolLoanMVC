// Service
using CommSchoolBankV2.Data;
using CommSchoolBankV2.Models;
using Microsoft.EntityFrameworkCore;

namespace CommSchoolBankV2.Service;

public interface IUserService
{
    Task<User?> GetUserByIdAsync(int userId);
    Task<bool> IsUserBlockedAsync(int userId);
    Task<IEnumerable<Loan>> GetUserLoansAsync(int userId);
    Task<Loan?> ApplyForLoanAsync(int userId, Loan loan);
}

public class UserService : IUserService
{
    private readonly CommSchoolBankContext _context;

    public UserService(CommSchoolBankContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        return await _context.Users.FindAsync(userId);
    }

    public async Task<bool> IsUserBlockedAsync(int userId)
    {
        var user = await GetUserByIdAsync(userId);
        return user?.IsBlocked ?? true; // if not found = blocked?...
    }

    public async Task<IEnumerable<Loan>> GetUserLoansAsync(int userId)
    {
        return await _context.Loans.Where(l => l.UserId == userId).ToListAsync();
    }

    public async Task<Loan?> ApplyForLoanAsync(int userId, Loan loan)
    {
        if (await IsUserBlockedAsync(userId))
        {
            throw new Exception("User is blocked and cannot apply for a loan.");
        }

        if (loan.Amount <= 100)
        {
            throw new Exception("Loan amount must be greater than 100.00");
        }

        if (string.IsNullOrEmpty(loan.Type) || !new[] { "Quick Loan", "Car Loan", "Installment" }.Contains(loan.Type))
        {
            throw new Exception("Invalid loan type. Allowed types are: Quick Loan, Car Loan, Installment.");
        }

        if (string.IsNullOrEmpty(loan.Currency) || !new[] { "GEL", "EUR", "USD" }.Contains(loan.Currency))
        {
            throw new Exception("Invalid currency. Allowed currencies are: GEL, EUR, USD.");
        }

        if (loan.PeriodMonths <= 0 || loan.PeriodMonths > 360)
        {
            throw new Exception("Invalid loan period. Period must be between 1 and 360 months.");
        }

        loan.UserId = userId;
        loan.Status = "Pending";

        _context.Loans.Add(loan);
        await _context.SaveChangesAsync();

        return loan;
    }
}
    