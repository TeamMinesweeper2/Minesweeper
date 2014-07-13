﻿namespace Minesweeper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ConsoleManager
    {
        private const int TopLeftMinefieldCellOnScreenRow = 6;
        private const int TopLeftMinefieldCellOnScreenCol = 4;
        private const int CommandEntryOnScreenRow = 13;
        private const int CommandEntryOnScreenCol = 22;
        private const int GameFieldWidth = 21;
        private const int MinefieldWidth = 10;
        private const int MineFieldHeight = 5;
        private const string TabSpace = "    ";
        private const string IvalidCommandMsg = "Ivalid command!";
        private const string AlreadyOpenedMsg = "Cell already opened!";
        private const string CellOutOfRangeMsg = "Cell is out of range of the minefield!";
        private const string PressKeyMessage = "Press any key to continue.";

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
            for (int col = 0; col < MinefieldWidth; col++)
            {
                gameField.AppendFormat("{0} ", col);
            }

            gameField.AppendLine();

            // Draw second row.
            gameField.Append(TabSpace);
            gameField.AppendLine(new string('-', GameFieldWidth));

            // Draw minefield rows.
            for (int row = 0; row < MineFieldHeight; row++)
            {
                gameField.AppendFormat("{0} | ", row);
                for (int col = 0; col < MinefieldWidth; col++)
                {
                    gameField.Append("? ");
                }

                gameField.AppendLine();
            }

            // Draw final row.
            gameField.Append(TabSpace);
            gameField.AppendLine(new string('-', GameFieldWidth));

            gameField.AppendLine();

            gameField.Append("Enter row and column: ");
            Console.Write(gameField);
        }

        public void DrawOpenCell(int rowOnField, int colOnField, int neighborMinesCount)
        {
            int rowOnScreen = TopLeftMinefieldCellOnScreenRow + rowOnField;
            int colOnScreen = TopLeftMinefieldCellOnScreenCol + colOnField * 2;
            DrawCell(rowOnScreen, colOnScreen, neighborMinesCount.ToString());
        }

        public void DrawFinalGameField(bool[,] minefield, bool[,] openedCells)
        {
            for (int row = 0; row < minefield.GetLength(0); row++)
            {
                for (int col = 0; col < minefield.GetLength(1); col++)
                {
                    int rowOnScreen = TopLeftMinefieldCellOnScreenRow + row;
                    int colOnScreen = TopLeftMinefieldCellOnScreenCol + col * 2;
                    if (minefield[row, col])
                    {
                        DrawCell(rowOnScreen, colOnScreen, "*");
                    }
                    else if (!openedCells[row, col])
                    {
                        DrawCell(rowOnScreen, colOnScreen, "-");
                    }
                }
            }
        }

        public void ErrorMessage(ErrorType error)
        {
            Console.SetCursorPosition(CommandEntryOnScreenCol, CommandEntryOnScreenRow);

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
            PrepareForEntry();
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
            PrepareForEntry();
            return command;
        }

        private void DrawCell(int rowOnScreen, int colOnScreen, string cellValue)
        {
            Console.SetCursorPosition(colOnScreen, rowOnScreen);
            Console.Write(cellValue);
            ResetCursorPosition();
        }

        private void PrepareForEntry()
        {
            string emptyLine = new string(' ', Console.WindowWidth);
            Console.Write("\r");
            Console.Write(emptyLine);
            ResetCursorPosition();
            Console.Write(emptyLine);
            ResetCursorPosition();
        }

        private void ResetCursorPosition()
        {
            Console.SetCursorPosition(CommandEntryOnScreenCol, CommandEntryOnScreenRow);
        }
    }
}
