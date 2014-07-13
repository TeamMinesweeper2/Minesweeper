namespace Minesweeper
{
    using System;

    /// <summary>
    /// Holds position by row and column.
    /// </summary>
    public struct CellPos
    {
        /// <summary>
        /// Represents a cell that has Row and Col values set to zero.
        /// </summary>
        public static readonly CellPos Empty = new CellPos(0, 0);

        /// <summary>Set property message format.</summary>
        private const string ExceptionMessageFormat = "Value for {0}'s position cannot be negative";

        /// <summary>Private field for position by row.</summary>
        private int row;

        /// <summary>Private field for position by column.</summary>
        private int col;

        /// <summary>
        /// Initializes a new instance of the <see cref="CellPos"/> class.
        /// </summary>
        /// <param name="initialRow">Position by row.</param>
        /// <param name="initialCol">Position by column.</param>
        public CellPos(int initialRow, int initialCol)
            : this()
        {
            this.Row = initialRow;
            this.Col = initialCol;
        }

        /// <summary>
        /// Gets or sets value for position by row.
        /// </summary>
        /// <value>Position by row.</value>
        public int Row
        {
            get
            {
                return this.row;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(ExceptionMessageFormat, "row");
                }

                this.row = value;
            }
        }

        /// <summary>
        /// Gets or sets value for position by column.
        /// </summary>
        /// <value>Position by column.</value>
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
                    throw new ArgumentOutOfRangeException(ExceptionMessageFormat, "column");
                }

                this.col = value;
            }
        }
    }
}
