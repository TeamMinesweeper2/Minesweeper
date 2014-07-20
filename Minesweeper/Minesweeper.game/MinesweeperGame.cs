namespace Minesweeper.Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Minesweeper.Lib;

    // The 'Receiver' class
    public class MinesweeperGame
    {
        private List<KeyValuePair<string, int>> topScores = new List<KeyValuePair<string, int>>();
        private UIManager uiManager;
        private Minefield minefield;
        private IDictionary<ErrorType, string> errorMessages;
        private IDictionary<UserMsg, string> userMessages;

        private const decimal MinesCountCoeficient = 0.16m; // 0.2m -> 10 mines; 0.16m -> 8 mines

        private int minefieldRows;
        private int minefieldCols;
        private string prompt;

        public MinesweeperGame()
        {
            this.minefieldRows = 5;
            this.minefieldCols = 10;
            this.uiManager = new UIManager(new ConsoleRenderer(), new ConsoleReader());

            this.InitializeMessages();

            // Show game
            this.uiManager.DisplayIntro(this.userMessages[UserMsg.Intro]);
            this.uiManager.DrawTable(this.minefieldRows, this.minefieldCols);
            this.GenerateMinefield();
        }

        public void OpenCell(ICellPosition cell)
        {
            var result = this.minefield.OpenCellHandler(cell);

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
                    this.UpdateGameStatus();                  
                    break;
                default:
                    break;
            }

            this.uiManager.ClearCommandLine(this.prompt);
        }

        public void FlagCell(ICellPosition cell)
        {
            var result = this.minefield.FlagCellHandler(cell);

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

            this.uiManager.ClearCommandLine(this.prompt);
        }

        public void MineBoomed()
        {
            this.FinishGame(UserMsg.Boom);
        }

        public void ExitGame()
        {
            this.uiManager.GoodBye(this.userMessages[UserMsg.Bye]);
            // the caller of this method will stop the game
        }

        public void ShowScores()
        {
            this.uiManager.DisplayHighScores(this.topScores);
            this.uiManager.ClearCommandLine(this.prompt);
        }

        public void GenerateMinefield()
        {
            // Create new minefield
            int minesCount = (int)(this.minefieldRows * this.minefieldCols * MinesCountCoeficient);
            var randomNumberProvider = RandomGeneratorProvider.GetInstance();
            this.minefield = new Minefield(this.minefieldRows, this.minefieldCols, minesCount, randomNumberProvider);

            // Show minefield
            this.RedrawMinefield(false);
            this.uiManager.ClearCommandLine(this.prompt);
        }

        public void DisplayError()
        {
            this.uiManager.DisplayError(this.errorMessages[ErrorType.IvalidCommand]);
            this.uiManager.ClearCommandLine(this.prompt);
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
                { UserMsg.Bye, "Good bye!" },
                { UserMsg.Success, "Success! You opened all cells without mines.\nPlease enter your name for the top scoreboard: " }
            };

            this.prompt = this.userMessages[UserMsg.EnterRowCol];
        }

        private void RedrawMinefield(bool showAll)
        {
            var minefield = this.minefield.GetImage(showAll);
            var neighborMines = this.minefield.AllNeighborMines;
            this.uiManager.DrawGameField(minefield, neighborMines);
        }

        private void UpdateGameStatus()
        {
            if (this.minefield.IsDisarmed())
            {
                // End game when all cells without mines are opened
                this.FinishGame(UserMsg.Success);
            }
            else
            {
                this.RedrawMinefield(false);
            }
        }

        private void FinishGame(UserMsg msg)
        {
            // A boomed mine does not have an OPEN state, so CountOpen() is correct
            int numberOfOpenedCells = this.minefield.GetOpenedCells;

            this.RedrawMinefield(true);
            this.uiManager.DisplayEnd(this.userMessages[msg], numberOfOpenedCells);

            string name = this.uiManager.ReadName();
            this.AddScore(numberOfOpenedCells, name);
            this.ShowScores();

            // Start new game
            this.GenerateMinefield();
            this.uiManager.ClearCommandLine(this.prompt);
        }

        private void AddScore(int numberOfOpenedCells, string name)
        {
            this.topScores.Add(new KeyValuePair<string, int>(name, numberOfOpenedCells));
            // Limit the scoreboard to only the top five players by score
            this.topScores = this.topScores.OrderBy(kvp => -kvp.Value).Take(5).ToList();
        }
    }
}
