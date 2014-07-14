namespace Minesweeper
{
    // The 'Invoker' class
    class CommandExecutor
    {
        public void ExecuteCommand(ICommand cmd)
        {
            cmd.Execute();
        }
    }
}
