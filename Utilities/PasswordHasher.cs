using System.Text;

namespace CommSchoolBankV2.Utilities;

public static class PasswordHasher
{
    private static readonly string FixedKey = "YourSuperSecretFixedKey"; // Фиксированный ключ

    public static string CreatePasswordHash(string password)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512(Encoding.UTF8.GetBytes(FixedKey)))
        {
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(passwordHash);
        }
    }
}