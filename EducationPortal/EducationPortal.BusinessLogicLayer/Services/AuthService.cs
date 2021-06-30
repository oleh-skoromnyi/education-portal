using System;
using System.Security.Cryptography;
using System.Text;
using EducationPortal.Core;

namespace EducationPortal.BLL
{
    public class AuthService : IAuthService
    {
        private IRepository<User> _repository;

        public AuthService(IRepository<User> context)
        {
            this._repository = context;
        }

        public int Login(string login, string password)
        {
            int id = _repository.FindIndex(login);
            if (id != 0)
            {
                User temp = _repository.Find(new Specification<User>(x=>x.Id == id));
                if (temp != null)
                {
                    if (VerifyHash(password, temp.Password))
                    {
                        return temp.Id;
                    }
                }
            }
            return -1;
        }

        public bool Register(string login, string password)
        {
            string hashedPass = GetHash(password);
            if (_repository.FindIndex(login) == 0)
            {
                return _repository.Save(new User { Login = login, Password = hashedPass });
            }
            return false;
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
