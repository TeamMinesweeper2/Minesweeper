//-----------------------------------------------------------------------
// <copyright file="IUIManager.cs" company="Telerik Academy">
//     Copyright (c) 2014 Telerik Academy. All rights reserved.
// </copyright>
// <summary> Defines the interface for UI Manager.</summary>
//-----------------------------------------------------------------------
namespace Minesweeper.Game
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// User Interface Manager interface.
    /// </summary>
    public interface IUIManager
    {
        /// <summary>
        /// Draws the game screen.
        /// </summary>
        /// <param name="numberOfRows">Number of rows of the minefield.</param>
        /// <param name="numberOfColumns">Number of columns of the minefield.</param>
        void DrawGameScreen(int numberOfRows, int numberOfColumns);

        /// <summary>
        /// Displays the ending messages of the game.
        /// </summary>
        /// <param name="gameState">State of the game end.</param>
        /// <param name="numberOfOpenedCells">The number of cells the user opened during the game.</param>
        void DisplayEnd(GameEndState gameState, int numberOfOpenedCells);

        /// <summary>
        /// Displays the messages when the user quits the game.
        /// </summary>
        void GameExit();

        /// <summary>
        /// Displays the high scores.
        /// </summary>
        /// <param name="topScores">The top scores to be displayed.</param>
        void DisplayHighScores(IEnumerable<KeyValuePair<string, int>> topScores);

        /// <summary>
        /// Displays error messages.
        /// </summary>
        /// <param name="gameError">The error to be displayed.</param>
        void HandleError(GameErrors gameError);

        /// <summary>
        /// Reads the player name.
        /// </summary>
        /// <returns>The player name.</returns>
        string GetPlayerName();

        /// <summary>
        /// Draws the game field.
        /// </summary>
        /// <param name="minefield">The minefield to be drawn.</param>
        /// <param name="neighborMines">The minefield with all values of neighboring mines.</param>
        void DrawGameField(CellImage[,] minefield, int[,] neighborMines);
    }
}