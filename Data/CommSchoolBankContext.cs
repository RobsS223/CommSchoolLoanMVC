using CommSchoolBankV2.Models;
using Microsoft.EntityFrameworkCore;

namespace CommSchoolBankV2.Data;

public class CommSchoolBankContext : DbContext
{
    public CommSchoolBankContext(DbContextOptions<CommSchoolBankContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Loan>().HasKey(l => l.LoanId);
        modelBuilder.Entity<User>().HasKey(u => u.UserId);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Loan> Loans { get; set; }
}