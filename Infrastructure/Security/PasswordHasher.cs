using Application.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly PasswordHasher<object> _passwordHasher = new PasswordHasher<object>();

        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(null, password);
        }

        public bool VerifyPassword(string enteredPassword, string storedHash)
        {
            var verificationResult = _passwordHasher.VerifyHashedPassword(null, storedHash, enteredPassword);
            return verificationResult == PasswordVerificationResult.Success;
        }
    }
}
