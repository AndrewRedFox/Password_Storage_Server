using System.Security.Cryptography;
using System.Text;

namespace RedFox.PasswordStorage.Infrastructure.Helpers;

public static class HashPasswordHelper
{
    public static string HashPasswordToSha256(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            string hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            return hash;
        }

    }
}