using EducationPortal.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.UI
{
    public class LoginCommand : ICommand
    {
        private Authorization authorization;

        public LoginCommand(Authorization auth)
        {
            this.authorization = auth;
        }
        public void Execute()
        {
            Console.Clear();
            Console.WriteLine("Input your login: ");
            string login = Console.ReadLine();
            Console.WriteLine("Input your password: ");
            string password = Console.ReadLine();
            if (authorization.Login(login, password))
            {
                Console.WriteLine("Authorization successful");
            }
            else
            {
                Console.WriteLine("Authorization failed");
            }
            Console.WriteLine("Press Enter to continue");
            Console.ReadLine();
        }
    }
}
