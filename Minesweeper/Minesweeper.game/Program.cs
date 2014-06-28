﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gyrmeji
{
    class Telerik
    {
        private static int[,] mineField = new int[5, 10];
        private static bool[,] openedCells = new bool[5, 10];
        private static int[] topCells = new int[5];
        private static string[] topNames = new string[5];
        private static int topCellsCounter = 0;
        private static bool isAlive = true;

        static void Main(string[] argumenti)
        {
            Console.WriteLine("Welcome to the game “Minesweeper”.\nTry to reveal all cells without mines. Use 'top' to view the scoreboard,\n'restart' to start a new game and 'exit' to quit the game.");

            AddMines();

            while (true)
            {
                Console.WriteLine("\nEnter row and column: ");
                string command = (Console.ReadLine());

                if (command.Equals("restart"))
                { break; }

                if (command.Equals("top"))
                { DisplayHighScores(); break; }

                if (command.Equals("exit"))
                    break;

                // MAIN
                if (command.Length < 3)
                { Console.WriteLine("Illegal input"); continue; }
                int row = int.Parse(command[0].ToString());

                int col = int.Parse(command[2].ToString());
                Console.WriteLine(row);

                if (openedCells[row, col])
                {
                    Console.WriteLine("Illegal move!");
                    //continue;
                }
                else
                {
                    openedCells[row, col] = true;
                    if (mineField[row, col] == 1)
                    {
                        isAlive = false;
                        DrawGameField();
                        Console.WriteLine("Booooom! You were killed by a mine. You revealed 2 cells without mines.Please enter your name for the top scoreboard:");
                        string str = Console.ReadLine();
                        topNames[topCellsCounter % 5] = str;
                        topCells[topCellsCounter % 5] = CountOpen() - 1;
                        break;
                    }
                    Console.WriteLine(CountNeighborcell(row, col));
                    DrawGameField();
                    //continue;
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
                while (mineField[(index / 10), (index % 10)] == 1)
                {
                    index = random.Next(50);
                }

                mineField[(index / 10), (index % 10)] = 1;
            }
        }

        private static void DrawGameField()
        {
            Console.Write("    ");
            for (int i = 0; i < 10; i++)
            {
                Console.Write("{0} ", i);
            }
            Console.WriteLine("");

            Console.Write("    ");
            for (int i = 0; i < 21; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine("");

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    if (2 <= j && j <= 11)
                    {
                        if (isAlive && !openedCells[i, j - 2])
                        {
                            Console.Write("? ");
                        }
                        else
                        {
                            if (mineField[i, j - 2] == 1)
                            {
                                Console.Write("* ");
                            }
                            else
                            {
                                if (openedCells[i, j - 2])
                                    Console.Write("{0} ", CountNeighborcell(i, j - 2));
                                else
                                    Console.Write("- ");
                            }
                        }
                    }
                    if (j == 1 || j == 12)
                    {
                        Console.Write("| ");
                    }
                    if (j == 0)
                    {
                        Console.Write("{0} ", i);
                    }

                }
                Console.WriteLine("");
            }

            Console.Write("    ");
            for (int i = 0; i < 21; i++)
            {
                Console.Write("-");
            }

            Console.WriteLine("");
        }

        private static int CountNeighborcell(int i, int j)
        {
            int counter = 0;

            for (int i1 = -1; i1 < 2; i1++)
            {

                for (int j1 = -1; j1 < 2; j1++)
                {
                    if (j1 == 0 && i1 == 0)
                        continue;
                    if (IsInsideMatrix(i + i1, j + j1) && mineField[i + i1, j + j1] == 1)
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
            for (int i = 0; i < (topCellsCounter) % 6; i++)
            {
                Console.WriteLine("{0}. {1} --> {2} cells", i, topNames[i], topCells[i]);
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
                for (int j = 0; j < 10; j++)
                {
                    if (openedCells[i, j])
                        res++;
                }
            return res;
        }
    }
}
