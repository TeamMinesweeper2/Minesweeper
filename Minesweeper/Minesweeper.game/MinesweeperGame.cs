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
        private IDictionary<ErrorType, string> errorMessages;
        private IDictionary<string, string> userMessages;

        public MinesweeperGame()
        {
            this.InitializeMessages();
        }

        private void InitializeMessages()
        {
            this.errorMessages = new Dictionary<ErrorType, string>()
            {
                { ErrorType.IvalidCommand, "Ivalid command!" },
                { ErrorType.AlreadyOpened, "Cell already opened!" },
                { ErrorType.CellOutOfRange, "Cell is out of range of the minefield!"}
            };

            this.userMessages = new Dictionary<string, string>()
            {
                { "PressAnyKey", "Press any key to continue."},
                { "EnterRowCol", "Enter row and column: " }
            };
        }

        public void Run()
        {
            int minefieldRows = 5;
            int minefieldCols = 10;

            this.minefield = new Minefield(minefieldRows, minefieldCols);
            int cmdLineCol = this.userMessages["EnterRowCol"].Length;
            this.consoleManager = new ConsoleManager(minefieldRows, minefieldCols, cmdLineCol);

            this.consoleManager.Intro();
            this.consoleManager.DrawInitialGameField(this.userMessages["EnterRowCol"]);

            var commandReader = new CommandReader();
            while (!this.gameEnded)
            {
                CellPos cellToOpen;
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
                        this.consoleManager.DisplayError(this.errorMessages[ErrorType.IvalidCommand]);
                        break;
                    case Command.OpenCell:
                        this.OpenCell(cellToOpen);
                        break;
                    default:
                        throw new ArgumentException("Unrecognized command!");
                }
            }
        }

        private void OpenCell(CellPos cell)
        {
            var result = this.minefield.OpenNewCell(cell);

            switch (result)
            {
                case MinefieldState.OutOfRange:
                    this.consoleManager.DisplayError(this.errorMessages[ErrorType.CellOutOfRange]);
                    this.consoleManager.WaitForKey(this.userMessages["PressAnyKey"]);
                    break;
                case MinefieldState.AlreadyOpened:
                    this.consoleManager.DisplayError(this.errorMessages[ErrorType.AlreadyOpened]);
                    this.consoleManager.WaitForKey(this.userMessages["PressAnyKey"]);
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
