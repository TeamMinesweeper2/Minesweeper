namespace Minesweeper
{
    using System;
    using System.Collections.Generic;

    public class MinesweeperGame
    {
        private bool[,] minefield = new bool[5, 10];
        private bool[,] openedCells = new bool[5, 10];
        private SortedDictionary<int, string> topScores = new SortedDictionary<int, string>();
        private bool gameEnded = false;
        private ConsoleManager consoleManager;

        public void Run()
        {
            consoleManager = new ConsoleManager();
            consoleManager.Intro();
            consoleManager.DrawGameField();
            var commandReader = new CommandReader();

            AddMines();

            while (!gameEnded)
            {
                var command = commandReader.ReadCommand(consoleManager);

                switch (command)
                {
                    case Command.Restart:
                        break;
                    case Command.ShowTopScores:
                        consoleManager.DisplayHighScores(topScores);
                        break;
                    case Command.Exit:
                        break;
                    case Command.OpenCell:
                        var cell = commandReader.GetCellToOpen();
                        OpenNewCell(cell);
                        break;
                    case Command.Invalid:
                        consoleManager.ErrorMessage(ErrorType.IllegalInput);
                        break;
                    default:
                        throw new ArgumentException("Unrecognized command!");
                }                
            }

            Console.WriteLine("Good Bye");
        }

        private void OpenNewCell(Cell cell)
        {
            if (openedCells[cell.Row, cell.Col])
            {
                consoleManager.ErrorMessage(ErrorType.IllegalMove);
            }
            else
            {
                openedCells[cell.Row, cell.Col] = true;
                if (minefield[cell.Row, cell.Col])
                {
                    int numberOfOpenedCells = CountOpen() - 1;
                    consoleManager.DrawFinalGameField(minefield, openedCells);
                    consoleManager.Finish(numberOfOpenedCells);
                    string name = Console.ReadLine();
                    topScores.Add(numberOfOpenedCells, name);
                    consoleManager.DisplayHighScores(topScores);
                    gameEnded = true;
                }

                int neighborMinesCount = CountNeighborMines(cell);
                consoleManager.OpenCell(cell.Row, cell.Col, neighborMinesCount);
            }
        }

        private void AddMines()
        {
            Random random = new Random();
            for (int i = 0; i < 15; i++)
            {
                int index = random.Next(50);
                while (minefield[(index / 10), (index % 10)])
                {
                    index = random.Next(50);
                }

                minefield[(index / 10), (index % 10)] = true;
            }
        }

        private int CountNeighborMines(Cell currentPosition) //(int i, int j)
        {
            int counter = 0;

            for (int row = -1; row < 2; row++)
            {
                for (int col = -1; col < 2; col++)
                {
                    if (col == 0 && row == 0)
                    {
                        continue;
                    }

                    if (IsInsideMatrix(currentPosition.Row + row, currentPosition.Col + col) &&
                        minefield[currentPosition.Row + row, currentPosition.Col + col])
                    {
                        counter++;
                    }
                }
            }

            return counter;
        }

        private bool IsInsideMatrix(int row, int col)
        {
            return (0 <= row && row <= 4) && (0 <= col && col <= 9);
        }

        private int CountOpen()
        {
            int res = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (openedCells[i, j])
                    {
                        res++;
                    }
                }
            }

            return res;
        }
    }
}
