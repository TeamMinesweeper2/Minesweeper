namespace Minesweeper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Minesweeper.Lib;

    // The 'Receiver' class
    public class MinesweeperGame
    {
        private readonly List<KeyValuePair<string, int>> topScores = new List<KeyValuePair<string, int>>();
        private UIManager uiManager;
        private Minefield minefield;
        private IDictionary<ErrorType, string> errorMessages;
        private IDictionary<UserMsg, string> userMessages;

        private const float MinesCountCoeficient = 0.3F;

        private int minefieldRows;
        private int minefieldCols;

        public MinesweeperGame()
        {
            this.minefieldRows = 5;
            this.minefieldCols = 10;
            this.InitializeMessages();
            int cmdLineCol = this.userMessages[UserMsg.EnterRowCol].Length;
            this.uiManager = new UIManager(this.minefieldRows, this.minefieldCols, cmdLineCol);

            // Show game
            this.uiManager.DisplayIntro(this.userMessages[UserMsg.Intro]);
            this.uiManager.DrawTable(this.userMessages[UserMsg.EnterRowCol]);
            this.GenerateMinefield();
        }

        public void OpenCell(CellPos cell)
        {
            var result = this.minefield.OpenNewCell(cell);

            switch (result)
            {
                case MinefieldState.OutOfRange:
                    this.uiManager.DisplayError(this.errorMessages[ErrorType.CellOutOfRange]);
                    break;
                case MinefieldState.AlreadyOpened:
                    this.uiManager.DisplayError(this.errorMessages[ErrorType.AlreadyOpened]);
                    break;
                case MinefieldState.Boom:
                    this.MineBoomed();
                    break;
                case MinefieldState.Normal:
                    // TODO: end game when all cells are opened !!!
                    this.RedrawMinefield(false);
                    break;
                default:
                    break;
            }

            this.uiManager.ClearCommandLine();
        }

        public void FlagCell(CellPos cell)
        {
            var result = this.minefield.FlagCell(cell);

            switch (result)
            {
                case MinefieldState.OutOfRange:
                    this.uiManager.DisplayError(this.errorMessages[ErrorType.CellOutOfRange]);
                    break;
                case MinefieldState.AlreadyOpened:
                    this.uiManager.DisplayError(this.errorMessages[ErrorType.AlreadyOpened]);
                    break;
                case MinefieldState.Normal:
                    this.RedrawMinefield(false);
                    break;
                default:
                    break;
            }

            this.uiManager.ClearCommandLine();
        }

        public void MineBoomed()
        {
            // The boomed mine does not have an OPEN state, so CountOpen() is correct
            int numberOfOpenedCells = this.minefield.CountOpen();

            this.RedrawMinefield(true);
            this.uiManager.DisplayEnd(this.userMessages[UserMsg.Boom], numberOfOpenedCells);

            string name = this.uiManager.ReadName();
            this.topScores.Add(new KeyValuePair<string, int>(name, numberOfOpenedCells));

            this.ShowScores();

            //this.EndGame();
            this.GenerateMinefield();

            this.uiManager.ClearCommandLine();
        }

        public void EndGame()
        {
            this.uiManager.GoodBye(this.userMessages[UserMsg.Bye]);
            //this.gameEnded = true;
        }

        public void ShowScores()
        {
            var sorted = topScores.OrderBy(kvp => -kvp.Value);
            this.uiManager.DisplayHighScores(sorted);
            this.uiManager.ClearCommandLine();
        }

        public void GenerateMinefield()
        {
            // Create new minefield
            int minesCount = (int)(this.minefieldRows * this.minefieldCols * MinesCountCoeficient);
            var randomNumberProvider = RandomGeneratorProvider.GetInstance();
            this.minefield = new Minefield(this.minefieldRows, this.minefieldCols, minesCount, randomNumberProvider);

            // Show minefield
            this.RedrawMinefield(false);
            this.uiManager.ClearCommandLine();
        }

        public void DisplayError()
        {
            this.uiManager.DisplayError(this.errorMessages[ErrorType.IvalidCommand]);
            this.uiManager.ClearCommandLine();
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
                { UserMsg.Intro, "Welcome to the game “Minesweeper”.\nTry to open all cells without mines. Use 'top' to view the scoreboard,\n'restart' to start a new game and 'exit' to quit the game. Use 'm' to flag a cell.\n" },
                { UserMsg.Boom, "Booooom! You were killed by a mine. You opened {0} cells without mines.\nPlease enter your name for the top scoreboard: "},
                { UserMsg.Bye, "Good bye!" }
            };
        }

        private void RedrawMinefield(bool showAll)
        {
            var minefield = this.minefield.GetImage(showAll);
            var neighborMines = this.minefield.AllNeighborMines;
            this.uiManager.DrawGameField(minefield, neighborMines);
        }
    }
}
