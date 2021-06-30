using EducationPortal.Core;
using System;

namespace EducationPortal.UI.Commands
{
    public class LoginCommand : ICommand
    {
        public string Name { get; } = "Login";

        private IAuthService authorization;

        public LoginCommand(IAuthService auth)
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
            state.CurrentState = State.ProgramState.guest;
            userId = authorization.Login(login, password);
            if (userId != -1)
            {
                Console.WriteLine("Authorization successful");
                state.CurrentState = State.ProgramState.authorized;
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
