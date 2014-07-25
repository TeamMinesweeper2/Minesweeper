//-----------------------------------------------------------------------
// <copyright file="IUIManager.cs" company="Telerik Academy">
//     Copyright (c) 2014 Telerik Academy. All rights reserved.
// </copyright>
// <summary>Defines UI Manager for the game.</summary>
//-----------------------------------------------------------------------

namespace Minesweeper.Game
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines methods for reading and writing.
    /// </summary>
    public interface IUIManager
    {
        /// <summary>
        /// Clears the command line.
        /// </summary>
        /// <param name="commandPrompt">The prompt that is going to be shown to the user.</param>
        void ClearCommandLine(string commandPrompt);

        /// <summary>
        /// Displays the ending messages of the game.
        /// </summary>
        /// <param name="msg">The ending message.</param>
        /// <param name="numberOfOpenedCells">The number of cells the user opened during the game.</param>
        void DisplayEnd(string msg, int numberOfOpenedCells);

        /// <summary>
        /// Displays error messages.
        /// </summary>
        /// <param name="errorMsg">The error message to be displayed.</param>
        void DisplayError(string errorMsg);

        /// <summary>
        /// Displays the high scores.
        /// </summary>
        /// <param name="topScores">The top scores to be displayed.</param>
        void DisplayHighScores(IEnumerable<KeyValuePair<string, int>> topScores);

        /// <summary>
        /// Displays the introduction text of the game.
        /// </summary>
        /// <param name="msg">The introduction message.</param>
        void DisplayIntro(string msg);

        /// <summary>
        /// Draws the game field.
        /// </summary>
        /// <param name="minefield">The minefield to be drawn.</param>
        /// <param name="neighborMines">The minefield with all values of neighboring mines.</param>
        void DrawGameField(CellImage[,] minefield, int[,] neighborMines);

        /// <summary>
        /// Draws the game table.
        /// </summary>
        /// <param name="mineFieldRows">The count of the minefield rows.</param>
        /// <param name="minefieldCols">The count of the minefield columns.</param>
        void DrawTable(int mineFieldRows, int minefieldCols);

        /// <summary>
        /// Displays the messages when the user quits the game.
        /// </summary>
        /// <param name="goodByeMsg">The goodbye message.</param>
        void GoodBye(string goodByeMsg);

        /// <summary>
        /// Reads input from the user.
        /// </summary>
        /// <returns>The user input.</returns>
        string ReadInput();
    }
}