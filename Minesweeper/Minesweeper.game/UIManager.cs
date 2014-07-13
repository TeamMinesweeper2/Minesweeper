namespace Minesweeper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class UIManager : IUserInputReader
    {
        private int minefieldCols;
        private int mineFieldRows;
        private int gameFieldWidth;
        private int cmdLineRow;
        private int cmdLineCol;
        private readonly IRenderer renderer;

        private BoardDrawer boardDrawer;

        public UIManager(int minefieldRows, int mineFieldCols, int cmdLineCol)
        {
            this.renderer = new ConsoleRenderer();
            this.mineFieldRows = minefieldRows;
            this.minefieldCols = mineFieldCols;
            this.gameFieldWidth = (mineFieldCols * 2) - 1;
            this.cmdLineRow = 8 + minefieldRows;
            this.cmdLineCol = cmdLineCol;
            CellPos minefieldTopLeft = new CellPos(6, 4);
            this.boardDrawer = new BoardDrawer(minefieldRows, minefieldCols, minefieldTopLeft);
        }

        public void DisplayIntro(string msg)
        {
            this.renderer.WriteLine(msg);
        }

        public void DisplayEnd(string msg, int numberOfOpenedCells)
        {
            this.renderer.WriteAt(0, this.cmdLineRow + 1, msg, numberOfOpenedCells);
        }      

        public void GoodBye(string goodByeMsg)
        {
            this.renderer.WriteLine();
            this.renderer.WriteLine(goodByeMsg);
        }

        public void DisplayHighScores(IEnumerable<KeyValuePair<string, int>> topScores)
        {
            this.renderer.WriteAt(0, this.cmdLineRow + 4, "Scoreboard:");
            this.renderer.WriteLine();

            var place = 0;
            foreach (var result in topScores)
            {
                this.renderer.WriteLine("{0}. {1} --> {2} cells", place, result.Key, result.Value);
                place++;
            }
        }

        public void DisplayError(string errorMsg)
        {
            this.renderer.WriteAt(this.cmdLineCol, this.cmdLineRow, errorMsg);
        }

        public void WaitForKey(string promptMsg)
        {
            Console.Write(promptMsg);
            Console.ReadKey();
            this.ClearCommandLine();
        }

        public string ReadInput()
        {
            Console.SetCursorPosition(this.cmdLineCol, this.cmdLineRow);
            string command = Console.ReadLine();
            this.ClearCommandLine();
            return command;
        }

        public string ReadName()
        {
            string name = Console.ReadLine();
            return name;
        }

        public void DrawInitialGameField(string enterRowColPrompt)
        {
            Console.SetCursorPosition(0, 3);
            this.boardDrawer.DrawInitialGameField();

            this.renderer.Write(enterRowColPrompt);
        }

        public void DrawOpenCell(int rowOnField, int colOnField, int neighborMinesCount)
        {
            this.boardDrawer.DrawOpenCell(rowOnField, colOnField, neighborMinesCount);
        }

        public void DrawFinalGameField(bool[,] minefield, bool[,] openedCells)
        {
            this.boardDrawer.DrawFinalGameField(minefield, openedCells);
        }

        private void ClearCommandLine()
        {
            string emptyLine = new string(' ', 3 * Console.WindowWidth);

            this.renderer.WriteAt(this.cmdLineCol, this.cmdLineRow, emptyLine);

            Console.SetCursorPosition(this.cmdLineCol, this.cmdLineRow);
        }
    }
}
