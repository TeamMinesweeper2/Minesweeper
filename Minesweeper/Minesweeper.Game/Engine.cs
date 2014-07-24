//-----------------------------------------------------------------------
// <copyright file="Engine.cs" company="Telerik Academy">
//     Copyright (c) 2014 Telerik Academy. All rights reserved.
// </copyright>
// <summary> A class that runs the main game loop.</summary>
//-----------------------------------------------------------------------
namespace Minesweeper.Game
{
    using System;
    using Minesweeper.Game.Interfaces;
    using Minesweeper.Lib;

    /// <summary>
    /// A class that runs the main game loop.
    /// </summary>
    public class Engine
    {
        /// <summary>Controls game running. If false game loop stops.</summary>
        private bool gameRunning;

        /// <summary>
        /// Runs the main game loop - accepts user input, parses the input and executes the command.
        /// </summary>
        public void Run()
        {
            // Creates new Console UI Manager.
            IUIManagerBridge uiBridge = new UIConsoleManager(new ConsoleRenderer(), new ConsoleReader());
            IUIManager uiManager = new UIManager(uiBridge);
            MinesweeperGame game = new MinesweeperGameEasy(uiManager);

            CommandFactory commandFactory = new CommandFactory(game);

            // Register game exit event from command factory.
            commandFactory.ExitGame += new EventHandler(this.OnCommandExitGame);

            // Start game loop
            this.gameRunning = true;
            while (this.gameRunning)
            {
                uiManager.WaitForCommand();
            }
        }

        /// <summary>
        /// Sets the gameRunning field to false, to stop the game loop.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnCommandExitGame(object sender, EventArgs e)
        {
            this.gameRunning = false;
        }
    }
}
