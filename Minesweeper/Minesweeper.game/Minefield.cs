using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    class Minefield
    {
        private bool[,] minefield = new bool[5, 10];
        private bool[,] openedCells = new bool[5, 10];
        private SortedDictionary<int, string> topScores = new SortedDictionary<int, string>();

        public Minefield()
        {
            this.AddMines();
        }

        public MinefieldState OpenNewCell(Cell cell)
        {
            var isInside = this.IsInsideMatrix(cell.Row, cell.Col);
            if (!isInside)
            {
                return MinefieldState.OutOfRange;
            }

            if (openedCells[cell.Row, cell.Col])
            {
                return MinefieldState.AlreadyOpened;
            }
            else
            {
                openedCells[cell.Row, cell.Col] = true;

                if (minefield[cell.Row, cell.Col])
                {
                    return MinefieldState.Boom;
                }

                return MinefieldState.Normal;
            }
        }

        public void EmptyCellOpened(Cell cell, ConsoleManager consoleManager)
        {
            int neighborMinesCount = this.CountNeighborMines(cell);
            consoleManager.DrawOpenCell(cell.Row, cell.Col, neighborMinesCount);
        }

        public void MineBoomed(ConsoleManager consoleManager)
        {
            int numberOfOpenedCells = this.CountOpen() - 1;

            consoleManager.DrawFinalGameField(minefield, openedCells);
            consoleManager.Finish(numberOfOpenedCells);

            string name = Console.ReadLine();
            topScores.Add(numberOfOpenedCells, name);
            consoleManager.DisplayHighScores(topScores);
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
