namespace Minesweeper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ConsoleManager
    {
        private readonly IPosition minefieldPositionOnScreen = new Position(Constants.MinefieldPositionOnScreenRow, Constants.MinefieldPositionOnScreenCol);
        private readonly IPosition commandEntryOnScreen = new Position(Constants.CommandEntryOnScreenRow, Constants.CommandEntryOnScreenCol);

        public void DrawWelcomeMessage()
        {
            Console.WriteLine("Welcome to the game “Minesweeper”.");
            Console.WriteLine("Try to reveal all cells without mines. Use 'top' to view the scoreboard,");
            Console.WriteLine("'restart' to start a new game and 'exit' to quit the game.");
        }

        public void DrawFinishMessage(int numberOfOpenedCells)
        {
            Console.SetCursorPosition(0, 0); // Place cursor in the beggining
            ClearLinesFromCurrent(3);
            Console.WriteLine("Booooom! You were killed by a mine. You revealed {0} cells without mines.", numberOfOpenedCells);
            Console.WriteLine("Please enter your name for the top scoreboard:");
            Console.WriteLine("Good Bye");
            PrepareForEntry(Constants.EnterNameMessage);
        }

        public void DrawGameField()
        {
            var gameField = new StringBuilder();
            gameField.AppendLine();

            // Draw first row 
            gameField.Append(Constants.TabSpace);
            for (int col = 0; col < Constants.MinefieldWidth; col++)
            {
                gameField.AppendFormat(" {0}", col);
            }

            gameField.AppendLine();

            // Draw second row.
            gameField.Append(Constants.TabSpace);
            gameField.AppendLine(new string('-', Constants.GameFieldWidth));

            // Draw minefield rows.
            for (int row = 0; row < Constants.MineFieldHeight; row++)
            {
                gameField.AppendFormat("{0} | ", row);
                for (int col = 0; col < Constants.MinefieldWidth; col++)
                {                 
                     gameField.Append("? ");
                }
                
                gameField.AppendLine();
            }

            // Draw final row.
            gameField.Append(Constants.TabSpace);
            gameField.AppendLine(new string('-', Constants.GameFieldWidth));
            gameField.AppendLine();

            Console.Write(gameField);
        }

        public void OpenCell(int rowOnField, int colOnField, int neighborMinesCount)
        {
            int rowOnScreen = minefieldPositionOnScreen.Row + rowOnField;
            int colOnScreen = minefieldPositionOnScreen.Col + colOnField * 2;
            DrawChange(rowOnScreen, colOnScreen, neighborMinesCount.ToString());
        }

        public void DrawFinalGameField(bool[,] minefield, bool[,] openedCells)
        {
            for (int row = 0; row < minefield.GetLength(0); row++)
            {
                for (int col = 0; col < minefield.GetLength(1); col++)
                {
                    int rowOnScreen = minefieldPositionOnScreen.Row + row;
                    int colOnScreen = minefieldPositionOnScreen.Col + col * 2;
                    if (minefield[row, col])
                    {
                        DrawChange(rowOnScreen, colOnScreen, "*");
                    }
                    else if (!openedCells[row, col])
                    {
                        DrawChange(rowOnScreen, colOnScreen, "-");
                    }
                }
            }
        }

        public void ErrorMessage(ErrorType error)
        {
            Console.SetCursorPosition(commandEntryOnScreen.Col, commandEntryOnScreen.Row);
            ClearLinesFromCurrent(Constants.NumberOfLinesToClear); 

            if (error == ErrorType.IllegalInput)
            {
                Console.WriteLine(Constants.IllegalInputMessage);
            }
            else if (error == ErrorType.IllegalMove)
            {
                Console.WriteLine(Constants.IllegalMoveMessage);
            }
            else
            {
                throw new ArgumentException("Unknown error message!");
            }

            Console.Write(Constants.PressKeyMessage);
            Console.ReadKey();
            PrepareForEntry(Constants.EnterPositionMessage);
        }

        public void DisplayHighScores(SortedDictionary<int, string> topScores)
        {
            Console.WriteLine("Scoreboard:\n");
            var place = 0;
            foreach (var result in topScores)
            {
                Console.WriteLine(Constants.ScoreboardDisplayFormat, place, result.Value, result.Key);
                place++;
            }

            Console.Write(Constants.PressKeyMessage);
            Console.ReadKey();
            PrepareForEntry(Constants.EnterPositionMessage);
        }

        public void Reset()
        {
            Console.Clear();
        }

        public string UserInput(InputType expectedInput)
        {
            string message = string.Empty;

            if (expectedInput == InputType.Command)
            {
                message = Constants.EnterPositionMessage;
            } 
            else if (expectedInput == InputType.Name)
            {
                message = Constants.EnterNameMessage;
            }
            else
            {
                throw new ArgumentException("Unknown input type expected!");
            }

            PrepareForEntry(message);
            string command = Console.ReadLine();
            return command;
        }

        private void DrawChange(int rowOnScreen, int colOnScreen, string changedValue)
        {
            Console.SetCursorPosition(colOnScreen, rowOnScreen);
            Console.Write(changedValue);
            PrepareForEntry(Constants.EnterPositionMessage); // TODO: Check Need!
        }

        private void PrepareForEntry(string entryMessage)
        {
            Console.SetCursorPosition(commandEntryOnScreen.Col, commandEntryOnScreen.Row);
            ClearLinesFromCurrent(Constants.NumberOfLinesToClear);
            Console.Write(entryMessage);
        }

        private void ClearLinesFromCurrent(int numberOfLines)
        {
            var  currentCursorPosition = new Position(Console.CursorTop, Console.CursorLeft);
            int lineLength = Console.WindowWidth - 1;
            string emptyLine = new string(' ', lineLength * numberOfLines);
            Console.Write(emptyLine);
            Console.SetCursorPosition(currentCursorPosition.Col, currentCursorPosition.Row);
        }
    }
}
