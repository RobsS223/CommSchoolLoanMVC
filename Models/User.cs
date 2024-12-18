using System.ComponentModel.DataAnnotations;

namespace CommSchoolBankV2.Models;

public class User
{
    [Key]
    public int UserId { get; set; } // Уникальный идентификатор пользователя

    [Required]
    public string IdNumber { get; set; } // Идентификационный номер
    
    [Required]
    public string FirstName { get; set; } // Имя
    
    [Required]
    public string LastName { get; set; } // Фамилия
    
    [Required]
    public string Username { get; set; } // Логин
    
    public DateTime? DateOfBirth { get; set; } // Дата рождения (может быть необязательной)

    public string City { get; set; } // Город проживания
    
    public string? Address { get; set; } // Адрес (необязательное поле)
    
    [Required]
    [Phone]
    public string PhoneNumber { get; set; } // Телефонный номер
    
    [EmailAddress]
    public string? Email { get; set; } // Электронная почта (необязательное поле)
    
    [Required]
    public string PasswordHash { get; set; } // Хэш пароля
    
    [Required]
    public string Role { get; set; } = "User"; // Роль пользователя, по умолчанию "User"
    
    public bool IsBlocked { get; set; } = false; // Статус блокировки (по умолчанию False)

    public ICollection<Loan> Loans { get; set; } // Список кредитов, связанных с пользователем
}