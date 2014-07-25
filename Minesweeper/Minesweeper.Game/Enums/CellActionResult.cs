//-----------------------------------------------------------------------
// <copyright file="CellActionResult.cs" company="Telerik Academy">
//     Copyright (c) 2014 Telerik Academy. All rights reserved.
// </copyright>
// <summary>Defines the possible results after an open/flag cell command.</summary>
//-----------------------------------------------------------------------

namespace Minesweeper
{
    /// <summary>
    /// Defines the possible results after an open/flag cell command.
    /// </summary>
    public enum CellActionResult
    {
        /// <summary>Cell out of range.</summary>
        OutOfRange,

        /// <summary>Cell already opened.</summary>
        AlreadyOpened,

        /// <summary>Cell has a mine.</summary>
        Boom,

        /// <summary>Cell has no mine and can be safely opened.</summary>
        Normal
    }
}