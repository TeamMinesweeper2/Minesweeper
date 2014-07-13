namespace Minesweeper
{
    using System;
    using System.Collections.Generic;

    public class MinesweeperGame
    {
        private static bool[,] minefield = new bool[5, 10];
        private static bool[,] openedCells = new bool[5, 10];
        private static SortedDictionary<int, string> topScores = new SortedDictionary<int, string>();

        public static void Main()
        {
            ConsoleManager.Intro();
            ConsoleManager.DrawGameField();

            AddMines();

            while (true)
            {
                string command = ConsoleManager.CommandInput();

                if (command.Equals("restart"))
                { 
                    break;
                }

                if (command.Equals("top"))
                {
                    ConsoleManager.DisplayHighScores(topScores);
                    break;
                }

                if (command.Equals("exit"))
                {
                    break;
                }                    

                if (command.Length < 3)
                {
                    ConsoleManager.ErrorMessage(ErrorType.IllegalInput);
                    continue;
                }

                int row = int.Parse(command[0].ToString());
                int col = int.Parse(command[2].ToString());

                if (openedCells[row, col])
                {
                    ConsoleManager.ErrorMessage(ErrorType.IllegalMove);
                }
                else
                {
                    openedCells[row, col] = true;
                    if (minefield[row, col])
                    {
                        int numberOfOpenedCells = CountOpen() - 1;
                        ConsoleManager.DrawFinalGameField(minefield, openedCells);
                        ConsoleManager.Finish(numberOfOpenedCells);
                        string name = Console.ReadLine();
                        topScores.Add(numberOfOpenedCells, name);
                        ConsoleManager.DisplayHighScores(topScores);
                        break;
                    }

                    ConsoleManager.OpenCell(row, col, CountNeighborMines(new Position(row, col))); //(row, col));
                    //DrawGameField();
                }
            }     

            Console.WriteLine("Good Bye");
        }

        private static void AddMines()
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

        private static int CountNeighborMines(Position currentPosition) //(int i, int j)
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

        private static bool IsInsideMatrix(int row, int col)
        {
            return (0 <= row && row <= 4) && (0 <= col && col <= 9);
        }

        private static int CountOpen()
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
