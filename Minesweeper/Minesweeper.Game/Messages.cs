//-----------------------------------------------------------------------
// <copyright file="Messages.cs" company="Telerik Academy">
//     Copyright (c) 2014 Telerik Academy. All rights reserved.
// </copyright>
// <summary>Static class that holds the messages to the user.</summary>
//-----------------------------------------------------------------------

namespace Minesweeper.Game
{
    /// <summary>
    /// Static class containing game messages.
    /// </summary>
    public static class Messages
    {
        // USER MESSAGES

        /// <summary>Press any key message.</summary>
        public const string PressAnyKey = "Press any key to continue.";

        /// <summary>Enter row and column message.</summary>
        public const string EnterRowCol = "Enter row and column: ";

        /// <summary>Intro message.</summary>
        public const string Intro = "Welcome to the game “Minesweeper”.\nTry to open all cells without mines. Use 'top' to view the scoreboard,\n'restart' to start a new game and 'exit' to quit the game. Use 'm' to flag a cell.\n";

        /// <summary>Mine exploded message.</summary>
        public const string Boom = "Booooom! You were killed by a mine. You opened {0} cells without mines.\nPlease enter your name for the top scoreboard: ";

        /// <summary>Game exit message.</summary>
        public const string Bye = "Good bye!";

        /// <summary>Opened all cells message.</summary>
        public const string Success = "Success! You opened all cells without mines.\nPlease enter your name for the top scoreboard: ";

        // ERROR MESSAGES

        /// <summary>Invalid command message.</summary>
        public const string IvalidCommand = "Invalid command!";

        /// <summary>Cell already opened message.</summary>
        public const string AlreadyOpened = "Cell already opened!";

        /// <summary>Cell is out of range message.</summary>
        public const string CellOutOfRange = "Cell is out of range of the minefield!";
    }
}