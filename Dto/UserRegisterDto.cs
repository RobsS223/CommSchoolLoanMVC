using System.ComponentModel.DataAnnotations;

namespace CommSchoolBankV2.Dto;

public class UserRegisterDto
{
    [Required(ErrorMessage = "This field is required")]
    [StringLength(50, ErrorMessage = "Id Number cannot be longer than 50 characters.")]
    public required string IdNumber { get; set; }

    [Required(ErrorMessage = "This field is required")]
    [StringLength(50, ErrorMessage = "First Name cannot be longer than 50 characters.")]
    public required string Firstname { get; set; }

    [Required(ErrorMessage = "This field is required")]
    [StringLength(50, ErrorMessage = "Last Name cannot be longer than 50 characters.")]
    public required string Lastname { get; set; }

    [Required(ErrorMessage = "This field is required")]
    [StringLength(50, ErrorMessage = "Username cannot be longer than 50 characters.")]
    public required string Username { get; set; }

    [Required(ErrorMessage = "This field is required")]
    [DataType(DataType.Date)]
    [Range(typeof(DateTime), "1900-01-01", "2006-12-31", ErrorMessage = "Age must be more then 18 years.")]
    public DateTime? DateOfBirth { get; set; }

    [Required(ErrorMessage = "This field is required")]
    [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters.")]
    public required string City { get; set; }

    [StringLength(100)] public string? Address { get; set; }

    [Required(ErrorMessage = "This field is required")]
    [Phone]
    [StringLength(20, ErrorMessage = "Phone number cannot be longer than 20 characters.")]
    public required string PhoneNumber { get; set; }

    [EmailAddress]
    [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "This field is required")]
    [StringLength(30, MinimumLength = 6)]
    [DataType(DataType.Password, ErrorMessage = "Password can be from 6 to 30 characters.")]
    public required string Password { get; set; }

    [Required(ErrorMessage = "This field is required")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    [DataType(DataType.Password)]
    public required string ConfirmPassword { get; set; }
}