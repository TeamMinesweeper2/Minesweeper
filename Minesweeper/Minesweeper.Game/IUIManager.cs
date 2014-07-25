namespace Minesweeper.Game
{
    using System;
    using System.Collections.Generic;

    public interface IUIManager
    {
        void ClearCommandLine(string commandPrompt);
        void DisplayEnd(string msg, int numberOfOpenedCells);
        void DisplayError(string errorMsg);
        void DisplayHighScores(IEnumerable<KeyValuePair<string, int>> topScores);
        void DisplayIntro(string msg);
        void DrawGameField(CellImage[,] minefield, int[,] neighborMines);
        void DrawTable(int mineFieldRows, int minefieldCols);
        void GoodBye(string goodByeMsg);
        string ReadInput();
    }
}
