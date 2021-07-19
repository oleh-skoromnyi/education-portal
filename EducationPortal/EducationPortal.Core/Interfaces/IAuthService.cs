using System.Threading;
using System.Threading.Tasks;

namespace EducationPortal.Core
{
    public interface IAuthService
    {
        public Task<int> LoginAsync(string login, string password, CancellationToken cancellationToken = default);

        public Task<bool> RegisterAsync(User user, CancellationToken cancellationToken = default);
    }
}
