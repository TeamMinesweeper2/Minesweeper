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

            this.minefield = new Minefield(minefieldRows, minefieldCols);
            this.consoleManager = new ConsoleManager(minefieldRows, minefieldCols);

            this.consoleManager.Intro();
            this.consoleManager.DrawInitialGameField();

            var commandReader = new CommandReader();
            while (!this.gameEnded)
            {
                Cell cellToOpen;
                var command = commandReader.ReadCommand(this.consoleManager, out cellToOpen);

                switch (command)
                {
                    case Command.Restart:
                        break;
                    case Command.ShowTopScores:
                        this.consoleManager.DisplayHighScores(this.topScores);
                        break;
                    case Command.Exit:
                        break;
                    case Command.Invalid:
                        this.consoleManager.ErrorMessage(ErrorType.IvalidCommand);
                        break;
                    case Command.OpenCell:
                        this.OpenCell(cellToOpen);
                        break;
                    default:
                        throw new ArgumentException("Unrecognized command!");
                }
            }            
        }

        private void OpenCell(Cell cell)
        {
            var result = this.minefield.OpenNewCell(cell);

            switch (result)
            {
                case MinefieldState.OutOfRange:
                    this.consoleManager.ErrorMessage(ErrorType.CellOutOfRange);
                    break;
                case MinefieldState.AlreadyOpened:
                    this.consoleManager.ErrorMessage(ErrorType.AlreadyOpened);
                    break;
                case MinefieldState.Boom:
                    this.MineBoomed();
                    break;
                case MinefieldState.Normal:
                    int neighborMinesCount = this.minefield.CountNeighborMines(cell);
                    this.consoleManager.DrawOpenCell(cell.Row, cell.Col, neighborMinesCount);
                    break;
                default:
                    break;
            }
        }

        private void MineBoomed()
        {
            // subtract the boomed mine that was opened
            int numberOfOpenedCells = this.minefield.CountOpen() - 1;

            this.consoleManager.DrawFinalGameField(this.minefield.Mines, this.minefield.OpenedCells);
            this.consoleManager.Finish(numberOfOpenedCells);

            string name = Console.ReadLine();
            this.topScores.Add(numberOfOpenedCells, name);
            this.consoleManager.DisplayHighScores(this.topScores);
            this.consoleManager.GoodBye();
            this.gameEnded = true;
        }
    }
}
