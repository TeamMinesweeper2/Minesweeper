//-----------------------------------------------------------------------
// <copyright file="MinefieldEasy.cs" company="Telerik Academy">
//     Copyright (c) 2014 Telerik Academy. All rights reserved.
// </copyright>
// <summary> Minefield class represents matrix of cells with functionality to open all empty neighbor cells.</summary>
//-----------------------------------------------------------------------
namespace Minesweeper.Game
{
    using System.Diagnostics;
    using Minesweeper.Lib;

    /// <summary>
    /// Minefield class represents matrix of cells.
    /// </summary>
    public class MinefieldEasy : Minefield
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MinefieldEasy"/> class.
        /// </summary>
        /// <param name="rows">Number of rows.</param>
        /// <param name="cols">Number of columns.</param>
        /// <param name="numberOfMines">Number of mines.</param>
        /// <param name="rndGenerator">Random generator provider.</param>
        public MinefieldEasy(int rows, int cols, int numberOfMines, IRandomGeneratorProvider rndGenerator)
            : base(rows, cols, numberOfMines, rndGenerator)
        {
        }

        /// <summary>
        /// Handles cell opening (recursively). Reveals cell's content and returns it's state.
        /// </summary>
        /// <param name="cellPosition">Cell's position in the minefield matrix.</param>
        /// <returns>State of the minefield.</returns>
        protected override bool OpenCell(ICellPosition cellPosition)
        {
            bool steppedOnAMine = base.OpenCell(cellPosition);

            if (!steppedOnAMine)
            {
                if (this.AllNeighborMines[cellPosition.Row, cellPosition.Col] == 0)
                {
                    this.OpenEmptyCellsRecursive(cellPosition);
                }
            }

            return steppedOnAMine;
        }

        /// <summary>
        /// Recursively opens all adjacent cells of a cellPosition which has no neighbors with mines.
        /// </summary>
        /// <param name="cellPos">The current cellPosition.</param>
        private void OpenEmptyCellsRecursive(ICellPosition cellPos)
        {
            // All neighbors must not have mines.
            Debug.Assert(this.AllNeighborMines[cellPos.Row, cellPos.Col] == 0, "All neighbors must not have mines!");

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

                        if (this.Cells[currentIndex].IsOpened)
                        {
                            continue;
                        }

                        this.Cells[currentIndex].OpenCell();
                        this.OpenedCellsCount += 1;

                        if (this.AllNeighborMines[neighborCellPos.Row, neighborCellPos.Col] == 0)
                        {
                            this.OpenEmptyCellsRecursive(neighborCellPos);
                        }
                    }
                }
            }
        }
    }
}
