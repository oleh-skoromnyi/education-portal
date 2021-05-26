using EducationPortal.BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.UI
{
    class LogoutCommand : ICommand
    {
        private IAuth authorization;
        public LogoutCommand(IAuth auth)
        {
            this.authorization = auth;
        }

        public bool IsAvailable()
        {
            return authorization.IsLogin();
        }

        public void Execute()
        {
            authorization.Logout();
        }
    }
}
