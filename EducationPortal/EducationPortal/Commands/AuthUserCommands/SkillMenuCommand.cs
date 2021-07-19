using System;
using System.Collections.Generic;
using EducationPortal.Core;
using System.Text;

namespace EducationPortal.UI.Commands
{
    public class SkillMenuCommand : ICommand
    {
        public string Name { get; } = "Skill Menu";

        public void Execute(ref State state, ref int userId)
        {
            state.CurrentState = State.ProgramState.skillMenu;
        }

        public bool IsAvailable(State state)
        {
            return state.CurrentState == State.ProgramState.authorized;
        }
    }
}
