using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPortal.WebAPI
{
    public interface ITokenService
    {
        public Task<bool> VerifyToken(string token);

        public Task<string> CreateToken(int id);

        public int GetUserIdByToken(string token);
    }
}
