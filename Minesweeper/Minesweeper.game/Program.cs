namespace Minesweeper
{
    using System;
    using System.Collections.Generic;

    public class MinesweeperGame
    {
        private static bool[,] mineField = new bool[5, 10];
        private static bool[,] openedCells = new bool[5, 10];
        private static SortedDictionary<int, string> topScores = new SortedDictionary<int, string>();
        private static bool isAlive = true;

        public static void Main()
        {
            Console.WriteLine("Welcome to the game “Minesweeper”.\nTry to reveal all cells without mines. Use 'top' to view the scoreboard,\n'restart' to start a new game and 'exit' to quit the game.");

            AddMines();

            while (true)
            {
                Console.WriteLine("\nEnter row and column: ");
                string command = Console.ReadLine();

                if (command.Equals("restart"))
                { 
                    break;
                }

                if (command.Equals("top"))
                { 
                    DisplayHighScores();
                    break;
                }

                if (command.Equals("exit"))
                {
                    break;
                }                    

                if (command.Length < 3)
                { 
                    Console.WriteLine("Illegal input");
                    continue;
                }

                int row = int.Parse(command[0].ToString());
                int col = int.Parse(command[2].ToString());
                Console.WriteLine(row);

                if (openedCells[row, col])
                {
                    Console.WriteLine("Illegal move!");
                }
                else
                {
                    openedCells[row, col] = true;
                    if (mineField[row, col])
                    {
                        isAlive = false;
                        DrawGameField();
                        Console.WriteLine("Booooom! You were killed by a mine. You revealed 2 cells without mines.Please enter your name for the top scoreboard:");
                        string name = Console.ReadLine();
                        topScores.Add(CountOpen() - 1, name);
                        DisplayHighScores();
                        break;
                    }

                    Console.WriteLine(CountNeighborcell(row, col));
                    DrawGameField();
                }

                Console.WriteLine();
            }     

            Console.WriteLine("Good Bye");
        }

        private static void AddMines()
        {
            Random random = new Random();
            for (int i = 0; i < 15; i++)
            {
                int index = random.Next(50);
                while (mineField[(index / 10), (index % 10)])
                {
                    index = random.Next(50);
                }

                mineField[(index / 10), (index % 10)] = true;
            }
        }

        private static void DrawGameField()
        {
            Console.Write("    ");
            for (int col = 0; col < 10; col++)
            {
                Console.Write("{0} ", col);
            }

            Console.WriteLine(string.Empty);
            Console.Write("    ");
            for (int col = 0; col < 21; col++)
            {
                Console.Write("-");
            }

            Console.WriteLine();
            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 13; col++)
                {
                    if (2 <= col && col <= 11)
                    {
                        if (isAlive && !openedCells[row, col - 2])
                        {
                            Console.Write("? ");
                        }
                        else
                        {
                            if (mineField[row, col - 2])
                            {
                                Console.Write("* ");
                            }
                            else
                            {
                                if (openedCells[row, col - 2])
                                {
                                    Console.Write("{0} ", CountNeighborcell(row, col - 2));
                                }                                    
                                else
                                {
                                    Console.Write("- ");
                                }                                    
                            }
                        }
                    }

                    if (col == 1 || col == 12)
                    {
                        Console.Write("| ");
                    }

                    if (col == 0)
                    {
                        Console.Write("{0} ", row);
                    }
                }

                Console.WriteLine();
            }

            Console.Write("    ");
            for (int col = 0; col < 21; col++)
            {
                Console.Write("-");
            }

            Console.WriteLine();
        }

        private static int CountNeighborcell(int i, int j)
        {
            int counter = 0;

            for (int i1 = -1; i1 < 2; i1++)
            {
                for (int j1 = -1; j1 < 2; j1++)
                {
                    if (j1 == 0 && i1 == 0)
                    {
                        continue;
                    }
                        
                    if (IsInsideMatrix(i + i1, j + j1) && mineField[i + i1, j + j1])
                    {
                        counter++;
                    }
                }
            }

            return counter;
        }

        private static void DisplayHighScores()
        {
            Console.WriteLine("Scoreboard:\n");
            var place = 0;
            foreach (var result in topScores)
            {
                Console.WriteLine("{0}. {1} --> {2} cells", place, result.Value, result.Key);
                place++;
            }
        }

        private static bool IsInsideMatrix(int i, int j)
        {
            return (0 <= i && i <= 4) && (0 <= j && j <= 9);
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
