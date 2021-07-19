using EducationPortal.Core;
using System;

namespace EducationPortal.UI
{
    public class CommandManager
    {
        private CommandProcessor commandProcessor;
        private IAuthService authService;
        private IUserService userService;
        protected int userId = -1;

        public CommandManager(CommandProcessor processor, IAuthService authService, IUserService userService)
        {
            this.commandProcessor = processor;
            this.authService = authService;
            this.userService = userService;
        }

        public void Start()
        {
            while (true)
            {
                var login = userService.GetUserLoginAsync(userId).GetAwaiter().GetResult();
                Console.Clear();
                Console.WriteLine($"Choose command, {login}:");
                var commands = commandProcessor.getCommands();
                foreach (var command in commands)
                {
                    Console.WriteLine(command);
                }
                int index;
                if (int.TryParse(Console.ReadLine(), out index))
                {
                    if (index <= commands.Count && index > 0)
                    {
                        if (!commandProcessor.Execute(ref userId,commands[index - 1].Split('.')[1].Trim()))
                        {
                            Console.WriteLine($"Command not found. Press ENTER and try again.");
                            Console.ReadLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Command not found. Press ENTER and try again.");
                        Console.ReadLine();
                    }
                }
            }
        }

    }
}
