using System.ComponentModel.DataAnnotations;

namespace CommSchoolBankV2.Dto;

public class UserLoginDto
{
    [Required(ErrorMessage = "Username is Required")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Password is Required")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}