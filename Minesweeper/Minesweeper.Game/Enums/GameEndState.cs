//-----------------------------------------------------------------------
// <copyright file="GameEndState.cs" company="Telerik Academy">
//     Copyright (c) 2014 Telerik Academy. All rights reserved.
// </copyright>
// <summary> Defines the possible game end states.</summary>
//-----------------------------------------------------------------------
namespace Minesweeper.Game.Enums
{
    /// <summary>
    /// Represents end game state.
    /// </summary>
    public enum GameEndState
    {
        /// <summary>Represents success game end condition, when all non-mined cells are revealed.</summary>
        Success,

        /// <summary>Represents fail game end condition, when a mined cells is opened.</summary>
        Fail
    }
}
