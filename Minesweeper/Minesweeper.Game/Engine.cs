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
          
            // Start game loop
            bool gameRunning = true;
            while (gameRunning)
            {
                uiManager.WaitForCommand();
            }
        }
    }
}
