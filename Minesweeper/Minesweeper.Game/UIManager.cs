//-----------------------------------------------------------------------
// <copyright file="UIManager.cs" company="Telerik Academy">
//     Copyright (c) 2014 Telerik Academy. All rights reserved.
// </copyright>
// <summary> Implementation of User Interface Manager contract.</summary>
//-----------------------------------------------------------------------
namespace Minesweeper.Game
{
    using System;
    using System.Collections.Generic;
    using Minesweeper.Game.Enums;
    using Minesweeper.Game.Interfaces;
    using Minesweeper.Lib.Interfaces;

    /// <summary>
    /// User Interface Manager class.
    /// </summary>
    public class UIManager : IUIManager
    {
        /// <summary>Bridged UI Manager implementation.</summary>
        private readonly IUIManagerBridge implementer = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="UIManager"/> class with default ConsoleRenderer and ConsoleReader.
        /// </summary>
        /// <param name="implementer">Concrete implementation of IUIManagerBridge.</param>
        public UIManager(IUIManagerBridge implementer)
        {
            this.implementer = implementer;

            // Register events from the implementer to be retriggered to the game.
            this.implementer.BoomCommandEvent += new CommandEventHandler(this.OnBoomCommandEvent);
            this.implementer.ExitCommandEvent += new CommandEventHandler(this.OnExtiCommandEvent);
            this.implementer.FlagCellCommandEvent += new CommandEventHandler(this.OnFlagCellCommandEvent);
            this.implementer.InvalidCommandEvent += new CommandEventHandler(this.OnInvalidCommandEvent);
            this.implementer.OpenCellCommandEvent += new CommandEventHandler(this.OnOpenCellCommandEvent);
            this.implementer.ResetCommandEvent += new CommandEventHandler(this.OnResetCommandEvent);
            this.implementer.ShowHighScoresCommandEvent += new CommandEventHandler(this.OnShowHighScoresCommandEvent);
        }

        /// <summary>Triggers 'Reset' command event.</summary>
        public event CommandEventHandler ResetCommandEvent;

        /// <summary>Triggers 'Boom' command event.</summary>
        public event CommandEventHandler BoomCommandEvent;

        /// <summary>Triggers 'Exit' command event.</summary>
        public event CommandEventHandler ExtiCommandEvent;

        /// <summary>Triggers 'FlagCell' command event.</summary>
        public event CommandEventHandler FlagCellCommandEvent;

        /// <summary>Triggers 'Invalid command' command event.</summary>
        public event CommandEventHandler InvalidCommandEvent;

        /// <summary>Triggers 'Open cell' command event.</summary>
        public event CommandEventHandler OpenCellCommandEvent;

        /// <summary>Triggers 'Show high scores' command event.</summary>
        public event CommandEventHandler ShowHighScoresCommandEvent;

        /// <summary>Draws the game screen.</summary>
        /// <param name="numberOfRows">Number of rows of the minefield.</param>
        /// <param name="numberOfColumns">Number of columns of the minefield.</param>
        public void DrawGameScreen(int numberOfRows, int numberOfColumns)
        {
            this.implementer.DrawGameScreen(numberOfRows, numberOfColumns);
        }

        /// <summary>
        /// Displays the ending messages of the game.
        /// </summary>
        /// <param name="gameState">Game end condition.</param>
        /// <param name="numberOfOpenedCells">The number of cells the user opened during the game.</param>
        public void DisplayEnd(GameEndState gameState, int numberOfOpenedCells)
        {
            this.implementer.DisplayEnd(gameState, numberOfOpenedCells);
        }

        /// <summary>
        /// Displays the messages when the user quits the game.
        /// </summary>
        public void GameExit()
        {
            this.implementer.GameExit();
        }

        /// <summary>
        /// Displays the high scores.
        /// </summary>
        /// <param name="topScores">The top scores to be displayed.</param>
        public void DisplayHighScores(IEnumerable<KeyValuePair<string, int>> topScores)
        {
            this.implementer.DisplayHighScores(topScores);
        }

        /// <summary>
        /// Displays error messages by given enumeration value.
        /// </summary>
        /// <param name="error">The error to be displayed.</param>
        public void HandleError(GameErrors error)
        {
            this.implementer.HandleError(error);
        }

        /// <summary>
        /// Reads the player name.
        /// </summary>
        /// <returns>The player name.</returns>
        public string GetPlayerName()
        {
            return this.implementer.GetPlayerName();
        }

        /// <summary>
        /// Waits for command to be triggered.
        /// </summary>
        public void WaitForCommand()
        {
            this.implementer.ReadCommand();
        }

        /// <summary>
        /// Draws the game field.
        /// </summary>
        /// <param name="minefield">The minefield to be drawn.</param>
        /// <param name="neighborMines">The minefield with all values of neighboring mines.</param>
        public void DrawGameField(CellImage[,] minefield, int[,] neighborMines)
        {
            this.implementer.DrawGameField(minefield, neighborMines);
        }

        /// <summary>
        /// Retriggers the event for HighScore command.
        /// </summary>
        /// <param name="sender">Sending object.</param>
        /// <param name="target">Coordinates in minefield.</param>
        protected virtual void OnShowHighScoresCommandEvent(object sender, ICellPosition target)
        {
            CommandEventHandler onShowHighScoresCommandEvent = this.ShowHighScoresCommandEvent;
            if (onShowHighScoresCommandEvent != null)
            {
                onShowHighScoresCommandEvent(this, target);
            }
        }

        /// <summary>
        /// Retriggers the event for OpenCell command.
        /// </summary>
        /// <param name="sender">Sending object.</param>
        /// <param name="target">Coordinates in minefield.</param>
        protected virtual void OnOpenCellCommandEvent(object sender, ICellPosition target)
        {
            CommandEventHandler onOpenCellCommandEvent = this.OpenCellCommandEvent;
            if (onOpenCellCommandEvent != null)
            {
                onOpenCellCommandEvent(this, target);
            }
        }

        /// <summary>
        /// Retriggers the event for Invalid command.
        /// </summary>
        /// <param name="sender">Sending object.</param>
        /// <param name="target">Coordinates in minefield.</param>
        protected virtual void OnInvalidCommandEvent(object sender, ICellPosition target)
        {
            CommandEventHandler onInvalidCommandEvent = this.InvalidCommandEvent;
            if (onInvalidCommandEvent != null)
            {
                onInvalidCommandEvent(this, target);
            }
        }

        /// <summary>
        /// Retriggers the event for FlagCell command.
        /// </summary>
        /// <param name="sender">Sending object.</param>
        /// <param name="target">Coordinates in minefield.</param>
        protected virtual void OnFlagCellCommandEvent(object sender, ICellPosition target)
        {
            CommandEventHandler onFlagCellCommandEvent = this.FlagCellCommandEvent;
            if (onFlagCellCommandEvent != null)
            {
                onFlagCellCommandEvent(this, target);
            }
        }

        /// <summary>
        /// Retriggers the event for Exit command.
        /// </summary>
        /// <param name="sender">Sending object.</param>
        /// <param name="target">Coordinates in minefield.</param>
        protected virtual void OnExtiCommandEvent(object sender, ICellPosition target)
        {
            CommandEventHandler onExtiCommandEvent = this.ExtiCommandEvent;
            if (onExtiCommandEvent != null)
            {
                onExtiCommandEvent(this, target);
            }
        }

        /// <summary>
        /// Retriggers the event for Boom command.
        /// </summary>
        /// <param name="sender">Sending object.</param>
        /// <param name="target">Coordinates in minefield.</param>
        protected virtual void OnBoomCommandEvent(object sender, ICellPosition target)
        {
            CommandEventHandler onBoomCommandEvent = this.BoomCommandEvent;
            if (onBoomCommandEvent != null)
            {
                onBoomCommandEvent(this, target);
            }
        }

        /// <summary>
        /// Retriggers the event for Reset command.
        /// </summary>
        /// <param name="sender">Sending object.</param>
        /// <param name="target">Coordinates in minefield.</param>
        protected virtual void OnResetCommandEvent(object sender, ICellPosition target)
        {
            CommandEventHandler onResetCommandEvent = this.ResetCommandEvent;
            if (onResetCommandEvent != null)
            {
                onResetCommandEvent(this, target);
            }
        }
    }
}