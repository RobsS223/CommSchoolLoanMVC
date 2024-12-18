using CommSchoolBankV2.Data;
using CommSchoolBankV2.Models;
using Microsoft.EntityFrameworkCore;

namespace CommSchoolBankV2.Service;

public interface IAccountantService
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> UpdateUserAsync(int id, User updatedUser);
    Task BlockUserAsync(int id);
    Task UnblockUserAsync(int id);
    Task<IEnumerable<Loan>> GetUserLoansAsync(int userId);
    Task<Loan?> UpdateLoanAsync(int loanId, Loan updatedLoan);
}

public class AccountantService : IAccountantService
{
    private readonly CommSchoolBankContext _context;

    public AccountantService(CommSchoolBankContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return (await _context.Users.ToListAsync())!;
    }

    public async Task<User?> UpdateUserAsync(int id, User updatedUser)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            throw new Exception("User not found.");
        }

        user.FirstName = updatedUser.FirstName;
        user.LastName = updatedUser.LastName;
        user.Email = updatedUser.Email;
        user.PhoneNumber = updatedUser.PhoneNumber;
        user.City = updatedUser.City;
        user.Address = updatedUser.Address;

        _context.Entry(user).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task BlockUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            throw new Exception("User not found.");
        }

        user.IsBlocked = true;
        _context.Entry(user).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task UnblockUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            throw new Exception("User not found.");
        }

        user.IsBlocked = false;
        _context.Entry(user).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Loan>> GetUserLoansAsync(int userId)
    {
        return await _context.Loans.Where(l => l.UserId == userId).ToListAsync();
    }

    public async Task<Loan?> UpdateLoanAsync(int loanId, Loan updatedLoan)
    {
        var loan = await _context.Loans.FindAsync(loanId);
        if (loan == null)
        {
            throw new Exception("Loan not found.");
        }

        loan.Amount = updatedLoan.Amount;
        loan.Currency = updatedLoan.Currency;
        loan.PeriodMonths = updatedLoan.PeriodMonths;
        loan.Status = updatedLoan.Status;
        loan.Type = updatedLoan.Type;

        _context.Entry(loan).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return loan;
    }
}