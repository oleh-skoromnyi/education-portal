namespace EducationPortal.UI.Commands
{
    public interface ICommand
    {
        public string Name { get; }
        public bool IsAvailable(State state);

        public void Execute(ref State state, ref int userId);
    }
}
