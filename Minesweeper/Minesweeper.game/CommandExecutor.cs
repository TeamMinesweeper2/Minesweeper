namespace Minesweeper.Game
{
    // The 'Invoker' class
    public class CommandExecutor
    {
        public bool ExecuteCommand(ICommand cmd)
        {
            return cmd.Execute();
        }
    }
}
