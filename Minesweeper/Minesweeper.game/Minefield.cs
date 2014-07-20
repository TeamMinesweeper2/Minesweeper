namespace Minesweeper.Game
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Minesweeper.Lib;

    /// <summary>
    /// Minefield class represents matrix of cells.
    /// </summary>
    public class Minefield
    {
        /// <summary>Exception message format for value type parameters.</summary>
        private const string ValueTypesExceptionFormat = "The value - {0} for {1} count must be greater than zero!";

        /// <summary>Exception message for null random generator provider.</summary>
        private const string IRandomGeneratorProviderNullExceptionMessage = "The constructor cannot generate mines with random generator equal to null!";

        /// <summary>Minefield container of cells.</summary>
        private readonly ICell[] cells;

        /// <summary>Random generator provider.</summary>
        private IRandomGeneratorProvider randomGenerator;

        /// <summary>Calculated number of mines for each cell in the minefield.</summary>
        private int[,] allNeighborMines;

        /// <summary>Number of columns for the minefield.</summary>
        private int columnsCount;

        /// <summary>Number of rows for the minefield.</summary>
        private int rowsCount;

        /// <summary>Number of mines for the minefield.</summary>
        private int numberOfMines;

        /// <summary>Number of opened cells in the minefield.</summary>
        private int openedCellsCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="Minefield"/> class.
        /// </summary>
        /// <param name="rows">Number of rows.</param>
        /// <param name="cols">Number of columns.</param>
        /// <param name="numberOfMines">Number of mines.</param>
        /// <param name="rndGenerator">Random generator provider.</param>
        public Minefield(int rows, int cols, int numberOfMines, IRandomGeneratorProvider rndGenerator)
        {
            // Validations
            this.RowsCount = rows;
            this.ColumnsCount = cols;
            this.NumberOfMines = numberOfMines;
            this.RandomGenerator = rndGenerator;

            // Initializations
            this.openedCellsCount = 0;
            this.cells = new Cell[rows * cols];
            this.InitializeCellsMatrix();
            this.GenerateMines();
            this.allNeighborMines = this.CalculateNeighborMines();
        }

        /// <summary>
        /// Gets the number of neighbor mines for each cell. Returns copy of the allNeighborMines matrix.
        /// </summary>
        /// <value>Not accepted.</value>
        public int[,] AllNeighborMines
        {
            get
            {
                var copy = (int[,])this.allNeighborMines.Clone();
                return copy;
            }
        }

        /// <summary>
        /// Gets opened cells count.
        /// </summary>
        /// <value>Not accepted.</value>
        public int GetOpenedCells
        {
            get
            {
                return this.openedCellsCount;
            }
        }

        /// <summary>
        /// Sets the value of randomGenerator.
        /// </summary>
        private IRandomGeneratorProvider RandomGenerator
        {
            set
            {
                if (value == null)
                {
                    throw new ArgumentException(IRandomGeneratorProviderNullExceptionMessage);
                }

                this.randomGenerator = value;
            }
        }

        /// <summary>
        /// Sets the value of rowsCount.
        /// </summary>
        private int RowsCount
        {
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException(string.Format(ValueTypesExceptionFormat, value, "rows"));
                }

                this.rowsCount = value;
            }
        }

        /// <summary>
        /// Sets the value of columnsCount.
        /// </summary>
        private int ColumnsCount
        {
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException(string.Format(ValueTypesExceptionFormat, value, "column"));
                }

                this.columnsCount = value;
            }
        }

        /// <summary>
        /// Sets the value of numberOfMines.
        /// </summary>
        private int NumberOfMines
        {
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException(string.Format(ValueTypesExceptionFormat, value, "mines"));
                }

                this.numberOfMines = value;
            }
        }

        /// <summary>
        /// Handles cell opening. Reveals cell's content and returns it's state.
        /// </summary>
        /// <param name="cell">Cell's position in the minefield matrix.</param>
        /// <returns>State of the minefield.</returns>
        public MinefieldState OpenCellHandler(ICellPosition cell) // TODO: Job to be done by Cell itself.
        {
            var isInsideMatrix = this.IsInsideMatrix(cell.Row, cell.Col);
            int currentIndex = this.GetIndex(cell);

            if (!isInsideMatrix)
            {
                return MinefieldState.OutOfRange;
            }

            if (this.cells[currentIndex].IsOpened)
            {
                return MinefieldState.AlreadyOpened;
            }

            // If the first open cell has mine, swap the mine with an empty cell
            if (this.openedCellsCount == 0 && this.cells[currentIndex].IsMined)
            {
                this.DisarmFirstCell(this.cells[currentIndex]);
            }

            if (this.cells[currentIndex].IsMined)
            {
                // Cells with bombs are not counted as open
                return MinefieldState.Boom;
            }

            // Open cell
            this.cells[currentIndex].OpenCell();
            this.openedCellsCount += 1; // Counts opened cells.

            if (this.CountNeighborMinesPerCell(cell) == 0)
            {
                this.OpenEmptyCellsRecursive(cell);
            }

            return MinefieldState.Normal;
        }

        /// <summary>
        /// Handles cell flagging.
        /// </summary>
        /// <param name="cell">Cell's position in the minefield matrix.</param>
        /// <returns>State of the minefield.</returns>
        public MinefieldState FlagCellHandler(ICellPosition cell)
        {
            var isInsideMatrix = this.IsInsideMatrix(cell.Row, cell.Col);
            int currentIndex = (cell.Row * this.columnsCount) + cell.Col;

            if (!isInsideMatrix)
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
        /// Gets an 'image' of the minefield as a matrix of type CellImage.
        /// </summary>
        /// <param name="showAll">Set to 'true' to uncover all mines.</param>
        /// <returns>A matrix of cells of type CellImage.</returns>
        public CellImage[,] GetImage(bool showAll = false)
        {
            var image = ConvertMinefield<CellImage>(c => ConvertCellToImage(c, showAll));

            return image;
        }

        /// <summary>
        /// Converts current cell state to <see cref="CellImage"/>.
        /// </summary>
        /// <param name="currentCell">Cell to be converted.</param>
        /// <param name="showAll">Set to 'true' uncovers mines, else returns flags.</param>
        /// <returns>Current cell state as CellImage.</returns>
        private CellImage ConvertCellToImage(ICell currentCell, bool showAll)
        {
            CellImage currentImage;

            if (currentCell.IsOpened)//(opened[row, col])
            {
                // Num
                currentImage = CellImage.Num;
            }
            else
            {
                if (showAll)
                {
                    // Bomb, NoBomb
                    currentImage = currentCell.IsMined ? CellImage.Bomb : CellImage.NoBomb;
                }
                else
                {
                    // Flagged, NotFlagged
                    currentImage = currentCell.IsFlagged ? CellImage.Flagged : CellImage.NotFlagged;
                }
            }

            return currentImage;
        }

        /// <summary>
        /// Checks if all cells are opened without explosion.
        /// </summary>
        /// <returns>True if all non-mined cells are opened.</returns>
        public bool IsDisarmed()
        {
            return this.GetOpenedCells >= ((this.rowsCount * this.columnsCount) - this.numberOfMines);
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
        /// Shuffles an array of T with Fisher-Yates shuffle algorithm.
        /// </summary>
        /// <typeparam name="T">Generic type.</typeparam>
        /// <param name="array">Array of T to be shuffled randomly.</param>
        private void Shuffle<T>(T[] array)
        {
            int currentIndex = array.Length - 1;
            while (currentIndex > 1)
            {
                int nextRandomIndex = this.randomGenerator.GetRandomNumber(currentIndex);
                T swapValue = array[currentIndex];
                array[currentIndex] = array[nextRandomIndex];
                array[nextRandomIndex] = swapValue;
                currentIndex = currentIndex - 1;
            }
        }

        /// <summary>
        /// Converts minefield matrix to matrix of type T upon given function.
        /// </summary>
        /// <typeparam name="T">Return type of the given function.</typeparam>
        /// <param name="func">Conversion function returning type T.</param>
        /// <returns>Two dimensional array of type T.</returns>
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
        /// <param name="currentPosition">Current cell position in the minefield.</param>
        /// <returns>Neighbor mines count.</returns>
        private int CountNeighborMinesPerCell(ICellPosition currentPosition)
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

        /// <summary>
        /// Disarms a cell and adds a mine to a random empty cell.
        /// Used if the first open cell by the user is a cell with mine.
        /// </summary>
        /// <param name="cellToDisarm">The cell to disarm.</param>
        private void DisarmFirstCell(ICell cellToDisarm)
        {
            // Store every cell without mine
            var emptyCells = new List<ICell>();
            for (int i = 0; i < this.cells.Length; i++)
            {
                if (!this.cells[i].IsMined)
                {
                    emptyCells.Add(this.cells[i]);
                }
            }

            if (emptyCells.Count == 0)
            {
                throw new InvalidOperationException("Cannot disarm a cell because all other cells have mines!");
            }

            // Add mine to a random empty cell
            int j = this.randomGenerator.GetRandomNumber(emptyCells.Count);
            emptyCells[j].AddMine();

            // Disarm the first cell and recalculate the neighbor mines count
            cellToDisarm.Disarm();
            this.allNeighborMines = this.CalculateNeighborMines();
        }

        /// <summary>
        /// Recursively opens all adjacent cells of a cell which has no neighbors with mines.
        /// </summary>
        /// <param name="cellPos">The current cell.</param>
        private void OpenEmptyCellsRecursive(ICellPosition cellPos)
        {
            // All neighbors must not have mines.
            Debug.Assert(this.CountNeighborMinesPerCell(cellPos) == 0, "All neighbors must not have mines!");

            for (int row = -1; row < 2; row++)
            {
                for (int col = -1; col < 2; col++)
                {
                    if (col == 0 && row == 0)
                    {
                        continue;
                    }

                    if (this.IsInsideMatrix(cellPos.Row + row, cellPos.Col + col))
                    {
                        CellPos neighborCellPos = new CellPos(cellPos.Row + row, cellPos.Col + col);
                        int currentIndex = this.GetIndex(neighborCellPos);

                        if (this.cells[currentIndex].IsOpened)
                        {
                            continue;
                        }

                        this.cells[currentIndex].OpenCell();
                        this.openedCellsCount += 1;

                        if (this.CountNeighborMinesPerCell(neighborCellPos) == 0)
                        {
                            this.OpenEmptyCellsRecursive(neighborCellPos);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Converts (row,col) coordinates to index in the cell list.
        /// </summary>
        /// <param name="cell">The cell position.</param>
        /// <returns>The index in the cell list.</returns>
        private int GetIndex(ICellPosition cell)
        {
            int index = (cell.Row * this.columnsCount) + cell.Col;
            return index;
        }
    }
}
