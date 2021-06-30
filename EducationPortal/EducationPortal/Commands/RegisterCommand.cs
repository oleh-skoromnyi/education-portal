using EducationPortal.BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.UI
{
    public class RegisterCommand : ICommand
    {
        private IAuth authorization;

        public RegisterCommand(IAuth auth)
        {
            this.authorization = auth;
        }
        public bool IsAvailable()
        {
            return !authorization.IsLogin();
        }

        public void Execute()
        {
            Console.Clear();
            Console.WriteLine("Input your login: ");
            string login = Console.ReadLine();
            Console.WriteLine("Input your password: ");
            string password = Console.ReadLine();
            if (authorization.Register(login, password))
            {
                Console.WriteLine("Registration successful");
            }
            else
            {
                Console.WriteLine("Registration failed");
            }
            Console.WriteLine("Press Enter to continue");
            Console.ReadLine();
        }
    }
}
