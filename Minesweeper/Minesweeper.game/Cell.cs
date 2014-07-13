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
        private bool isMine;

        public Cell(int row, int col)
        {
            this.row = row;
            this.col = col;
            this.isOpened = false;
            this.IsFlagged = false;
            this.isMine = false;
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

        public void OpenCell()
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public bool ToggleFlag()
        {
            if (IsFlagged)
            {
                this.IsFlagged = false;
            }
            else
            {
                this.IsFlagged = true;
            }

            // TODO: Implement this method
            throw new NotImplementedException();
        }
        public void AddMine()
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }
    }
}
