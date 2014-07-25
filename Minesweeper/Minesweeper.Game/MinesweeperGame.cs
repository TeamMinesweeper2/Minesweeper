//-----------------------------------------------------------------------
// <copyright file="MinesweeperGame.cs" company="Telerik Academy">
//     Copyright (c) 2014 Telerik Academy. All rights reserved.
// </copyright>
// <summary>Main game logic.</summary>
//-----------------------------------------------------------------------

namespace Minesweeper.Game
{
    using Minesweeper.Lib;

    /// <summary>
    /// The 'receiver' class in the Command pattern.
    /// Also a Facade for the Minefield, UIManager and ScoreBoard class.
    /// Uses Factory Method to create the minefield.
    /// </summary>
    public abstract class MinesweeperGame
    {
        /// <summary>Instance of the <see cref="Minesweeper.Game.ScoreBoard"/> class.</summary>
        private readonly ScoreBoard scoreBoard;

        /// <summary>Instance of the <see cref="Minesweeper.Game.IUIManager"/> class.</summary>
        private readonly IUIManager uiManager;

        /// <summary>The number of rows of the minefield.</summary>
        private readonly int minefieldRows;
        
        /// <summary>The number of columns of the minefield.</summary>
        private readonly int minefieldCols;

        /// <summary>The message which is going to prompt the user of the expected input.</summary>
        private readonly string prompt;

        /// <summary>Instance of the <see cref="Minesweeper.Game.Minefield"/> class.</summary>
        private Minefield minefield;

        /// <summary>
        /// Initializes a new instance of the <see cref="Minesweeper.Game.MinesweeperGame"/> class.
        /// </summary>
        /// <param name="uiManager">The <see cref="Minesweeper.Game.IUIManager"/> implementation used to read and write.</param>
        public MinesweeperGame(IUIManager uiManager)
        {
            this.minefieldRows = 5;
            this.minefieldCols = 10;
            this.uiManager = uiManager;

            this.prompt = Messages.EnterRowCol;
            this.scoreBoard = new ScoreBoard();

            // Show game
            this.uiManager.DisplayIntro(Messages.Intro);
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
                case CellActionResult.OutOfRange:
                    this.uiManager.DisplayError(Messages.CellOutOfRange);
                    break;
                case CellActionResult.AlreadyOpened:
                    this.uiManager.DisplayError(Messages.AlreadyOpened);
                    break;
                case CellActionResult.Boom:
                    this.MineBoomed();
                    break;
                case CellActionResult.Normal:
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
                case CellActionResult.OutOfRange:
                    this.uiManager.DisplayError(Messages.CellOutOfRange);
                    break;
                case CellActionResult.AlreadyOpened:
                    this.uiManager.DisplayError(Messages.AlreadyOpened);
                    break;
                case CellActionResult.Normal:
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
            this.FinishGame(Messages.Boom);
        }

        /// <summary>
        /// Method for quitting the game.
        /// </summary>
        public void ExitGame()
        {
            // the caller of this method will stop the game
            this.uiManager.GoodBye(Messages.Bye);
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
            // Create new minefield by Factory Method
            this.minefield = this.CreateMinefield(this.minefieldRows, this.minefieldCols);

            // Show minefield
            this.RedrawMinefield(false);
            this.uiManager.ClearCommandLine(this.prompt);
        }

        /// <summary>
        /// Displays error message if the user enters invalid command.
        /// </summary>
        public void DisplayError()
        {
            this.uiManager.DisplayError(Messages.IvalidCommand);
            this.uiManager.ClearCommandLine(this.prompt);
        }

        /// <summary>
        /// Factory Method to create a new minefield.
        /// </summary>
        /// <param name="rows">Rows in the minefield.</param>
        /// <param name="cols">Columns in the minefield.</param>
        /// <returns>Returns a new minefield.</returns>
        protected abstract Minefield CreateMinefield(int rows, int cols);

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
                this.FinishGame(Messages.Success);
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
        private void FinishGame(string msg)
        {
            // A boomed mine does not have an OPEN state, so CountOpen() is correct
            int numberOfOpenedCells = this.minefield.OpenedCellsCount;

            this.RedrawMinefield(true);
            this.uiManager.DisplayEnd(msg, numberOfOpenedCells);

            string name = this.uiManager.ReadInput();
            this.scoreBoard.AddScore(name, numberOfOpenedCells);
            this.ShowScores();

            // Start new game
            this.GenerateMinefield();
            this.uiManager.ClearCommandLine(this.prompt);
        }
    }
}