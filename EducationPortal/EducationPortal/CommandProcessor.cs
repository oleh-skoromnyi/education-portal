using System;
using System.Collections.Generic;
using System.Linq;
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
            var resultList = new List<string>();
            foreach (var command in commands)
            {
                if (command.Value.IsAvailable())
                {
                    resultList.Add(command.Key);
                }
            }
            return resultList;
        
        }

        public bool Execute(string command)
        {
            if (commands.ContainsKey(command.ToLower()))
            { 
                commands[command].Execute();
                return true;
            }
            return false;
        }
    }
}
