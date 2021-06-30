using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.UI.Commands
{
    public class BackCommand : ICommand
    {
        public string Name { get; } = "Back";

        public void Execute(ref State state, ref int userId)
        {
            switch(state.CurrentState)
            {
                case State.ProgramState.courseMenu:
                    state.CurrentState = State.ProgramState.authorized;
                    break;
                case State.ProgramState.materialMenu:
                    state.CurrentState = State.ProgramState.authorized;
                    break;
                case State.ProgramState.userMenu:
                    state.CurrentState = State.ProgramState.authorized;
                    break;
                case State.ProgramState.skillMenu:
                    state.CurrentState = State.ProgramState.authorized;
                    break;
                default:
                    state.CurrentState = State.ProgramState.guest;
                    break;
            }
        }

        public bool IsAvailable(State state)
        {
            return state.CurrentState != State.ProgramState.guest && state.CurrentState != State.ProgramState.authorized;
        }
    }
}
