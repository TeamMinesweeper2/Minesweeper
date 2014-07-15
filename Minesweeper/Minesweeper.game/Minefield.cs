namespace Minesweeper
{
    using System;
    using Minesweeper.Lib;

    /// <summary>
    /// Minefield class represents matrix of cells.
    /// </summary>
    internal class Minefield
    {
        /// <summary>Minefield container of cells.</summary>
        private readonly ICell[] cells;

        /// <summary>Random generator provider.</summary>
        private readonly IRandomGeneratorProvider randomGenerator;

        /// <summary>Number of columns for the minefield.</summary>
        private readonly int columnsCount;

        /// <summary>Number of rows for the minefield.</summary>
        private readonly int rowsCount;

        /// <summary>Number of mines for the minefield.</summary>
        private readonly int numberOfMines;

        /// <summary>Calculated number of mines for each cell in the minefield.</summary>
        private readonly int[,] allNeighborMines;

        /// <summary>Number of opened cells in the minefield.</summary>
        private int openedCellsCount;

        /// <summary>
        /// Creates a new instance of <see cref="Minefield"/> class.
        /// </summary>
        /// <param name="rows">Number of rows.</param>
        /// <param name="cols">Number of columns.</param>
        /// <param name="numberOfMines">Number of mines.</param>
        /// <param name="rndGenerator">Random generator provider.</param>
        public Minefield(int rows, int cols, int numberOfMines, IRandomGeneratorProvider rndGenerator)
        {
            this.cells = new Cell[rows * cols];
            this.InitializeCellsMatrix();
            this.rowsCount = rows;
            this.columnsCount = cols;
            this.numberOfMines = numberOfMines;
            this.openedCellsCount = 0;
            this.randomGenerator = rndGenerator;
            this.GenerateMines();
            this.allNeighborMines = this.CalculateNeighborMines();
        }

        /// <summary>
        /// Gets the number of neighbor mines for each cell.
        /// </summary>
        public int[,] AllNeighborMines
        {
            get
            {
                var copy = (int[,])this.allNeighborMines.Clone();
                return copy;
            }
        }

        /// <summary>
        /// Handles cell opening. Reveals cell's content and returns it's state.
        /// </summary>
        /// <param name="cell">Cell's position in the minefield matrix.</param>
        /// <returns>State of the matrix.</returns>
        public MinefieldState OpenCellHandler(CellPos cell) // TODO: Job to be done by Cell itself.
        {
            var isInsideMatrix = this.IsInsideMatrix(cell.Row, cell.Col);
            if (!isInsideMatrix)
            {
                return MinefieldState.OutOfRange;
            }

            if (this.cells[(cell.Row * this.columnsCount) + cell.Col].IsOpened)
            {
                return MinefieldState.AlreadyOpened;
            }
            else
            {
                int index = (cell.Row * this.columnsCount) + cell.Col;

                if (this.cells[index].IsMined)
                {
                    // Cells with bombs are not counted as open
                    return MinefieldState.Boom;
                }

                // Open cell
                this.cells[index].OpenCell();
                this.openedCellsCount += 1; // Counts opened cells.
                return MinefieldState.Normal;
            }
        }

        /// <summary>
        /// Handles cell flagging.
        /// </summary>
        /// <param name="cell">Cell position.</param>
        /// <returns>State of the minefield.</returns>
        public MinefieldState FlagCell(CellPos cell)
        {
            var isInside = this.IsInsideMatrix(cell.Row, cell.Col);
            int currentIndex = (cell.Row * this.columnsCount) + cell.Col;

            if (!isInside)
            {
                return MinefieldState.OutOfRange;
            }

            if (this.cells[currentIndex].IsOpened)
            {
                return MinefieldState.AlreadyOpened;
            }

            this.cells[currentIndex].ToggleFlag();
            return MinefieldState.Normal;
        }

        /// <summary>
        /// Returns opened cells count.
        /// </summary>
        /// <returns></returns>
        public int GetOpenedCells()
        {
            return this.openedCellsCount;
        }

        /// <summary>
        /// Gets an 'image' of the minefield as a matrix of type CellImage
        /// </summary>
        /// <param name="showAll">Set to 'true' to uncover all mines.</param>
        /// <returns>A matrix of cells of type CellImage.</returns>
        public CellImage[,] GetImage(bool showAll)
        {
            var opened = this.OpenedCells();
            var mines = this.Mines();
            var flagged = this.FlaggedCells();

            var image = new CellImage[this.rowsCount, this.columnsCount];
            for (int row = 0; row < this.rowsCount; row++)
            {
                for (int col = 0; col < this.columnsCount; col++)
                {
                    if (opened[row, col])
                    {
                        // Num
                        image[row, col] = CellImage.Num;
                    }
                    else
                    {
                        if (showAll)
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

        /// <summary>
        /// Checks if all cells are opened without explosion.
        /// </summary>
        /// <returns>True if all non-mined cells are opened.</returns>
        public bool IsDisarmed()
        {
            return this.GetOpenedCells() >= ((this.rowsCount * this.columnsCount) - this.numberOfMines);
        }

        /// <summary>
        /// Validates if given coordinates are inside the minefield matrix.
        /// </summary>
        /// <param name="row">Current position by row.</param>
        /// <param name="col">Current position by column.</param>
        /// <returns>Validation result.</returns>
        private bool IsInsideMatrix(int row, int col)
        {
            return (0 <= row && row < this.rowsCount) && (0 <= col && col < this.columnsCount);
        }

        /// <summary>
        /// Initializes minefield matrix of cells.
        /// </summary>
        private void InitializeCellsMatrix()
        {
            for (int index = 0; index < this.cells.Length; index++)
            {
                this.cells[index] = new Cell();
            }
        }

        /// <summary>
        /// Generates mines randomly in the minefield matrix.
        /// </summary>
        private void GenerateMines()
        {
            // Add mines.
            for (int index = 0; index < this.numberOfMines; index++)
            {
                this.cells[index].AddMine();
            }

            // Shuffle mines.
            this.Shuffle(this.cells);
        }

        /// <summary>
        /// Shuffles an array of T.
        /// </summary>
        /// <typeparam name="T">Generic type.</typeparam>
        /// <param name="array">Array of T to be shuffled randomly.</param>
        private void Shuffle<T>(T[] array)
        {
            int n = array.Length - 1;
            while (n > 1)
            {
                int k = this.randomGenerator.GetRandomNumber(n);
                n = n - 1;
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }

        /// <summary>
        /// Converts minefield matrix to matrix of type T upon to given function.
        /// </summary>
        /// <typeparam name="T">Return type of the given function.</typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        private T[,] ConvertMinefield<T>(Func<ICell, T> func)
        {
            T[,] result = new T[this.rowsCount, this.columnsCount];

            for (int index = 0; index < this.cells.Length; index++)
            {
                int row = index / this.columnsCount;
                int column = index % this.columnsCount;
                result[row, column] = func(this.cells[index]);
            }

            return result;
        }

        /// <summary>
        /// Generates matrix of mines from minefield.
        /// </summary>
        /// <returns>Two dimensional bool array.</returns>
        private bool[,] Mines()
        {
            return this.ConvertMinefield(x => x.IsMined);
        }

        /// <summary>
        /// Generates matrix of opened cells from minefield.
        /// </summary>
        /// <returns>Two dimensional bool array.</returns>
        private bool[,] OpenedCells()
        {
            return this.ConvertMinefield(x => x.IsOpened);
        }

        /// <summary>
        /// Generates matrix of flagged cells from minefield.
        /// </summary>
        /// <returns>Two dimensional bool array.</returns>
        private bool[,] FlaggedCells()
        {
            return this.ConvertMinefield(x => x.IsFlagged);
        }

        /// <summary>
        /// Calculates the number of neighbor mines for each cell.
        /// </summary>
        /// <returns>Two dimensional array with neighbor mines count.</returns>
        private int[,] CalculateNeighborMines()
        {
            var resultArray = new int[this.rowsCount, this.columnsCount];
            for (int row = 0; row < this.rowsCount; row++)
            {
                for (int col = 0; col < this.columnsCount; col++)
                {
                    resultArray[row, col] = this.CountNeighborMinesPerCell(new CellPos(row, col));
                }
            }

            return resultArray;
        }

        /// <summary>
        /// Calculates the number of neighbor mines for a specified cell.
        /// </summary>
        /// <returns>Neighbor mines count.</returns>
        private int CountNeighborMinesPerCell(CellPos currentPosition)
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

                    int currentIndex = ((currentPosition.Row + row) * this.columnsCount) + (currentPosition.Col + col);
                    bool isInsideMatrix = this.IsInsideMatrix(currentPosition.Row + row, currentPosition.Col + col);
                    if (isInsideMatrix)
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
    }
}
