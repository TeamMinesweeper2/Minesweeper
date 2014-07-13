namespace Minesweeper
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ConsoleManager : IUserInputReader
    {
        private int minefieldCols;
        private int mineFieldRows;
        private int gameFieldWidth;
        private int cmdLineRow;
        private int cmdLineCol;

        private BoardDrawer boardDrawer;

        public ConsoleManager(int minefieldRows, int mineFieldCols, int cmdLineCol)
        {
            this.mineFieldRows = minefieldRows;
            this.minefieldCols = mineFieldCols;
            this.gameFieldWidth = (mineFieldCols * 2) - 1;
            this.cmdLineRow = 8 + minefieldRows;
            this.cmdLineCol = cmdLineCol;
            CellPos minefieldTopLeft = new CellPos(6, 4);
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

        public void GoodBye()
        {
            Console.WriteLine("Good Bye");
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

        public void DisplayError(string errorMsg)
        {
            Console.SetCursorPosition(this.cmdLineCol, this.cmdLineRow);
            Console.WriteLine(errorMsg);
        }

        public void WaitForKey(string promptMsg)
        {
            Console.Write(promptMsg);
            Console.ReadKey();
            this.PrepareForEntry();
        }

        public string ReadInput()
        {
            string command = Console.ReadLine();
            this.PrepareForEntry();
            return command;
        }

        public void DrawInitialGameField(string enterRowColPrompt)
        {
            this.boardDrawer.DrawInitialGameField();
            Console.Write(enterRowColPrompt);
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
