using System;
using EducationPortal.Core;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EducationPortal.UI.Commands
{
    public class UserMenuCommand : ICommand
    {
        public string Name { get; } = "User Menu";

        public void Execute(ref State state, ref int userId)
        {
            state.CurrentState = State.ProgramState.userMenu;
        }

        public bool IsAvailable(State state)
        {
            return state.CurrentState == State.ProgramState.authorized;
        }
    }
}
