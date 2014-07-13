namespace Minesweeper
{
    using System;
    using System.Collections.Generic;

    public class MinesweeperGame
    {
        private readonly SortedDictionary<int, string> topScores = new SortedDictionary<int, string>();
        private bool gameEnded = false;
        private ConsoleManager consoleManager;
        private Minefield minefield;

        public MinesweeperGame()
        {
        }

        public void Run()
        {
            int minefieldRows = 5;
            int minefieldCols = 10;

            minefield = new Minefield(minefieldRows, minefieldCols);
            consoleManager = new ConsoleManager(minefieldRows, minefieldCols);

            consoleManager.Intro();
            consoleManager.DrawInitialGameField();

            var commandReader = new CommandReader();
            while (!gameEnded)
            {
                Cell cellToOpen;
                var command = commandReader.ReadCommand(consoleManager, out cellToOpen);

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
                        OpenCell(cellToOpen);
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
                    MineBoomed();
                    break;
                case MinefieldState.Normal:
                    int neighborMinesCount = minefield.CountNeighborMines(cell);
                    consoleManager.DrawOpenCell(cell.Row, cell.Col, neighborMinesCount);
                    break;
                default:
                    break;
            }
        }

        private void MineBoomed()
        {
            // subtract the boomed mine that was opened
            int numberOfOpenedCells = minefield.CountOpen() - 1;

            consoleManager.DrawFinalGameField(minefield.Mines, minefield.OpenedCells);
            consoleManager.Finish(numberOfOpenedCells);

            string name = Console.ReadLine();
            topScores.Add(numberOfOpenedCells, name);
            consoleManager.DisplayHighScores(topScores);
            gameEnded = true;
        }
    }
}
