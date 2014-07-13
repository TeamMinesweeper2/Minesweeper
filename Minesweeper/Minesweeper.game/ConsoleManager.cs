namespace Minesweeper
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ConsoleManager : IUserInputReader
    {
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
        private int cmdLineRow;
        private int cmdLineCol;

        private BoardDrawer boardDrawer;

        public ConsoleManager(int minefieldRows, int mineFieldCols)
        {
            this.mineFieldRows = minefieldRows;
            this.minefieldCols = mineFieldCols;
            this.gameFieldWidth = (mineFieldCols * 2) - 1;
            this.cmdLineRow = 8 + minefieldRows;
            this.cmdLineCol = EnterRowColPrompt.Length;
            Cell minefieldTopLeft = new Cell(6, 4);
            this.boardDrawer = new BoardDrawer(minefieldRows, minefieldCols, minefieldTopLeft);
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

        public void ErrorMessage(ErrorType error)
        {
            Console.SetCursorPosition(this.cmdLineCol, this.cmdLineRow);

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

        public void GoodBye()
        {
            Console.WriteLine("Good Bye");
        }



        public string ReadInput()
        {
            string command = Console.ReadLine();
            this.PrepareForEntry();
            return command;
        }



        public void DrawInitialGameField()
        {
            this.boardDrawer.DrawInitialGameField();
            Console.Write(EnterRowColPrompt);
        }

        public void DrawOpenCell(int rowOnField, int colOnField, int neighborMinesCount)
        {
            this.boardDrawer.DrawOpenCell(rowOnField, colOnField, neighborMinesCount);
            this.ResetCursorPosition();
        }

        public void DrawFinalGameField(bool[,] minefield, bool[,] openedCells)
        {
            this.boardDrawer.DrawFinalGameField(minefield, openedCells);
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
            Console.SetCursorPosition(this.cmdLineCol, this.cmdLineRow);
        }
    }
}
