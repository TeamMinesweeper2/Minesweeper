﻿namespace Minesweeper
{
    /// <summary>
    /// Defines the possible images a cell can have.
    /// </summary>
    public enum CellImage
    {
        /// <summary>Closed: not flagged.</summary>
        NotFlagged,

        /// <summary>Closed: flagged.</summary>
        Flagged,

        /// <summary>Opened: has mine (exploded).</summary>
        Bomb,

        /// <summary>Opened: no mine (exploded).</summary>
        NoBomb,

        /// <summary>Opened: shows the number of adjacent mines.</summary>
        Num
    }
}