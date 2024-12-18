using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommSchoolBankV2.Models;

public class Loan
{
    [Key]
    public int LoanId { get; set; }
    
    [ForeignKey("User")]
    public int UserId { get; set; }
    
    [Required]
    public string Type { get; set; } // Тип кредита (например, "Car Loan", "Home Loan")
    
    [Required]
    [Range(100.00, double.MaxValue, ErrorMessage = "Amount must be greater than 100.00.")]
    public decimal Amount { get; set; }
    
    [Required]
    public string Currency { get; set; } = "GEL"; // По умолчанию GEL
    
    [Required]
    [Range(1, 360, ErrorMessage = "Period must be between 1 and 360 months.")]
    public int PeriodMonths { get; set; }
    
    [Required]
    public string Status { get; set; } = "Pending"; // По умолчанию Pending
    
    public User User { get; set; } // Навигационное свойство для связи с пользователем
}