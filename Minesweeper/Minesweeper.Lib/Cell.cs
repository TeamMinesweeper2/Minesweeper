//-----------------------------------------------------------------------
// <copyright file="Cell.cs" company="Telerik Academy">
//     Copyright (c) 2014 Telerik Academy. All rights reserved.
// </copyright>
// <summary>Class library for the Minesweeper game.</summary>
//-----------------------------------------------------------------------
namespace Minesweeper.Lib
{
    using System;
    using System.Linq;
    using Minesweeper.Lib.Interfaces;

    /// <summary>
    /// Cell class which represents the minefield's single cell states.
    /// </summary>
    public class Cell : ICell
    {
        /// <summary>A boolean flag indicating if the current cell is opened.</summary>
        private bool isOpened;

        /// <summary>A boolean flag indicating if the current cell is flagged.</summary>
        private bool isFlagged;

        /// <summary>A boolean flag indicating if the current cell is mined.</summary>
        private bool isMined;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cell" /> class.
        /// </summary>
        public Cell()
        {
            this.IsOpened = false;
            this.IsFlagged = false;
            this.IsMined = false;
        }

        /// <summary>
        /// Gets a value indicating whether the current cell is opened.
        /// </summary>
        /// <value>True if the current cell is opened.</value>
        public bool IsOpened
        {
            get
            {
                return this.isOpened;
            }

            private set
            {
                this.isOpened = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current cellPosition is flagged.
        /// </summary>
        /// <value>True if the current cell is flagged.</value>
        public bool IsFlagged
        {
            get
            {
                return this.isFlagged;
            }

            private set
            {
                this.isFlagged = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current cell is mined.
        /// </summary>
        /// <value>True if the current cell has a mine.</value>
        public bool IsMined
        {
            get
            {
                return this.isMined;
            }

            private set
            {
                this.isMined = value;
            }
        }

        /// <summary>
        /// Marks the current cell as opened and removes flags.
        /// </summary>
        public void OpenCell()
        {
            if (!this.IsOpened)
            {
                this.IsOpened = true;
                this.IsFlagged = false;
            }
        }

        /// <summary>
        /// Changes the state of the flag.
        /// </summary>
        public void ToggleFlag()
        {
            if (this.IsFlagged)
            {
                this.IsFlagged = false;
            }
            else
            {
                this.IsFlagged = true;
            }
        }

        /// <summary>
        /// Marks the current cell as mined.
        /// </summary>
        public void AddMine()
        {
            if (!this.IsMined)
            {
                this.IsMined = true;
            }
        }

        /// <summary>
        /// Removes the mine from the current cell.
        /// </summary>
        public void Disarm()
        {
            if (this.IsMined)
            {
                this.IsMined = false;
            }
        }
    }
}
