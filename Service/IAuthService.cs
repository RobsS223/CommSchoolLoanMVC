using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CommSchoolBankV2.Data;
using CommSchoolBankV2.Dto;
using CommSchoolBankV2.Models;
using CommSchoolBankV2.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CommSchoolBankV2.Service;

public interface IAuthService
{
    Task<User> Register(UserRegisterDto userDto);
    Task<string> Login(UserLoginDto userDto);
}

public class AuthService : IAuthService
{
    private readonly CommSchoolBankContext _context;
    private readonly ITokenGenerator _tokenGenerator;

    public AuthService(CommSchoolBankContext context, ITokenGenerator tokenGenerator)
    {
        _context = context;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<User> Register(UserRegisterDto userDto)
    {
        if (await UserExists(userDto.Username))
        {
            throw new Exception("Username already exists");
        }

        string passwordHash = PasswordHasher.CreatePasswordHash(userDto.Password);

        var user = new User
        {
            IdNumber = userDto.IdNumber,
            FirstName = userDto.Firstname,
            LastName = userDto.Lastname,
            Username = userDto.Username,
            DateOfBirth = userDto.DateOfBirth,
            City = userDto.City,
            Address = userDto.Address,
            PhoneNumber = userDto.PhoneNumber,
            Email = userDto.Email,
            PasswordHash = passwordHash,
            Role = "User",
            IsBlocked = false
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<string> Login(UserLoginDto userDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userDto.Username);

        if (user == null || !PasswordVerifier.VerifyPasswordHash(userDto.Password, user.PasswordHash))
        {
            throw new Exception("Invalid username or password.");
        }

        return _tokenGenerator.GenerateToken(user);
    }

    private async Task<bool> UserExists(string username)
    {
        return await _context.Users.AnyAsync(u => u.Username == username);
    }
}
