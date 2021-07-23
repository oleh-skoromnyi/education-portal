using EducationPortal.Core;

namespace EducationPortal.UI.Commands
{
    class LogoutCommand : ICommand
    {
        public string Name { get; } = "Logout";

        private IAuthService authorization;
        public LogoutCommand(IAuthService auth)
        {
            this.authorization = auth;
        }

        public bool IsAvailable(State state)
        {
            return state.CurrentState == State.ProgramState.authorized;
        }

        public void Execute(ref State state, ref int userId)
        {
            userId = -1;
            state.CurrentState = State.ProgramState.guest;
        }
    }
}
