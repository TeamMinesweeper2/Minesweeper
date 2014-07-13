namespace Minesweeper
{
    using System;
    using Minesweeper.Interfaces;

    internal class Minefield
    {
        private readonly ICell[] cells;
        private readonly IRandomGeneratorProvider randomGenerator;
        private readonly int columns;
        private readonly int rows;
        private readonly int numberOfMines;

        private int openedCellsCount;

        // Constructor
        public Minefield(int rows, int cols, int numberOfMines, IRandomGeneratorProvider rndGenerator)
        {
            this.cells = new ICell[rows * cols];
            this.rows = rows;
            this.columns = cols;
            this.numberOfMines = numberOfMines;
            this.openedCellsCount = 0;
            this.randomGenerator = rndGenerator;
            this.AddMines();
        }

        public bool[,] Mines
        {
            get
            {
                bool[,] mines;

                mines = GetValueCount(x => x.IsMined());

                return mines;
            }
        }

        public bool[,] OpenedCells
        {
            get
            {
                bool[,] openedCells;

                openedCells = GetValueCount(x => x.IsOpened());

                return openedCells;
            }
        }

        public MinefieldState OpenNewCell(CellPos cell)
        {
            var isInside = this.IsInsideMatrix(cell.Row, cell.Col);
            if (!isInside)
            {
                return MinefieldState.OutOfRange;
            }

            if (this.cells[cell.Row * this.columns + cell.Col].IsOpened())
            {
                return MinefieldState.AlreadyOpened;
            }
            else
            {
                this.cells[cell.Row * this.columns + cell.Col].OpenCell();
                this.openedCellsCount += 1; // Counts opened cells.

                if (this.cells[cell.Row * this.columns + cell.Col].IsMined())
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

                    int currentIndex = (currentPosition.Row + row) * this.columns + (currentPosition.Col + col);
                    if (this.IsInsideMatrix(currentPosition.Row + row, currentPosition.Col + col))
                    {
                        if (this.cells[currentIndex].IsMined())
                        {
                            counter++;
                        }
                    }
                }
            }

            return counter;
        }

        public int CountOpen()
        {
            return this.openedCellsCount;
        }

        private bool IsInsideMatrix(int row, int col)
        {
            return (0 <= row && row <= this.rows) && (0 <= col && col <= this.columns);
        }

        private void AddMines()
        {
            // Add mines.
            for (int index = 0; index < this.numberOfMines; index++)
            {
                this.cells[index].AddMine();
            }

            // Shuffle mines.
            Shuffle(this.cells);
        }

        private void Shuffle<T>(T[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = randomGenerator.GetRandomNumber(n);
                n = n - 1;
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }

        private bool[,] GetValueCount(Func<ICell, bool> filter)
        {
            bool[,] result = new bool[this.rows, this.columns];

            for (int index = 0; index < this.cells.Length; index++)
            {
                result[index / this.columns, index % this.columns] = filter(this.cells[index]);
            }

            return result;
        }
    }
}
