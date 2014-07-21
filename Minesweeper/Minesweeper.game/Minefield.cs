namespace Minesweeper.Game
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
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
        private readonly IList<ICell> cells;

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
            this.cells = this.GenerateMinefield(rows * cols, numberOfMines);            
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
        /// <param name="cellPosition">Cell's position in the minefield matrix.</param>
        /// <returns>State of the minefield.</returns>
        public CellActionResult OpenCellHandler(ICellPosition cellPosition)
        {
            return this.CellInteractionHandler(cellPosition, this.OpenCell);
        }

        /// <summary>
        /// Handles cell flagging.
        /// </summary>
        /// <param name="cellPosition">Cell's position in the minefield matrix.</param>
        /// <returns>State of the minefield.</returns>
        public CellActionResult FlagCellHandler(ICellPosition cellPosition)
        {
            return this.CellInteractionHandler(cellPosition, this.FlagCell);
        }

        /// <summary>
        /// Gets an 'image' of the minefield as a matrix of type CellImage.
        /// </summary>
        /// <param name="showAll">Set to 'true' to uncover all mines.</param>
        /// <returns>A matrix of cells of type CellImage.</returns>
        public CellImage[,] GetImage(bool showAll = false)
        {
            Func<ICell, CellImage> converter = c => this.ConvertCellToImage(c, showAll);
            var image = this.ConvertArrayToMatrix<ICell, CellImage>(this.cells, this.columnsCount, converter);
            
            return image;
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
        /// Handles cell interaction - opening and flagging.
        /// </summary>
        /// <param name="cellPosition">Cell's position in the minefield matrix.</param>
        /// <param name="handler">Handler function - accepts cell position returns boolean.</param>
        /// <returns>State of the minefield.</returns>
        private CellActionResult CellInteractionHandler(ICellPosition cellPosition, Func<ICellPosition, bool> handler)
        {
            var isInsideMatrix = this.IsInsideMatrix(cellPosition.Row, cellPosition.Col);
            int currentIndex = this.GetIndex(cellPosition);

            if (!isInsideMatrix)
            {
                return CellActionResult.OutOfRange;
            }

            if (this.cells[currentIndex].IsOpened)
            {
                return CellActionResult.AlreadyOpened;
            }

            bool steppedOnAMine = handler(cellPosition);

            if (steppedOnAMine)
            {
                return CellActionResult.Boom;
            }

            return CellActionResult.Normal;
        }

        /// <summary>
        /// Handles cell opening. Reveals cell's content and returns it's state.
        /// </summary>
        /// <param name="cellPosition">Cell's position in the minefield matrix.</param>
        /// <returns>State of the minefield.</returns>
        private bool OpenCell(ICellPosition cellPosition)
        {
            bool steppedOnAMine = false;
            int currentIndex = this.GetIndex(cellPosition);

            if (this.cells[currentIndex].IsMined)
            {
                if (this.openedCellsCount == 0)
                {
                    // If the first open cell has mine, swap the mine with an empty cell
                    this.DisarmFirstCell(this.cells[currentIndex]);
                }
                else
                {
                    steppedOnAMine = true;
                }
            }

            if (!steppedOnAMine)
            {
                // Open cell
                this.cells[currentIndex].OpenCell();
                this.openedCellsCount += 1; // Counts opened cells.

                if (this.allNeighborMines[cellPosition.Row, cellPosition.Col] == 0)
                {
                    this.OpenEmptyCellsRecursive(cellPosition);
                }
            }

            return steppedOnAMine;
        }

        /// <summary>
        /// Handles cell flagging.
        /// </summary>
        /// <param name="cellPosition">Cell's position in the minefield matrix.</param>
        /// <returns>State of the minefield.</returns>
        private bool FlagCell(ICellPosition cellPosition)
        {
            bool steppedOnAMine = false;
            int currentIndex = this.GetIndex(cellPosition);
            this.cells[currentIndex].ToggleFlag();

            return steppedOnAMine;
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

            if (currentCell.IsOpened)
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
        /// Generates mines randomly in the minefield matrix.
        /// </summary>
        /// <param name="size">Size of the minefield.</param>
        /// <param name="numberOfMines">Number of mines scattered in the minefield.</param>
        /// <returns>Generated minefield.</returns>
        private IList<ICell> GenerateMinefield(int size, int numberOfMines)
        {
            var buffer = new List<ICell>();

            // Initialize minefield.
            for (int index = 0; index < size; index++)
            {
                buffer.Add(new Cell());

                // Add mines.
                if (index < numberOfMines)
                {
                    buffer[index].AddMine();
                }
            }

            // Shuffle mines.
            var result = buffer.Shuffle(this.randomGenerator);

            return result.ToList();
        }

        /// <summary>
        /// Converts minefield matrix to matrix of type T upon given function.
        /// </summary>
        /// <typeparam name="C">Type of the elements of the input array.</typeparam>
        /// <typeparam name="T">Return type of the given function.</typeparam>
        /// <param name="source">The IList to be converted.</param>
        /// <param name="columns">Number of columns of the return matrix.</param>
        /// <param name="func">Conversion function accepts C and returns T.</param>
        /// <returns>Two dimensional array of type T.</returns>
        private T[,] ConvertArrayToMatrix<C, T>(IList<C> source, int columns, Func<C, T> func)
        {
            int rows = source.Count / columns;
            T[,] result = new T[rows, columns];

            for (int index = 0; index < source.Count; index++)
            {
                int row = index / columns;
                int column = index % columns;
                result[row, column] = func(source[index]);
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
            for (int i = 0; i < this.cells.Count; i++)
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
            int j = this.randomGenerator.Next(emptyCells.Count);
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
            Debug.Assert(this.allNeighborMines[cellPos.Row, cellPos.Col] == 0, "All neighbors must not have mines!");

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

                        if (this.allNeighborMines[neighborCellPos.Row, neighborCellPos.Col] == 0)
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
