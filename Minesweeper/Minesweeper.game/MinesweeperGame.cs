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
        private IDictionary<UserMsg, string> userMessages;

        public MinesweeperGame()
        {
            this.InitializeMessages();
        }

        public void Run()
        {
            int minefieldRows = 5;
            int minefieldCols = 10;

            this.minefield = new Minefield(minefieldRows, minefieldCols);
            int cmdLineCol = this.userMessages[UserMsg.EnterRowCol].Length;
            this.consoleManager = new ConsoleManager(minefieldRows, minefieldCols, cmdLineCol);

            this.consoleManager.DisplayIntro(this.userMessages[UserMsg.Intro]);
            this.consoleManager.DrawInitialGameField(this.userMessages[UserMsg.EnterRowCol]);

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
                    this.consoleManager.WaitForKey(this.userMessages[UserMsg.PressAnyKey]);
                    break;
                case MinefieldState.AlreadyOpened:
                    this.consoleManager.DisplayError(this.errorMessages[ErrorType.AlreadyOpened]);
                    this.consoleManager.WaitForKey(this.userMessages[UserMsg.PressAnyKey]);
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
            this.consoleManager.DisplayEnd(this.userMessages[UserMsg.Boom], numberOfOpenedCells);

            string name = this.consoleManager.ReadName();
            this.topScores.Add(numberOfOpenedCells, name);
            this.consoleManager.DisplayHighScores(this.topScores);
            this.consoleManager.GoodBye(this.userMessages[UserMsg.Bye]);
            this.gameEnded = true;
        }

        private void InitializeMessages()
        {
            this.errorMessages = new Dictionary<ErrorType, string>()
            {
                { ErrorType.IvalidCommand, "Ivalid command!" },
                { ErrorType.AlreadyOpened, "Cell already opened!" },
                { ErrorType.CellOutOfRange, "Cell is out of range of the minefield!"}
            };

            this.userMessages = new Dictionary<UserMsg, string>()
            {
                { UserMsg.PressAnyKey, "Press any key to continue."},
                { UserMsg.EnterRowCol, "Enter row and column: " },
                { UserMsg.Intro, "Welcome to the game “Minesweeper”.\nTry to open all cells without mines. Use 'top' to view the scoreboard,\n'restart' to start a new game and 'exit' to quit the game." },
                { UserMsg.Boom, "Booooom! You were killed by a mine. You opened {0} cells without mines.\nPlease enter your name for the top scoreboard: "},
                { UserMsg.Bye, "Good bye!" }
            };
        }
    }
}
