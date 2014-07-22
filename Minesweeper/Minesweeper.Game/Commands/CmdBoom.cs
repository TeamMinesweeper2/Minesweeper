//-----------------------------------------------------------------------
// <copyright file="CmdBoom.cs" company="Telerik Academy">
//     Copyright (c) 2014 Telerik Academy. All rights reserved.
// </copyright>
// <summary> Implements the Execute method by invoking an action on MinesweeperGame.</summary>
//-----------------------------------------------------------------------
namespace Minesweeper
{
    using System;
    using Minesweeper.Game;

    /// <summary>
    /// Implements the Execute method by invoking an action on <see cref="MinesweeperGame"/>.
    /// </summary>
    public class CmdBoom : ICommand
    {
        /// <summary>
        /// The <see cref="MinesweeperGame"/> object on which the action will be invoked.
        /// </summary>
        private MinesweeperGame game;

        /// <summary>
        /// Initializes a new instance of the <see cref="CmdBoom"/> class.
        /// </summary>
        /// <param name="game">The <see cref="MinesweeperGame"/> object on which the action will be invoked.</param>
        public CmdBoom(MinesweeperGame game)
        {
            if (game == null)
            {
                throw new ArgumentNullException("game");  
            }

            this.game = game;
        }

        /// <summary>
        /// Invokes the action on the <see cref="MinesweeperGame"/> object.
        /// </summary>
        /// <returns>Returns true if more commands can be executed.</returns>
        public bool Execute()
        {
            this.game.MineBoomed();
            return true;
        }
    }
}
