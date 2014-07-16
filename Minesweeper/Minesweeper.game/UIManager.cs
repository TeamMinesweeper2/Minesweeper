namespace Minesweeper.Game
{
    using Minesweeper.Lib;
    using System;
    using System.Collections.Generic;

    public class UIManager
    {
        private int cmdLineRow;
        private int cmdLineCol;
        private CellPos minefieldTopLeft;
        private readonly IRenderer renderer;
        private readonly IUserInputReader inputReader;

        private BoardDrawer boardGenerator;

        public UIManager(int minefieldRows, int minefieldCols, int cmdLineCol)
        {
            this.renderer = new ConsoleRenderer();
            this.inputReader = new ConsoleReader();

            this.cmdLineRow = 8 + minefieldRows;
            this.cmdLineCol = cmdLineCol;
            this.minefieldTopLeft = new CellPos(3, 0);
            this.boardGenerator = new BoardDrawer(renderer, minefieldRows, minefieldCols, this.minefieldTopLeft);
        }

        public void DisplayIntro(string msg)
        {
            ValidateMessage(msg);
            this.renderer.WriteAt(0, 0, msg);
        }

        public void DisplayEnd(string msg, int numberOfOpenedCells)
        {
            ValidateMessage(msg);
            this.renderer.WriteAt(0, this.cmdLineRow + 1, msg, numberOfOpenedCells);
        }

        public void GoodBye(string goodByeMsg)
        {
            ValidateMessage(goodByeMsg);
            this.renderer.WriteLine();
            this.renderer.WriteLine(goodByeMsg);
        }

        public void DisplayHighScores(IEnumerable<KeyValuePair<string, int>> topScores)
        {
            if (topScores == null)
            {
                throw new NullReferenceException("Top score list can not be null!");
            }

            // Clear the old board (6 lines)
            this.renderer.ClearLines(0, this.cmdLineRow + 4, 6);

            this.renderer.WriteAt(0, this.cmdLineRow + 4, "Scoreboard:");
            this.renderer.WriteLine();

            var place = 1;
            foreach (var result in topScores)
            {
                this.renderer.WriteLine("{0}. {1} --> {2} cells", place, result.Key, result.Value);
                place++;
            }
        }

        public void DisplayError(string errorMsg)
        {
            ValidateMessage(errorMsg);
            this.renderer.WriteAt(this.cmdLineCol, this.cmdLineRow, errorMsg);
            this.WaitForKey(" Press any key to continue...");
        }

        public string ReadName()
        {
            string name = this.inputReader.ReadLine();
            this.ClearCommandLine();
            return name;
        }

        public void DrawTable(string enterRowColPrompt)
        {
            int left = this.minefieldTopLeft.Col;
            int top = this.minefieldTopLeft.Row;
            this.boardGenerator.DrawTable(left, top);
            this.renderer.Write(enterRowColPrompt);
        }

        public void ClearCommandLine()
        {
            this.renderer.ClearLines(this.cmdLineCol, this.cmdLineRow, 3);
        }

        public void DrawGameField(CellImage[,] minefield, int[,] neighborMines)
        {
            this.boardGenerator.DrawGameField(minefield, neighborMines);
        }

        private void WaitForKey(string promptMsg)
        {
            this.renderer.Write(promptMsg);
            this.inputReader.WaitForKey();
            this.ClearCommandLine();
        }

        private void ValidateMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentException("Message can not be null or empty!");
            }
        }
    }
}
