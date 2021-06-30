using EducationPortal.Core;
using System;

namespace EducationPortal.UI.Commands
{
    public class RegisterCommand : ICommand
    {
        public string Name { get; } = "Registration Menu";

        private IAuthService authorization;

        public RegisterCommand(IAuthService auth)
        {
            this.authorization = auth;
        }
        public bool IsAvailable(State state)
        {
            return state.CurrentState == State.ProgramState.guest;
        }

        public void Execute(ref State state, ref int userId)
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
            state.CurrentState = State.ProgramState.guest;
            Console.WriteLine("Press Enter to continue");
            Console.ReadLine();
        }
    }
}
