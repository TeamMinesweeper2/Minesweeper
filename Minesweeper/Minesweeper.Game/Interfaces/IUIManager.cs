//-----------------------------------------------------------------------
// <copyright file="IUIManager.cs" company="Telerik Academy">
//     Copyright (c) 2014 Telerik Academy. All rights reserved.
// </copyright>
// <summary> Defines the interface for UI Manager.</summary>
//-----------------------------------------------------------------------
namespace Minesweeper.Game.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Minesweeper.Game.Enums;

    /// <summary>
    /// User Interface Manager interface.
    /// </summary>
    public interface IUIManager
    {
        /// <summary>Triggers 'Reset' command event.</summary>
        event CommandEventHandler ResetCommandEvent;

        /// <summary>Triggers 'Boom' command event.</summary>
        event CommandEventHandler BoomCommandEvent;

        /// <summary>Triggers 'Exit' command event.</summary>
        event CommandEventHandler ExtiCommandEvent;

        /// <summary>Triggers 'FlagCell' command event.</summary>
        event CommandEventHandler FlagCellCommandEvent;

        /// <summary>Triggers 'Invalid command' command event.</summary>
        event CommandEventHandler InvalidCommandEvent;

        /// <summary>Triggers 'Open cell' command event.</summary>
        event CommandEventHandler OpenCellCommandEvent;

        /// <summary>Triggers 'Show high scores' command event.</summary>
        event CommandEventHandler ShowHighScoresCommandEvent;

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
        /// Handles error messages from entered commands execution.
        /// </summary>
        /// <param name="gameError">The error to be handled.</param>
        void HandleError(GameErrors gameError);

        /// <summary>
        /// Reads the player name.
        /// </summary>
        /// <returns>The player name.</returns>
        string GetPlayerName();

        /// <summary>
        /// Waits for command to be triggered.
        /// </summary>
        void WaitForCommand();

        /// <summary>
        /// Draws the game field.
        /// </summary>
        /// <param name="minefield">The minefield to be drawn.</param>
        /// <param name="neighborMines">The minefield with all values of neighboring mines.</param>
        void DrawGameField(CellImage[,] minefield, int[,] neighborMines);
    }
}