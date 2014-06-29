namespace Minesweeper
{
    using System;

    /// <summary>
    /// Holds position in the game matrix field by row and column.
    /// </summary>
    public class Position
    {
        /// <summary>Set property message format.</summary>
        private const string ExceptionMessageFormat = "Value for {0}'s position cannot be negative";

        /// <summary>Private field for position by row.</summary>
        private int row;

        /// <summary>Private field for position by column.</summary>
        private int col;

        /// <summary>
        /// Initializes a new instance of the <see cref="Position"/> class.
        /// </summary>
        /// <param name="initialRow">Position by row.</param>
        /// <param name="initialCol">Position by column.</param>
        public Position(int initialRow, int initialCol)
        {
            this.Row = initialRow;
            this.Col = initialCol;
        }

        /// <summary>
        /// Gets or sets value for position by row field.
        /// </summary>
        /// <value>Position by row in the game field matrix.</value>
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
        /// Gets or sets value for position by column field.
        /// </summary>
        /// <value>Position by column in the game field matrix.</value>
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
