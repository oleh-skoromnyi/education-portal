using EducationPortal.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EducationPortal.UI.Commands
{
    class MaterialMenuCommand : ICommand
    {
        public string Name { get; } = "Material Menu";
        
        public void Execute(ref State state, ref int userId)
        {
            state.CurrentState = State.ProgramState.materialMenu;
        }

        public bool IsAvailable(State state)
        {
            return state.CurrentState == State.ProgramState.authorized;
        }
    }
}
