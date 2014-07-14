namespace Minesweeper
{
    using System.Text;
    using Minesweeper.Lib;
    using System.Collections.Generic;

    public class BoardDrawer
    {
        private int minefieldCols;
        private int mineFieldRows;
        private CellPos boardTopLeft;
        private readonly IRenderer renderer;

        private readonly Dictionary<CellImage, string> symbols = new Dictionary<CellImage, string>() 
        {
            { CellImage.Bomb, "*"},
            { CellImage.NoBomb, "-"},
            { CellImage.NotFlagged, "?"},
            { CellImage.Flagged, "!"},
        };

        public BoardDrawer(IRenderer renderer, int minefieldRows, int mineFieldCols, CellPos topLeft)
        {
            this.mineFieldRows = minefieldRows;
            this.minefieldCols = mineFieldCols;
            this.boardTopLeft = topLeft;
            this.renderer = renderer;
        }

        public void DrawTable(int left, int top)
        {
            var gameField = new StringBuilder();
            gameField.AppendLine();

            string tabSpace = "    ";
            int gameFieldWidth = (this.minefieldCols * 2) - 1;

            // Draw first row 
            gameField.Append(tabSpace);
            for (int col = 0; col < this.minefieldCols; col++)
            {
                gameField.AppendFormat("{0} ", col);
            }

            gameField.AppendLine();

            // Draw second row.
            gameField.Append(tabSpace);
            gameField.AppendLine(new string('-', gameFieldWidth));

            // Draw minefield rows.
            for (int row = 0; row < this.mineFieldRows; row++)
            {
                gameField.AppendFormat("{0} | ", row);
                gameField.AppendLine();
            }

            // Draw final row.
            gameField.Append(tabSpace);
            gameField.AppendLine(new string('-', gameFieldWidth));

            gameField.AppendLine();

            this.renderer.WriteAt(left, top, gameField.ToString());
        }

        public void DrawGameField(CellImage[,] minefield, int[,] neighborMines)
        {
            for (int row = 0; row < minefield.GetLength(0); row++)
            {
                for (int col = 0; col < minefield.GetLength(1); col++)
                {
                    int rowOnScreen = boardTopLeft.Row + 3 + row;
                    int colOnScreen = boardTopLeft.Col + 4 + (col * 2);

                    string symbol;
                    if (minefield[row, col] == CellImage.Num)
                    {
                        symbol = neighborMines[row, col].ToString();
                    }
                    else
                    {
                        symbol = this.symbols[minefield[row, col]];
                    }

                    this.DrawCell(rowOnScreen, colOnScreen, symbol);
                }
            }
        }

        private void DrawCell(int rowOnScreen, int colOnScreen, string cellValue)
        {
            this.renderer.WriteAt(colOnScreen, rowOnScreen, cellValue);
        }
    }
}
