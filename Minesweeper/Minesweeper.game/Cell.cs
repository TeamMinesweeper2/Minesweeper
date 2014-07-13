namespace Minesweeper
{
    using Minesweeper.Interfaces;
    using System;
    using System.Linq;

    public class Cell : ICell
    {
        //Commiting testing number 2
        private int row;
        private int col;
        private bool isOpened;
        private bool isFlagged;
        private bool isMine;

        public Cell(int row, int col)
        {
            this.Row = row;
            this.Col = col;
            this.IsOpened = false;
            this.IsFlagged = false;
            this.IsMine = false;
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
            set
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
            set
            {
                this.isFlagged = value;
            }
        }

        public bool IsMine
        {
            get
            {
                return this.isMine;
            }
            set
            {
                this.isMine = value;
            }
        }

        public void OpenCell()
        {
            if (!isOpened)
            {
                this.IsOpened = true;
                this.IsFlagged = false;
            }

        }

        public void ToggleFlag()
        {
            if (IsFlagged)
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
            if (!IsMine)
            {
                this.IsMine = true;
            }
        }
    }
}
