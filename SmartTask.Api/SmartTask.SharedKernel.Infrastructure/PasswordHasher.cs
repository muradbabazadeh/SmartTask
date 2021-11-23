using System;
using System.Security.Cryptography;
using System.Text;
namespace SmartTask.SharedKernel.Infrastructure
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            return BitConverter.ToString(
                MD5.Create().ComputeHash(
                    Encoding.UTF8.GetBytes(password)
                )
            );
        }
    }
}
