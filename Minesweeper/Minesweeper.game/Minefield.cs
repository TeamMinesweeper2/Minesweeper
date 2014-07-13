namespace Minesweeper
{
    using System;

    internal class Minefield
    {
        private readonly bool[,] mines;
        private readonly bool[,] openedCells;

        private readonly IRandomGeneratorProvider randomGenerator;

        // Constructor
        public Minefield(int rows, int colls, IRandomGeneratorProvider rndGenerator)
        {
            this.mines = new bool[rows, colls];
            this.openedCells = new bool[rows, colls];
            this.randomGenerator = rndGenerator;
            this.AddMines();
        }

        public bool[,] Mines
        { 
            get
            {
                // TODO: return copy - DONE
                bool[,] minesToReturn = new bool[this.mines.GetLength(0), this.mines.GetLength(1)];
                Array.Copy(this.mines, minesToReturn, this.mines.GetLength(0) * this.mines.GetLength(1));
                return minesToReturn;
            }
        }

        public bool[,] OpenedCells 
        {
            get
            {
                // TODO: return copy - DONE
                bool[,] openedCellsToReturn = new bool[this.mines.GetLength(0), this.mines.GetLength(1)];
                Array.Copy(this.openedCells, openedCellsToReturn, this.openedCells.GetLength(0) * this.openedCells.GetLength(1));
                return openedCellsToReturn;
            }
        }

        public MinefieldState OpenNewCell(CellPos cell)
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

        public int CountNeighborMines(CellPos currentPosition)
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

            for (int i = 0; i < 15; i++)
            {
                int index = randomGenerator.GetRandomNumber(50);
                while (this.mines[(index / 10), (index % 10)])
                {
                    index = randomGenerator.GetRandomNumber(50);
                }

                this.mines[(index / 10), (index % 10)] = true;
            }
        }
    }
}
