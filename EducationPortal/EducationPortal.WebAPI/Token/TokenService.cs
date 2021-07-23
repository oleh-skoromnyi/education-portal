using EducationPortal.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.WebAPI
{
    public class TokenService : ITokenService
    {
        private IUserService _userService;

        public TokenService(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<bool> VerifyToken(string token)
        {
            var id = GetUserIdByToken(token);
            if (id > 0)
            {
                var newToken = await CreateToken(id);
                return newToken == token;
            }
            return false;
        }

        public int GetUserIdByToken(string token)
        {
            int id = 0;
            int index = token.IndexOf('s');
            if (index != -1)
            {
                int.TryParse(token.Substring(0, index), out id);
            }
            return id;
        }

        public async Task<string> CreateToken(int id)
        {
            var user = await _userService.GetPersonalDataAsync(id);
            string rawData = $"Token {user.Id} 1234 {user.Login} 4321 {user.Password} End Token";
            var sBuilder = new StringBuilder();
            sBuilder.Append(id.ToString() + "s");
            using (var hashAlgorithm = SHA512.Create())
            {
                byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                return sBuilder.ToString();
            }
        }
    }
}
