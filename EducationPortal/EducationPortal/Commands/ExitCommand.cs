using System;

namespace EducationPortal.UI.Commands
{
    class ExitCommand: ICommand
    {
        public string Name { get; } = "Exit";
        public bool IsAvailable(State state)
        {
            return true;
        }

        public void Execute(ref State state, ref int userId)
        {
            Environment.Exit(0);
        }
    }
}
