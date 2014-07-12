namespace Minesweeper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Minefield : IMinefield
    {
        private readonly bool[,] mines;
        private bool[,] openedCells;

        public Minefield(int rows, int columns, IRandomGeneratorProvider randomGenerator)
        {
            mines = new bool[rows, columns];
            openedCells = new bool[rows, columns];
            GenerateMines(randomGenerator);
        }

        public void OpenCell(int row, int column)
        {
            this.openedCells[row, column] = true;
        }

        public bool IsCellOpened(int row, int column)
        {
            return this.openedCells[row, column];
        }

        public bool IsThereMineInCell(int row, int column)
        {
            return this.mines[row, column];
        }

        private void GenerateMines(IRandomGeneratorProvider randomGenerator)
        {
            for (int i = 0; i < 15; i++)
            {
                int index = randomGenerator.GetRandomNumber(50);
                while (this.mines[(index / 10), (index % 10)])
                {
                    index = randomGenerator.GetRandomNumber(50);
                }

                this.mines[(index / 10), (index % 10)] = true;
            }
        }

        private int CountNeighborMines(Position currentPosition)
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
                        mines[currentPosition.Row + row, currentPosition.Col + col])
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
    }
}
