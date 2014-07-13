namespace Minesweeper
{
    internal class CommandReader
    {
        //// TODO: add constants for all commands ("top", "restart"...) - DONE
        private const string Restart = "restart";
        private const string Top = "top";
        private const string Exit = "exit";
        private const int MinCommandLength = 3;

        public CommandReader()
        {
        }

        public Command ReadCommand(IUserInputReader consoleManager, out CellPos cellToOpen)
        {
            string command = consoleManager.ReadInput();
            cellToOpen = CellPos.Empty;

            if (command.Equals(Restart))
            {
                return Command.Restart;
            }

            if (command.Equals(Top))
            {
                return Command.ShowTopScores;
            }

            if (command.Equals(Exit))
            {
                return Command.Exit;
            }

            if (command.Length < MinCommandLength)
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
