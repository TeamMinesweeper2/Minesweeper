namespace Minesweeper.Game
{
    using System.Collections.Generic;
    using Minesweeper.Lib;

    /// <summary>
    /// The 'receiver' class in the Command pattern.
    /// Also a Facade for the Minefield, UIManager and ScoreBoard class.
    /// </summary>
    public class MinesweeperGame
    {
        /// <summary>The coefficient of how many mines to be placed on the minefield.</summary>
        private const decimal MinesCountCoeficient = 0.16m; // 0.2m -> 10 mines; 0.16m -> 8 mines

        /// <summary>Instance of the <see cref="Minesweeper.Game.ScoreBoard"/> class.</summary>
        private readonly ScoreBoard scoreBoard;        

        /// <summary>Instance of the <see cref="Minesweeper.Game.UIManager"/> class.</summary>
        private readonly UIManager uiManager;

        /// <summary>Instance of the <see cref="Minesweeper.Game.Minefield"/> class.</summary>
        private Minefield minefield;

        /// <summary>Collection of key value pair of error messages.</summary>
        private IDictionary<ErrorType, string> errorMessages;

        /// <summary>Collection of key value pair of user messages.</summary>
        private IDictionary<UserMsg, string> userMessages;

        /// <summary>The number of rows of the minefield.</summary>
        private int minefieldRows;
        
        /// <summary>The number of columns of the minefield.</summary>
        private int minefieldCols;

        /// <summary>The message which is going to prompt the user of the expected input.</summary>
        private string prompt;

        /// <summary>
        /// Initializes a new instance of the <see cref="Minesweeper.Game.MinesweeperGame"/> class.
        /// </summary>
        public MinesweeperGame()
        {
            this.minefieldRows = 5;
            this.minefieldCols = 10;
            this.uiManager = new UIManager(new ConsoleRenderer(), new ConsoleReader());

            this.InitializeMessages();
            this.scoreBoard = new ScoreBoard();

            // Show game
            this.uiManager.DisplayIntro(this.userMessages[UserMsg.Intro]);
            this.uiManager.DrawTable(this.minefieldRows, this.minefieldCols);
            this.GenerateMinefield();
        }

        /// <summary>
        /// Opens the selected <see cref="Cell"/>
        /// </summary>
        /// <param name="cell">The position of the cell on the minefield.</param>
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

        /// <summary>
        /// Flags the cell on the given coordinates.
        /// </summary>
        /// <param name="cell">The position of the cell.</param>
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

        /// <summary>
        /// When the mine explodes finishes the game and shows the appropriate message to the user.
        /// </summary>
        public void MineBoomed()
        {
            this.FinishGame(UserMsg.Boom);
        }

        /// <summary>
        /// Method for quitting the game.
        /// </summary>
        public void ExitGame()
        {
            // the caller of this method will stop the game
            this.uiManager.GoodBye(this.userMessages[UserMsg.Bye]);
        }

        /// <summary>
        /// Shows the high scores of the game.
        /// </summary>
        public void ShowScores()
        {
            this.uiManager.DisplayHighScores(this.scoreBoard.TopScores);
            this.uiManager.ClearCommandLine(this.prompt);
        }

        /// <summary>
        /// Generates randomly mined minefield.
        /// </summary>
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

        /// <summary>
        /// Displays error message if the user enters invalid command.
        /// </summary>
        public void DisplayError()
        {
            this.uiManager.DisplayError(this.errorMessages[ErrorType.IvalidCommand]);
            this.uiManager.ClearCommandLine(this.prompt);
        }

        /// <summary>
        /// Initializes the available messages. 
        /// </summary>
        private void InitializeMessages()
        {
            this.errorMessages = new Dictionary<ErrorType, string>()
            {
                { ErrorType.IvalidCommand, "Ivalid command!" },
                { ErrorType.AlreadyOpened, "Cell already opened!" },
                { ErrorType.CellOutOfRange, "Cell is out of range of the minefield!" }
            };

            this.userMessages = new Dictionary<UserMsg, string>()
            {
                { UserMsg.PressAnyKey, "Press any key to continue." },
                { UserMsg.EnterRowCol, "Enter row and column: " },
                { UserMsg.Intro, "Welcome to the game “Minesweeper”.\nTry to open all cells without mines. Use 'top' to view the scoreboard,\n'restart' to start a new game and 'exit' to quit the game. Use 'm' to flag a cell.\n" },
                { UserMsg.Boom, "Booooom! You were killed by a mine. You opened {0} cells without mines.\nPlease enter your name for the top scoreboard: " },
                { UserMsg.Bye, "Good bye!" },
                { UserMsg.Success, "Success! You opened all cells without mines.\nPlease enter your name for the top scoreboard: " }
            };

            this.prompt = this.userMessages[UserMsg.EnterRowCol];
        }

        /// <summary>
        /// Redraws the minefield.
        /// </summary>
        /// <param name="showAll">Tells the method whether to show all the mines on the field.</param>
        private void RedrawMinefield(bool showAll)
        {
            var minefield = this.minefield.GetImage(showAll);
            var neighborMines = this.minefield.AllNeighborMines;
            this.uiManager.DrawGameField(minefield, neighborMines);
        }

        /// <summary>
        /// Updates the current game status. End game when all cells without mines are opened.
        /// </summary>
        private void UpdateGameStatus()
        {
            if (this.minefield.IsDisarmed())
            {
                this.FinishGame(UserMsg.Success);
            }
            else
            {
                this.RedrawMinefield(false);
            }
        }

        /// <summary>
        /// Finishes the current game.
        /// </summary>
        /// <param name="msg">The message to be displayed to the user after the game finishes.</param>
        private void FinishGame(UserMsg msg)
        {
            // A boomed mine does not have an OPEN state, so CountOpen() is correct
            int numberOfOpenedCells = this.minefield.GetOpenedCells;

            this.RedrawMinefield(true);
            this.uiManager.DisplayEnd(this.userMessages[msg], numberOfOpenedCells);

            string name = this.uiManager.ReadName();
            this.scoreBoard.AddScore(name, numberOfOpenedCells);
            this.ShowScores();

            // Start new game
            this.GenerateMinefield();
            this.uiManager.ClearCommandLine(this.prompt);
        }
    }
}
