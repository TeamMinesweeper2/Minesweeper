namespace Minesweeper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Minefield
    {
        private static bool[,] mines;
        private static bool[,] openedCells;

        public Minefield(int rows, int columns)
        {
            mines = new bool[rows, columns];
            openedCells = new bool[rows, columns];
            GenerateMines();
        }

        private static void GenerateMines(IRandomGeneratorProvider randomGenerator)
        {
            Random random = new Random();
            for (int i = 0; i < 15; i++)
            {
                int index = random.Next(50);
                while (mines[(index / 10), (index % 10)])
                {
                    index = random.Next(50);
                }

                mines[(index / 10), (index % 10)] = true;
            }
        }
    }
}
