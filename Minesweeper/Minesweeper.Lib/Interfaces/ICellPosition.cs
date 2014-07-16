//-----------------------------------------------------------------------
// <copyright file="ICellPosition.cs" company="Telerik Academy">
//     Copyright (c) 2014 Telerik Academy. All rights reserved.
// </copyright>
// <summary>Class library for the Minesweeper game.</summary>
//-----------------------------------------------------------------------
namespace Minesweeper.Lib
{
    using System;

    /// <summary>
    /// Defines properties for the cell position.
    /// </summary>
    public interface ICellPosition
    {
        /// <summary>
        /// Gets or sets value for position by row.
        /// </summary>
        /// <value>Position by row.</value>
        int Row { get; set; }

        /// <summary>
        /// Gets or sets value for position by column.
        /// </summary>
        /// <value>Position by column.</value>
        int Col { get; set; }
    }
}