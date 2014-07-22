//-----------------------------------------------------------------------
// <copyright file="GameErrors.cs" company="Telerik Academy">
//     Copyright (c) 2014 Telerik Academy. All rights reserved.
// </copyright>
// <summary> Defines the possible errors in the game while executing command.</summary>
//-----------------------------------------------------------------------
namespace Minesweeper.Game
{
    /// <summary>
    /// Represents game error.
    /// </summary>
    public enum GameErrors
    {
        /// <summary>Represents game error, when entered coordinates are out of the range.</summary>
        CellOutOfRange,

        /// <summary>Represents game error, when entered coordinates are equal to an already opened cell.</summary>
        CellAlreadyOpened,

        /// <summary>Represents game error, when invalid command is entered.</summary>
        InvalidCommand
    }
}
