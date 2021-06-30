namespace EducationPortal.Core
{
    public interface IAuthService
    {

        public int Login(string login, string password);

        public bool Register(string login, string password);
    }
}
