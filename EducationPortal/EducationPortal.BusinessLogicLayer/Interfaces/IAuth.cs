using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.BusinessLogicLayer
{
    public interface IAuth
    {

        public bool Login(string login, string password);

        public bool Register(string login, string password);

        public bool Logout();

        public bool IsLogin();

        public string GetLogin();
    }
}
