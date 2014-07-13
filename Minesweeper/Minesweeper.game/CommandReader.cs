namespace Minesweeper
{
    internal class CommandReader
    {
        //// TODO: add constants for all commands ("top", "restart"...)

        public CommandReader()
        {
        }

        public Command ReadCommand(IUserInputReader consoleManager, out CellPos cellToOpen)
        {
            string command = consoleManager.ReadInput();
            cellToOpen = CellPos.Empty;

            if (command.Equals("restart"))
            {
                return Command.Restart;
            }

            if (command.Equals("top"))
            {
                return Command.ShowTopScores;
            }

            if (command.Equals("exit"))
            {
                return Command.Exit;
            }

            if (command.Length < 3)
            {
                return Command.Invalid;
            }

            // TODO: split string and TryParse !!!!
            cellToOpen.Row = int.Parse(command[0].ToString());
            cellToOpen.Col = int.Parse(command[2].ToString());

            return Command.OpenCell;
        }
    }
}
