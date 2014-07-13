namespace Minesweeper
{
    class CommandReader
    {
        private Cell cellToOpen;

        public CommandReader()
        {
            this.cellToOpen = Cell.Empty;
        }

        public Command ReadCommand(ConsoleManager consoleManager)
        {
            string command = consoleManager.CommandInput();

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

            this.cellToOpen.Row = int.Parse(command[0].ToString());
            this.cellToOpen.Col = int.Parse(command[2].ToString());

            return Command.OpenCell;
        }

        public Cell GetCellToOpen()
        {
            return cellToOpen;
        }
    }
}
