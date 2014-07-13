namespace Minesweeper
{
    using System;

    internal class Minefield
    {
        private bool[,] mines;
        private bool[,] openedCells;

        // Constructor
        public Minefield(int rows, int colls)
        {
            this.mines = new bool[rows, colls];
            this.openedCells = new bool[rows, colls];
            this.AddMines();
        }

        public bool[,] Mines
        { 
            get
            {
                // TODO: return copy
                return this.mines;
            }
        }

        public bool[,] OpenedCells 
        {
            get
            {
                // TODO: return copy
                return this.openedCells;
            }
        }

        public MinefieldState OpenNewCell(Cell cell)
        {
            var isInside = this.IsInsideMatrix(cell.Row, cell.Col);
            if (!isInside)
            {
                return MinefieldState.OutOfRange;
            }

            if (this.openedCells[cell.Row, cell.Col])
            {
                return MinefieldState.AlreadyOpened;
            }
            else
            {
                this.openedCells[cell.Row, cell.Col] = true;

                if (this.mines[cell.Row, cell.Col])
                {
                    return MinefieldState.Boom;
                }

                return MinefieldState.Normal;
            }
        }

        public int CountNeighborMines(Cell currentPosition)
        {
            int counter = 0;

            for (int row = -1; row < 2; row++)
            {
                for (int col = -1; col < 2; col++)
                {
                    if (col == 0 && row == 0)
                    {
                        continue;
                    }

                    if (this.IsInsideMatrix(currentPosition.Row + row, currentPosition.Col + col) &&
                        this.mines[currentPosition.Row + row, currentPosition.Col + col])
                    {
                        counter++;
                    }
                }
            }

            return counter;
        }

        public int CountOpen()
        {
            int res = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (this.openedCells[i, j])
                    {
                        res++;
                    }
                }
            }

            return res;
        }

        private bool IsInsideMatrix(int row, int col)
        {
            return (0 <= row && row <= 4) && (0 <= col && col <= 9);
        }

        private void AddMines()
        {
            // TODO: make testable (use shuffling)
            Random random = new Random();
            for (int i = 0; i < 15; i++)
            {
                int index = random.Next(50);
                while (this.mines[(index / 10), (index % 10)])
                {
                    index = random.Next(50);
                }

                this.mines[(index / 10), (index % 10)] = true;
            }
        }
    }
}
