namespace Minesweeper
{
    using Minesweeper.Interfaces;
    using System;
    using System.Linq;

    public class Cell : ICell
    {
        private int row;
        private int col;
        private bool isOpened;
        private bool isFlagged;
        private bool isMined;

        public Cell()
        {
            this.IsOpened = false;
            this.IsFlagged = false;
            this.IsMined = false;
        }

        public int Row
        {
            get
            {
                return this.row;
            }

            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("The value of the cell's row can not be negative");
                }

                this.row = value;
            }
        }

        public int Col
        {
            get
            {
                return this.col;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("The value of the cell's col can not be negative");
                }

                this.col = value;
            }
        }

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

        public void OpenCell()
        {
            if (!this.IsOpened)
            {
                this.IsOpened = true;
                this.IsFlagged = false;
            }

        }

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

        public void AddMine()
        {
            if (!this.IsMined)
            {
                this.IsMined = true;
            }
        }
    }
}
