using System;
using System.Security.Cryptography;
using System.Text;
using EducationPortal.Core;
using System.Threading.Tasks;
using System.Threading;
using FluentValidation;

namespace EducationPortal.BLL
{
    public class AuthService : IAuthService
    {
        private IRepository<User> _userRepository;
        private IValidator<User> _userValidator;

        public AuthService(IRepository<User> context, IValidator<User> userValidator)
        {
            this._userRepository = context;
            this._userValidator = userValidator;
        }

        public async Task<int> LoginAsync(string login, string password, CancellationToken cancellationToken = default)
        {
            var result = await _userRepository.FindAsync(new FindUserByLoginSpecification(login), cancellationToken);
            if (result != null)
            {
                User temp = result;
                if (VerifyHash(password, temp.Password))
                {
                    return temp.Id;
                }
            }
            return -1;
        }

        public async Task<bool> RegisterAsync(User user, CancellationToken cancellationToken = default)
        {
            if ((await _userValidator.ValidateAsync(user, cancellationToken)).IsValid)
            {
                var regUser = user;
                regUser.Password = GetHash(regUser.Password);
                if (!await _userRepository.ExistAsync(new FindUserByLoginSpecification(user.Login), cancellationToken))
                {
                    return await _userRepository.InsertAsync(regUser, cancellationToken);
                }
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
