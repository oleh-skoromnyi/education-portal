using EducationPortal.Core;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.UI.Commands
{
    public class CourseMenuCommand : ICommand
    {
        public string Name { get; } = "Course Menu";

        public void Execute(ref State state, ref int userId)
        {
            state.CurrentState = State.ProgramState.courseMenu;
        }

        public bool IsAvailable(State state)
        {
            return state.CurrentState == State.ProgramState.authorized;
        }
    }
}
