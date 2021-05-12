using EducationPortal.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.UI
{
    public class CommandManager
    {
        CommandProcessor commandProcessor;
        Authorization authorization;
        public CommandManager(CommandProcessor processor, Authorization auth)
        {
            this.commandProcessor = processor;
            this.authorization = auth;
        }

        public void Start()
        {
            
            string input = String.Empty;
            while (input != "Exit")
            {
                Console.Clear();
                Console.WriteLine($"Choose command, {authorization.GetLogin()}:");
                foreach (var command in commandProcessor.getCommands())
                {
                    Console.WriteLine(command);
                }
                Console.WriteLine("Exit");
                input = Console.ReadLine();
                commandProcessor.Execute(input.Trim());
            }
        }

    }
}
