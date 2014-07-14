namespace Minesweeper.Lib
{
    using System;
    using System.Linq;

    public class Cell : ICell
    {
        private bool isOpened;
        private bool isFlagged;
        private bool isMined;

        /// <summary>
        /// Constructor of the class Cell
        /// </summary>
        public Cell()
        {
            this.IsOpened = false;
            this.IsFlagged = false;
            this.IsMined = false;
        }

        /// <summary>
        /// Checks if the current cell is opened
        /// </summary>
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
        /// Checks if the current cell is flagged
        /// </summary>
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
        /// Checks whether the current cell is mined
        /// </summary>
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
        /// Marks the current cell as opened and removes flages
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
        /// Changes the state of the flag
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
        /// Marks the current cell as mined
        /// </summary>
        public void AddMine()
        {
            if (!this.IsMined)
            {
                this.IsMined = true;
            }
        }
    }
}
