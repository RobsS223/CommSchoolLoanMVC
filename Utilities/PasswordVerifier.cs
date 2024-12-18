using System.Text;

namespace CommSchoolBankV2.Utilities;

public static class PasswordVerifier
{
    private static readonly string FixedKey = "YourSuperSecretFixedKey"; // Тот же ключ

    public static bool VerifyPasswordHash(string password, string storedHash)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512(Encoding.UTF8.GetBytes(FixedKey)))
        {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return storedHash == Convert.ToBase64String(computedHash);
        }
    }
}