using BTL.Models.Class;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
namespace BTL.Models.MK
{
    public class PasswordService
    {
        private readonly PasswordHasher<Object> Hash=new PasswordHasher<Object> ();
        public string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Mật khẩu không được để trống");
            return Hash.HashPassword(null, password);
        }
        public bool VerifyPassword(string password,string passwordHash)
        {
            
            if (passwordHash == null || password == null)
            {
                return false;
            }
           var result = Hash.VerifyHashedPassword(null,passwordHash,password);
            return result == PasswordVerificationResult.Success;
        }
    }
}
