namespace Minesweeper
{
    using System;
    using Minesweeper.Lib;

    internal class Minefield
    {
        private readonly ICell[] cells;
        private readonly IRandomGeneratorProvider randomGenerator;
        private readonly int cols;
        private readonly int rows;
        private readonly int numberOfMines;

        private int openedCellsCount;

        // Constructor
        public Minefield(int rows, int cols, int numberOfMines, IRandomGeneratorProvider rndGenerator)
        {
            this.cells = new Cell[rows * cols];
            this.Initialize();
            this.rows = rows;
            this.cols = cols;
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

                mines = GetValueCount(x => x.IsMined);

                return mines;
            }
        }

        public bool[,] OpenedCells
        {
            get
            {
                bool[,] openedCells;

                openedCells = GetValueCount(x => x.IsOpened);

                return openedCells;
            }
        }

        public bool[,] FlaggedCells
        {
            get
            {
                return GetValueCount(x => x.IsFlagged);
            }
        }

        public MinefieldState OpenNewCell(CellPos cell)
        {
            var isInside = this.IsInsideMatrix(cell.Row, cell.Col);
            if (!isInside)
            {
                return MinefieldState.OutOfRange;
            }

            if (this.cells[cell.Row * this.cols + cell.Col].IsOpened)
            {
                return MinefieldState.AlreadyOpened;
            }
            else
            {
                this.cells[cell.Row * this.cols + cell.Col].OpenCell();
                this.openedCellsCount += 1; // Counts opened cells.

                if (this.cells[cell.Row * this.cols + cell.Col].IsMined)
                {
                    return MinefieldState.Boom;
                }

                return MinefieldState.Normal;
            }
        }

        public MinefieldState FlagCell(CellPos cell)
        {
            var isInside = this.IsInsideMatrix(cell.Row, cell.Col);

            if (!isInside)
            {
                return MinefieldState.OutOfRange;
            }

            if (this.cells[cell.Row * this.cols + cell.Col].IsOpened)
            {
                return MinefieldState.AlreadyOpened;
            }

            this.cells[cell.Row * this.cols + cell.Col].ToggleFlag();
            return MinefieldState.Normal;
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

                    int currentIndex = (currentPosition.Row + row) * this.cols + (currentPosition.Col + col);
                    if (this.IsInsideMatrix(currentPosition.Row + row, currentPosition.Col + col))
                    {
                        if (this.cells[currentIndex].IsMined)
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

        /// <summary>
        /// Gets an 'image' of the minefield as a matrix of type CellImage
        /// </summary>
        /// <param name="gameEnded">True if game has ended (will reveal all mines)</param>
        /// <returns>A matrix of cells of type CellImage.</returns>
        public CellImage[,] GetImage(bool gameEnded)
        {
            var opened = this.OpenedCells;
            var mines = this.Mines;
            var flagged = this.FlaggedCells;

            var image = new CellImage[this.rows, this.cols];
            for (int row = 0; row < this.rows; row++)
            {
                for (int col = 0; col < this.cols; col++)
                {
                    if (opened[row, col])
                    {
                        // Num
                        image[row, col] = CellImage.Num;
                    }
                    else
                    {
                        if (gameEnded)
                        {
                            // Bomb, NoBomb
                            image[row, col] = mines[row, col] ? CellImage.Bomb : CellImage.NoBomb;
                        }
                        else
                        {
                            // Flagged, NotFlagged
                            image[row, col] = flagged[row, col] ? CellImage.Flagged : CellImage.NotFlagged;
                        }
                    }
                }
            }

            return image;
        }

        private bool IsInsideMatrix(int row, int col)
        {
            return (0 <= row && row < this.rows) && (0 <= col && col < this.cols);
        }

        private void Initialize()
        {
            for (int index = 0; index < this.cells.Length; index++)
            {
                this.cells[index] = new Cell();
            }
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
            int n = array.Length - 1;
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
            bool[,] result = new bool[this.rows, this.cols];

            for (int index = 0; index < this.cells.Length; index++)
            {
                result[index / this.cols, index % this.cols] = filter(this.cells[index]);
            }

            return result;
        }
    }
}
