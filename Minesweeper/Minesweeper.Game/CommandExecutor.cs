//-----------------------------------------------------------------------
// <copyright file="CommandExecutor.cs" company="Telerik Academy">
//     Copyright (c) 2014 Telerik Academy. All rights reserved.
// </copyright>
// <summary>A class which can execute commands in the game.</summary>
//-----------------------------------------------------------------------

namespace Minesweeper.Game
{
    using System;

    /// <summary>
    /// A class which can execute commands of type <see cref="Minesweeper.Game.ICommand"/>.
    /// </summary>
    public class CommandExecutor
    {
        /// <summary>
        /// Executes commands of type <see cref="Minesweeper.Game.ICommand"/>.
        /// </summary>
        /// <param name="cmd">The command to execute.</param>
        /// <returns>Returns true if more commands can be executed.</returns>
        public bool ExecuteCommand(ICommand cmd)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException();
            }

            return cmd.Execute();
        }
    }
}