namespace Minesweeper
{
    using System;
    using System.Text;
    using Minesweeper.Lib;

    public class BoardDrawer
    {
        // strings
        private const string TabSpace = "    ";
        private const string CellBomb = "*"; // bomb -> "\u000f";
        private const string CellNoBomb = "-";
        private const string CellClosed = "?";
        private const string CellMarked = "!";

        private int minefieldCols;
        private int mineFieldRows;
        private int gameFieldWidth;
        private CellPos topLeft;

        public BoardDrawer(int minefieldRows, int mineFieldCols, CellPos topLeft)
        {
            this.mineFieldRows = minefieldRows;
            this.minefieldCols = mineFieldCols;
            this.gameFieldWidth = (mineFieldCols * 2) - 1;
            this.topLeft = topLeft;
        }

        public void DrawInitialGameField(int left, int top)
        {
            var gameField = new StringBuilder();
            gameField.AppendLine();

            // Draw first row 
            gameField.Append(TabSpace);
            for (int col = 0; col < this.minefieldCols; col++)
            {
                gameField.AppendFormat("{0} ", col);
            }

            gameField.AppendLine();

            // Draw second row.
            gameField.Append(TabSpace);
            gameField.AppendLine(new string('-', this.gameFieldWidth));

            // Draw minefield rows.
            for (int row = 0; row < this.mineFieldRows; row++)
            {
                gameField.AppendFormat("{0} | ", row);
                for (int col = 0; col < this.minefieldCols; col++)
                {
                    gameField.Append(CellClosed + " ");
                }

                gameField.AppendLine();
            }

            // Draw final row.
            gameField.Append(TabSpace);
            gameField.AppendLine(new string('-', this.gameFieldWidth));

            gameField.AppendLine();

            Console.SetCursorPosition(left, top);
            Console.Write(gameField);
        }

        public void DrawOpenCell(int rowOnField, int colOnField, int neighborMinesCount)
        {
            int rowOnScreen = topLeft.Row + rowOnField;
            int colOnScreen = topLeft.Col + (colOnField * 2);
            this.DrawCell(rowOnScreen, colOnScreen, neighborMinesCount.ToString());
        }

        public void DrawFinalGameField(bool[,] minefield, bool[,] openedCells)
        {
            for (int row = 0; row < minefield.GetLength(0); row++)
            {
                for (int col = 0; col < minefield.GetLength(1); col++)
                {
                    int rowOnScreen = topLeft.Row + row;
                    int colOnScreen = topLeft.Col + (col * 2);
                    if (minefield[row, col])
                    {
                        this.DrawCell(rowOnScreen, colOnScreen, CellBomb);
                    }
                    else if (!openedCells[row, col])
                    {
                        this.DrawCell(rowOnScreen, colOnScreen, CellNoBomb);
                    }
                }
            }
        }

        private void DrawCell(int rowOnScreen, int colOnScreen, string cellValue)
        {
            Console.SetCursorPosition(colOnScreen, rowOnScreen);
            Console.Write(cellValue);
        }
    }
}
