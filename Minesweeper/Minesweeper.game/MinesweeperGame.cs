namespace Minesweeper
{
    using System;
    using System.Collections.Generic;

    public class MinesweeperGame
    {
        private SortedDictionary<int, string> topScores = new SortedDictionary<int, string>();
        private bool gameEnded = false;
        private ConsoleManager consoleManager;
        private Minefield minefield;

        public void Run()
        {
            minefield = new Minefield();

            consoleManager = new ConsoleManager();
            consoleManager.Intro();
            consoleManager.DrawInitialGameField();

            var commandReader = new CommandReader();
            while (!gameEnded)
            {
                var command = commandReader.ReadCommand(consoleManager);

                switch (command)
                {
                    case Command.Restart:
                        break;
                    case Command.ShowTopScores:
                        consoleManager.DisplayHighScores(topScores);
                        break;
                    case Command.Exit:
                        break;
                    case Command.Invalid:
                        consoleManager.ErrorMessage(ErrorType.IvalidCommand);
                        break;
                    case Command.OpenCell:
                        var cell = commandReader.GetCellToOpen();
                        OpenCell(cell);
                        break;
                    default:
                        throw new ArgumentException("Unrecognized command!");
                }                
            }

            Console.WriteLine("Good Bye");
        }

        private void OpenCell(Cell cell)
        {
            var result = minefield.OpenNewCell(cell);

            switch (result)
            {
                case MinefieldState.OutOfRange:
                    consoleManager.ErrorMessage(ErrorType.CellOutOfRange);
                    break;
                case MinefieldState.AlreadyOpened:
                    consoleManager.ErrorMessage(ErrorType.AlreadyOpened);
                    break;
                case MinefieldState.Boom:
                    minefield.MineBoomed(consoleManager);
                    gameEnded = true;
                    break;
                case MinefieldState.Normal:
                    minefield.EmptyCellOpened(cell, consoleManager);
                    break;
                default:
                    break;
            }
        }
    }
}
