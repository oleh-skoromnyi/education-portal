using EducationPortal.BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.UI
{
    public class CommandManager
    {
        CommandProcessor commandProcessor;
        IAuth authorization;
        public CommandManager(CommandProcessor processor, IAuth auth)
        {
            this.commandProcessor = processor;
            this.authorization = auth;
        }

        public void Start()
        {
            string input = "";
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Choose command, {authorization.GetLogin()}:");
                foreach (var command in commandProcessor.getCommands())
                {
                    Console.WriteLine(command);
                }

                input = Console.ReadLine();
                if(!commandProcessor.Execute(input.Trim()))
                {
                    Console.WriteLine($"Command not found. Press ENTER and try again.");
                    Console.ReadLine();
                }
            }
        }

    }
}
