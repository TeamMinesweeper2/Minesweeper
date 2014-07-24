//-----------------------------------------------------------------------
// <copyright file="CommandFactory.cs" company="Telerik Academy">
//     Copyright (c) 2014 Telerik Academy. All rights reserved.
// </copyright>
// <summary> A class that can parse string input and return a command of type <see cref="IGameCommand"/>.</summary>
//-----------------------------------------------------------------------

namespace Minesweeper.Game
{
    using System;
    using System.Linq;
    using Minesweeper.Game.Interfaces;
    using Minesweeper.Lib.Interfaces;

    /// <summary>
    /// A class that can parse string input and return a command of type <see cref="IGameCommand"/>.
    /// </summary>
    public class CommandFactory
    {
        /// <summary>
        /// The <see cref="MinesweeperGame"/> object for which commands will be parsed.
        /// </summary>
        private MinesweeperGame game;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandFactory"/> class.
        /// </summary>
        /// <param name="game">The <see cref="MinesweeperGame"/> object for which commands will be returned.</param>
        public CommandFactory(MinesweeperGame game)
        {
            this.Game = game;
            this.RegisterListeners(game.UiManager);
        }

        /// <summary>
        /// Gets or sets the <see cref="MinesweeperGame"/> object for which commands will be generated.
        /// </summary>
        /// <value>The MinesweeperGame property gets the value of the MinesweeperGame field, game.</value>
        protected MinesweeperGame Game
        {
            get 
            { 
                return this.game; 
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Game field cannot recieve null value!");
                }

                this.game = value;
            }
        }

        /// <summary>
        /// Register listeners for commands received through the UI Manager.
        /// </summary>
        /// <param name="uiManager">Current UI Manager.</param>
        private void RegisterListeners(IUIManager uiManager)
        {
            uiManager.BoomCommandEvent += new CommandEventHandler(this.ExecuteBoomCommand);
            uiManager.ExtiCommandEvent += new CommandEventHandler(this.ExecuteExitCommand);
            uiManager.FlagCellCommandEvent += new CommandEventHandler(this.ExecuteFlagCellCommand);
            uiManager.InvalidCommandEvent += new CommandEventHandler(this.ExecuteInvalidCommand);
            uiManager.OpenCellCommandEvent += new CommandEventHandler(this.ExecuteOpenCellCommand);
            uiManager.ResetCommandEvent += new CommandEventHandler(this.ExecuteRestartCommand);
            uiManager.ShowHighScoresCommandEvent += new CommandEventHandler(this.ExecuteShowScoresCommand);
        }

        /// <summary>
        /// Executes command to simulate mined explosion.
        /// </summary>
        /// <param name="sender">Sending object.</param>
        /// <param name="coordinates">Coordinates in minefield.</param>
        private void ExecuteBoomCommand(object sender, ICellPosition coordinates)
        {
            this.game.MineBoomed();
        }

        /// <summary>
        /// Executes command to exit the game.
        /// </summary>
        /// <param name="sender">Sending object.</param>
        /// <param name="coordinates">Coordinates in minefield.</param>
        private void ExecuteExitCommand(object sender, ICellPosition coordinates)
        {
            this.game.ExitGame();
            Environment.Exit(0); // TODO: Make event to send game stop signal to the Engine.
        }
        
        /// <summary>
        /// Executes command to flag cell in provided coordinates.
        /// </summary>
        /// <param name="sender">Sending object.</param>
        /// <param name="coordinates">Coordinates in minefield.</param>
        private void ExecuteFlagCellCommand(object sender, ICellPosition coordinates)
        {
            this.game.FlagCell(coordinates);
        }
        
        /// <summary>
        /// Executes command to inform the game that entered command is invalid.
        /// </summary>
        /// <param name="sender">Sending object.</param>
        /// <param name="coordinates">Coordinates in minefield.</param>
        private void ExecuteInvalidCommand(object sender, ICellPosition coordinates)
        {
            this.game.HandleCommandError();
        }
        
        /// <summary>
        /// Executes command to open cell in provided coordinates.
        /// </summary>
        /// <param name="sender">Sending object.</param>
        /// <param name="coordinates">Coordinates in minefield.</param>
        private void ExecuteOpenCellCommand(object sender, ICellPosition coordinates)
        {
            this.game.OpenCell(coordinates);
        }

        /// <summary>
        /// Executes command to restart the game.
        /// </summary>
        /// <param name="sender">Sending object.</param>
        /// <param name="coordinates">Coordinates in minefield.</param>
        private void ExecuteRestartCommand(object sender, ICellPosition coordinates)
        {
            this.game.GenerateMinefield();
        }

        /// <summary>
        /// Executes command to show high scores on screen.
        /// </summary>
        /// <param name="sender">Sending object.</param>
        /// <param name="coordinates">Coordinates in minefield.</param>
        private void ExecuteShowScoresCommand(object sender, ICellPosition coordinates)
        {
            this.game.ShowScores();
        }
    }
}