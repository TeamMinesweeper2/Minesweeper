namespace Minesweeper
{
    // The 'Invoker' class
    class CommandExecutor
    {
        public bool ExecuteCommand(ICommand cmd)
        {
            return cmd.Execute();
        }
    }
}
