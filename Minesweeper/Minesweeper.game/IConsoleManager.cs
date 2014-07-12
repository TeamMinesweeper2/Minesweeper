namespace Minesweeper
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IConsoleManager
    {
        void DrawWelcomeMessage();

        void DrawFinishMessage(int numberOfOpenedCells);

        void DrawGameField();

        void OpenCell(int rowOnField, int colOnField, int neighborMinesCount);

        void DrawFinalGameField(bool[,] minefield, bool[,] openedCells);

        void ErrorMessage(ErrorType error);

        void DisplayHighScores(IDictionary<int, string> topScores);

        void Reset();

        string UserInput(InputType expectedInput);
    }
}