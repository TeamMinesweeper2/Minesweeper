namespace Minesweeper
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ConsoleManager
    {
        // const
        private const int TopLeftMinefieldCellOnScreenRow = 6;
        private const int TopLeftMinefieldCellOnScreenCol = 4;          

        // strings
        private const string TabSpace = "    ";
        private const string IvalidCommandMsg = "Ivalid command!";
        private const string AlreadyOpenedMsg = "Cell already opened!";
        private const string CellOutOfRangeMsg = "Cell is out of range of the minefield!";
        private const string PressKeyMessage = "Press any key to continue.";
        private const string EnterRowColPrompt = "Enter row and column: ";

        private int minefieldCols;
        private int mineFieldRows;
        private int gameFieldWidth;
        private int commandEntryOnScreenRow;
        private int commandEntryOnScreenCol;      

        public ConsoleManager(int minefieldRows, int mineFieldCols)
        {
            this.mineFieldRows = minefieldRows;
            this.minefieldCols = mineFieldCols;
            this.gameFieldWidth = (mineFieldCols * 2) - 1;
            this.commandEntryOnScreenRow = 8 + minefieldRows;
            this.commandEntryOnScreenCol = EnterRowColPrompt.Length;  
        }

        public void Intro()
        {
            Console.WriteLine("Welcome to the game “Minesweeper”.");
            Console.WriteLine("Try to reveal all cells without mines. Use 'top' to view the scoreboard,");
            Console.WriteLine("'restart' to start a new game and 'exit' to quit the game.");
        }

        public void Finish(int numberOfOpenedCells)
        {
            Console.WriteLine("Booooom! You were killed by a mine. You revealed {0} cells without mines.", numberOfOpenedCells);
            Console.WriteLine("Please enter your name for the top scoreboard:");
            Console.WriteLine("Good Bye");
        }

        public void DrawInitialGameField()
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
                    gameField.Append("? ");
                }

                gameField.AppendLine();
            }

            // Draw final row.
            gameField.Append(TabSpace);
            gameField.AppendLine(new string('-', this.gameFieldWidth));

            gameField.AppendLine();

            gameField.Append(EnterRowColPrompt);
            Console.Write(gameField);
        }

        public void DrawOpenCell(int rowOnField, int colOnField, int neighborMinesCount)
        {
            int rowOnScreen = TopLeftMinefieldCellOnScreenRow + rowOnField;
            int colOnScreen = TopLeftMinefieldCellOnScreenCol + (colOnField * 2);
            this.DrawCell(rowOnScreen, colOnScreen, neighborMinesCount.ToString());
        }

        public void DrawFinalGameField(bool[,] minefield, bool[,] openedCells)
        {
            for (int row = 0; row < minefield.GetLength(0); row++)
            {
                for (int col = 0; col < minefield.GetLength(1); col++)
                {
                    int rowOnScreen = TopLeftMinefieldCellOnScreenRow + row;
                    int colOnScreen = TopLeftMinefieldCellOnScreenCol + (col * 2);
                    if (minefield[row, col])
                    {
                        this.DrawCell(rowOnScreen, colOnScreen, "*");
                    }
                    else if (!openedCells[row, col])
                    {
                        this.DrawCell(rowOnScreen, colOnScreen, "-");
                    }
                }
            }
        }

        public void ErrorMessage(ErrorType error)
        {
            Console.SetCursorPosition(this.commandEntryOnScreenCol, this.commandEntryOnScreenRow);

            switch (error)
            {
                case ErrorType.CellOutOfRange:
                    Console.WriteLine(CellOutOfRangeMsg);
                    break;
                case ErrorType.AlreadyOpened:
                    Console.WriteLine(AlreadyOpenedMsg);
                    break;
                case ErrorType.IvalidCommand:
                    Console.WriteLine(IvalidCommandMsg);
                    break;
                default:
                    throw new ArgumentException("Unknown error message!");
            }

            Console.Write(PressKeyMessage);
            Console.ReadKey();
            this.PrepareForEntry();
        }

        public void DisplayHighScores(SortedDictionary<int, string> topScores)
        {
            Console.WriteLine("Scoreboard:\n");
            var place = 0;
            foreach (var result in topScores)
            {
                Console.WriteLine("{0}. {1} --> {2} cells", place, result.Value, result.Key);
                place++;
            }
        }

        public string ReadInput()
        {
            string command = Console.ReadLine();
            this.PrepareForEntry();
            return command;
        }

        private void DrawCell(int rowOnScreen, int colOnScreen, string cellValue)
        {
            Console.SetCursorPosition(colOnScreen, rowOnScreen);
            Console.Write(cellValue);
            this.ResetCursorPosition();
        }

        private void PrepareForEntry()
        {
            string emptyLine = new string(' ', Console.WindowWidth);
            Console.Write("\r");
            Console.Write(emptyLine);
            this.ResetCursorPosition();
            Console.Write(emptyLine);
            this.ResetCursorPosition();
        }

        private void ResetCursorPosition()
        {
            Console.SetCursorPosition(this.commandEntryOnScreenCol, this.commandEntryOnScreenRow);
        }
    }
}
