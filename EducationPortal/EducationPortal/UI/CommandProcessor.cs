using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.UI
{
    public class CommandProcessor
    {
        Dictionary<string, ICommand> commands;

        public CommandProcessor(Dictionary<string, ICommand> commandDictionary)
        {
            this.commands = commandDictionary;
        }

        public List<string> getCommands()
        {
            return new List<string>(commands.Keys);
        }

        public void Execute(string command)
        {
            if (commands.ContainsKey(command))
            { 
                commands[command].Execute(); 
            }
        }
    }
}
