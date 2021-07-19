using System;
using EducationPortal.Core;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EducationPortal.UI.Commands
{
    public class UserMenuChangePersonalDataCommand : ICommand
    {
        public string Name { get; } = "Change personal data";

        private IUserService service;

        public UserMenuChangePersonalDataCommand(IUserService service)
        {
            this.service = service;
        }
        public void Execute(ref State state, ref int userId)
        {
            int input;
            var userData = service.GetPersonalDataAsync(userId).GetAwaiter().GetResult();
            Console.WriteLine($"1.Name: {userData.Name}");
            Console.WriteLine($"2.Email: {userData.Email}");
            Console.WriteLine($"3.Phone: {userData.Phone}");
            Console.WriteLine($"\nChoose field to change");
            int.TryParse(Console.ReadLine(), out input);
            switch (input)
            {
                case 1:
                    Console.WriteLine($"\nWrite new name:");
                    userData.Name = Console.ReadLine();
                    break;
                case 2:
                    Console.WriteLine($"\nWrite new email:");
                    userData.Email = Console.ReadLine();
                    break;
                case 3:
                    Console.WriteLine($"\nWrite new phone:");
                    userData.Phone = Console.ReadLine();
                    break;
            }
            if (service.ChangePersonalDataAsync(userData).GetAwaiter().GetResult())
            {
                Console.WriteLine($"Changed successfully");
            }
            else
            {
                Console.WriteLine($"Change failed");
            }
            Console.ReadLine();
        }

        public bool IsAvailable(State state)
        {
            return state.CurrentState == State.ProgramState.userMenu;
        }
    }
}