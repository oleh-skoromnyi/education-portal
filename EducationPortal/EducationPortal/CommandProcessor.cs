using System;
using System.Collections.Generic;
using System.Linq;
using EducationPortal.UI.Commands;

namespace EducationPortal.UI
{
    public class CommandProcessor
    {
        private List<ICommand> commands;
        private State programState;
        public CommandProcessor(IEnumerable<ICommand> commandDictionary)
        {
            this.commands = commandDictionary.ToList();
            programState = new State();
            programState.CurrentState = State.ProgramState.guest;
        }

        public List<string> getCommands()
        {
            var resultList = new List<string>();
            int index = 1;
            foreach (var command in commands)
            {
                if (command.IsAvailable(programState))
                {
                    resultList.Add($"{index++}. {command.Name}");
                }
            }
            return resultList;
        
        }

        public bool Execute(ref int userId,string command)
        {
            int index = commands.FindIndex(x => x.Name == command);
            if (index != -1)
            {
                Console.Clear();
                commands[index].Execute(ref programState,ref userId);
                return true;
            }
            return false;
        }
    }
}
