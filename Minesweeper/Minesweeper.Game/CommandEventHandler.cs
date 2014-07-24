//-----------------------------------------------------------------------
// <copyright file="CommandEventHandler.cs" company="Telerik Academy">
//     Copyright (c) 2014 Telerik Academy. All rights reserved.
// </copyright>
// <summary> Delegate that describes the event handler for command.</summary>
//-----------------------------------------------------------------------
namespace Minesweeper.Game
{
    using Minesweeper.Lib.Interfaces;

    /// <summary>
    /// Delegate that describes the event handler for command.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="target">Position on minefield.</param>
    public delegate void CommandEventHandler(object sender, ICellPosition target);
}