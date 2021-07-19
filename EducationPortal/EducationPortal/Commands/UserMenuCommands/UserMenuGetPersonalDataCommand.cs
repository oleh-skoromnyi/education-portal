using System;
using EducationPortal.Core;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EducationPortal.UI.Commands
{
    public class UserMenuGetPersonalDataCommand : ICommand
    {
        public string Name { get; } = "Get personal data";

        private IUserService service;

        public UserMenuGetPersonalDataCommand(IUserService service)
        {
            this.service = service;
        }
        public void Execute(ref State state, ref int userId)
        {
            var userData = service.GetPersonalDataAsync(userId).GetAwaiter().GetResult();
            Console.WriteLine($"Login: {userData.Login}");
            Console.WriteLine($"Name: {userData.Name}");
            Console.WriteLine($"Email: {userData.Email}");
            Console.WriteLine($"Phone: {userData.Phone}");
            Console.ReadLine();
        }

        public bool IsAvailable(State state)
        {
            return state.CurrentState == State.ProgramState.userMenu;
        }
    }
}
