//-----------------------------------------------------------------------
// <copyright file="MinesweeperGame.cs" company="Telerik Academy">
//     Copyright (c) 2014 Telerik Academy. All rights reserved.
// </copyright>
// <summary> The 'receiver' class in the Command pattern.</summary>
//-----------------------------------------------------------------------
namespace Minesweeper.Game
{
    using System;
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

        /// <summary>Instance of the <see cref="Minesweeper.Game.UIManager"/> class.</summary>
        private readonly UIManager uiManager;

        /// <summary>Instance of the <see cref="Minesweeper.Game.Minefield"/> class.</summary>
        private Minefield minefield;

        /// <summary>The number of rows of the minefield.</summary>
        private int minefieldRows;
        
        /// <summary>The number of columns of the minefield.</summary>
        private int minefieldCols;

        /// <summary>
        /// Initializes a new instance of the <see cref="Minesweeper.Game.MinesweeperGame"/> class.
        /// </summary>
        public MinesweeperGame()
        {
            this.minefieldRows = 5;
            this.minefieldCols = 10;
            this.uiManager = new UIManager(new ConsoleRenderer(), new ConsoleReader());
            this.scoreBoard = new ScoreBoard();
            this.uiManager.DrawGameScreen(this.minefieldRows, this.minefieldCols);
            this.GenerateMinefield();
        }

        /// <summary>
        /// Opens the selected cellPosition in minefield.
        /// </summary>
        /// <param name="cellPosition">The position of a selected cell on the minefield.</param>
        public void OpenCell(ICellPosition cellPosition)
        {
            this.MinefieldInteractionHandler(cellPosition, this.minefield.OpenCellHandler);            
        }

        /// <summary>
        /// Flags the cellPosition on the given coordinates.
        /// </summary>
        /// <param name="cellPosition">The position of a selected cell on the minefield.</param>
        public void FlagCell(ICellPosition cellPosition)
        {
            this.MinefieldInteractionHandler(cellPosition, this.minefield.FlagCellHandler);            
        }

        /// <summary>
        /// When the mine explodes finishes the game and shows the appropriate message to the user.
        /// </summary>
        public void MineBoomed()
        {
            this.FinishGame(GameEndState.Fail);
        }

        /// <summary>
        /// Method for quitting the game.
        /// </summary>
        public void ExitGame()
        {
            // the caller of this method will stop the game
            this.uiManager.GameExit();
        }

        /// <summary>
        /// Shows the high scores of the game.
        /// </summary>
        public void ShowScores()
        {
            this.uiManager.DisplayHighScores(this.scoreBoard.TopScores);
        }

        /// <summary>
        /// Generates randomly mined minefield.
        /// </summary>
        public void GenerateMinefield()
        {
            // Create new minefield by Factory Method
            this.minefield = this.CreateMinefield(this.minefieldRows, this.minefieldCols);

            // Show minefield
            this.UpdateMinefield(false);
        }

        /// <summary>
        /// Handles error message if the user enters invalid command.
        /// </summary>
        public void HandleCommandError()
        {
            this.uiManager.HandleError(GameErrors.InvalidCommand);
        }

        /// <summary>
        /// Factory Method to create a new minefield.
        /// </summary>
        /// <param name="rows">Rows in the minefield.</param>
        /// <param name="cols">Columns in the minefield.</param>
        /// <returns>Returns a new minefield.</returns>
        protected abstract Minefield CreateMinefield(int rows, int cols);

        /// <summary>
        /// Handles minefield interaction - Open, Flag.
        /// </summary>
        /// <param name="cellPosition">Selected cell position.</param>
        /// <param name="handler">Handling function that returns CellActionResult.</param>
        private void MinefieldInteractionHandler(ICellPosition cellPosition, Func<ICellPosition, CellActionResult> handler)
        {
            var result = handler(cellPosition);

            switch (result)
            {
                case CellActionResult.OutOfRange:
                    this.uiManager.HandleError(GameErrors.CellOutOfRange);
                    break;
                case CellActionResult.AlreadyOpened:
                    this.uiManager.HandleError(GameErrors.CellAlreadyOpened);
                    break;
                case CellActionResult.Boom:
                    this.MineBoomed();
                    break;
                case CellActionResult.Normal:
                    this.UpdateGameStatus();
                    break;
                default:
                    throw new ArgumentException("Unknown switch key of type CellActionResult!");
            }
        }

        /// <summary>
        /// Requests update on changes in the minefield.
        /// </summary>
        /// <param name="showAll">Tells the method whether to show all the mines on the field.</param>
        private void UpdateMinefield(bool showAll)
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
                this.FinishGame(GameEndState.Success);
            }
            else
            {
                this.UpdateMinefield(false);
            }
        }

        /// <summary>
        /// Requests drawing of game end screen, asks player to enter name and adds it to the 
        /// scoreboard, requests scoreboard display and starts a new game.
        /// </summary>
        /// <param name="gameState">State of game end.</param>
        private void FinishGame(GameEndState gameState)
        {
            // A boomed mine does not have an OPEN state, so CountOpen() is correct
            int numberOfOpenedCells = this.minefield.OpenedCellsCount;

            this.UpdateMinefield(true);
            this.uiManager.DisplayEnd(gameState, numberOfOpenedCells);

            string name = this.uiManager.GetPlayerName();
            this.scoreBoard.AddScore(name, numberOfOpenedCells);
            this.ShowScores();

            // Start new game
            this.GenerateMinefield();
        }
    }
}
