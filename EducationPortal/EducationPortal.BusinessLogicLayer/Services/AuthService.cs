using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace EducationPortal.BusinessLogicLayer
{
    public class AuthService : IAuth
    {
        private IDbContext<User> dbContext;
        private User user;

        public AuthService(IDbContext<User> context)
        {
            this.dbContext = context;
        }

        public bool Login(string login, string password)
        {
            User temp = dbContext.Load(login);
            if (temp == null)
            {
                return false;
            }
            if (VerifyHash(password, temp.Password))
            {
                this.user = temp;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Register(string login, string password)
        {
            User temp = new User(
               dbContext.Count(),
               login,
               GetHash(password));
            return dbContext.Save(temp);
        }

        public bool Logout()
        {
            user = null;
            return true;
        }

        public bool IsLogin()
        {
            return user != null ? true : false;
        }

        public string GetLogin()
        {
            if (user != null)
            {
                return user.Login;
            }
            else
            {
                return "Anonim";
            }
        }

        private string GetHash(string input)
        {
            using (var hashAlgorithm = SHA512.Create())
            {
                byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                return sBuilder.ToString();
            }
        }

        private bool VerifyHash(string input, string hash)
        {
            var hashOfInput = GetHash(input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return comparer.Compare(hashOfInput, hash) == 0;
        }
    }
}
