namespace Minesweeper.Game
{
    using System.Text;
    using Minesweeper.Lib;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Class that handles drawing of the game board. This includes the minefield and
    /// </summary>
    public class BoardDrawer
    {
        /// <summary>Value of empty cell.</summary>
        private const string EmptyCell = " ";

        /// <summary>Format for column enumeration in table.</summary>
        private const string ColumnEnumerationFormat = "{0} ";

        /// <summary>Format for row enumeration in table.</summary>
        private const string RowEnumerationFormat = "{0} | ";

        /// <summary>Space in intervals occupied by a cell on screen.</summary>
        private const int CellSpaceOnScreen = 2;

        /// <summary>Board offset by row relative to table.</summary>
        private const int BoardOffsetByRow = 3;

        /// <summary>Board offset by column relative to table.</summary>
        private const int BoardOffsetByColumn = 4;

        /// <summary>Number of rows of the minefield.</summary>
        private int minefieldCols;

        /// <summary>Number of columns of the minefield.</summary>
        private int mineFieldRows;

        /// <summary>Position of the top left corner of the board.</summary>
        private ICellPosition boardTopLeft;

        /// <summary>Renderer instance.</summary>
        private readonly IRenderer renderer;

        /// <summary>Cell image alphabet.</summary>
        private IReadOnlyDictionary<CellImage, string> symbols = new ReadOnlyDictionary<CellImage, string>(
            new Dictionary<CellImage, string>()
            {
                { CellImage.Bomb, "*" },
                { CellImage.NoBomb, "-" },
                { CellImage.NotFlagged, "#" },
                { CellImage.Flagged, "!" },
            });

        /// <summary>
        /// Initializes a new instance of <see cref="BoardDrawer"/>
        /// </summary>
        /// <param name="renderer">Renderer for the game.</param>
        /// <param name="minefieldRows">Number of rows of the minefield.</param>
        /// <param name="mineFieldCols">Number of columns of the minefield.</param>
        /// <param name="topLeft">Top left coordinates of the board.</param>
        public BoardDrawer(IRenderer renderer, int minefieldRows, int mineFieldCols, ICellPosition topLeft)
        {
            this.mineFieldRows = minefieldRows;
            this.minefieldCols = mineFieldCols;
            this.boardTopLeft = topLeft;
            this.renderer = renderer;
        }

        /// <summary>
        /// Draw table on screen.
        /// </summary>
        /// <param name="left">Left coordinate on screen.</param>
        /// <param name="top">Top coordinate on screen.</param>
        public void DrawTable(int left, int top)
        {
            var gameField = new StringBuilder();
            gameField.AppendLine();

            string tabSpace = "    ";
            int gameFieldWidth = (this.minefieldCols * CellSpaceOnScreen) - 1;

            // Draw first row 
            gameField.Append(tabSpace);
            for (int col = 0; col < this.minefieldCols; col++)
            {
                gameField.AppendFormat(ColumnEnumerationFormat, col);
            }

            gameField.AppendLine();

            // Draw second row.
            gameField.Append(tabSpace);
            gameField.AppendLine(new string('-', gameFieldWidth));

            // Draw minefield rows.
            for (int row = 0; row < this.mineFieldRows; row++)
            {
                gameField.AppendFormat(RowEnumerationFormat, row);
                gameField.AppendLine();
            }

            // Draw final row.
            gameField.Append(tabSpace);
            gameField.AppendLine(new string('-', gameFieldWidth));

            gameField.AppendLine();
            string result = gameField.ToString();
            this.renderer.WriteAt(left, top, result);
        }

        /// <summary>
        /// Draws game minefield on screen via given IRenderer.
        /// </summary>
        /// <param name="minefield">Image of the minefield represented by two dimensional array of CellImage enumeration.</param>
        /// <param name="neighborMines">Two dimensional array of numbers representing neighbor mines for each cell.</param>
        public void DrawGameField(CellImage[,] minefield, int[,] neighborMines)
        {
            for (int row = 0; row < this.mineFieldRows; row++)
            {
                for (int col = 0; col < this.minefieldCols; col++)
                {
                    int rowOnScreen = boardTopLeft.Row + BoardOffsetByRow + row;
                    int colOnScreen = boardTopLeft.Col + BoardOffsetByColumn + (col * CellSpaceOnScreen);

                    string symbol;
                    var symbolType = minefield[row, col];
                    if (symbolType == CellImage.Num)
                    {
                        int num = neighborMines[row, col];
                        symbol = (num == 0) ? EmptyCell : num.ToString();
                    }
                    else
                    {
                        symbol = this.symbols[symbolType];
                    }

                    this.DrawCell(rowOnScreen, colOnScreen, symbol);
                }
            }
        }

        /// <summary>
        /// Draws cell at given coordinates.
        /// </summary>
        /// <param name="rowOnScreen">Coordinates by row on screen.</param>
        /// <param name="colOnScreen">Coordinates by column on screen.</param>
        /// <param name="cellValue">Value to be drawn.</param>
        private void DrawCell(int rowOnScreen, int colOnScreen, string cellValue)
        {
            this.renderer.WriteAt(colOnScreen, rowOnScreen, cellValue);
        }
    }
}