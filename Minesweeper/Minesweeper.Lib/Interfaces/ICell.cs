//-----------------------------------------------------------------------
// <copyright file="ICell.cs" company="Telerik Academy">
//     Copyright (c) 2014 Telerik Academy. All rights reserved.
// </copyright>
// <summary>Class library for the Minesweeper game.</summary>
//-----------------------------------------------------------------------
namespace Minesweeper.Lib
{
    using System;
    using System.Linq;

    /// <summary>
    /// Defines methods for interacting with a cell. 
    /// </summary>
    public interface ICell
    {
        /// <summary>
        /// Gets a value indicating whether the current cell is opened.
        /// </summary>
        /// <value>True if the current cell is opened.</value>
        bool IsOpened
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the current cell is mined.
        /// </summary>
        /// <value>True if the current cell has a mine.</value>
        bool IsMined
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the current cell is flagged.
        /// </summary>
        /// <value>True if the current cell is flagged.</value>
        bool IsFlagged
        {
            get;
        }

        /// <summary>
        /// Opens the current cell.
        /// </summary>
        void OpenCell();

        /// <summary>
        /// Toggles the flag of the current cell.
        /// </summary>
        void ToggleFlag();

        /// <summary>
        /// Adds mine to the current cell.
        /// </summary>
        void AddMine();

        /// <summary>
        /// Removes a mine from the current cell.
        /// </summary>
        void Disarm();
    }
}
