namespace Minesweeper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class GameLogic
    {
        private const int MinefieldRowsCount = 5;
        private const int MinefieldColumnsCount = 10;

        private static SortedDictionary<int, string> topScores = new SortedDictionary<int, string>();
        private IConsoleManager userInteractionManager;
        private IRandomGeneratorProvider randomGenerator;
        private IMinefield minefield;

        public void Initialize()
        {
            userInteractionManager = new ConsoleManager();
            randomGenerator = RandomGenerator.GetInstance();
            minefield = new Minefield(MinefieldRowsCount, MinefieldColumnsCount, randomGenerator);
        }

        public void Update()
        {
            string command = userInteractionManager.UserInput(InputType.Command);

            if (command.Equals("restart"))
            {
                Restart();
            }
            else if (command.Equals("top"))
            {
                userInteractionManager.DisplayHighScores(topScores);
            }
            else if (command.Equals("exit"))
            {
                Environment.Exit(0);
            }
            else if (command.Length < 3)
            {
                userInteractionManager.ErrorMessage(ErrorType.IllegalInput);
            }
            else
            {
                int row = int.Parse(command[0].ToString());
                int col = int.Parse(command[2].ToString());

                if (minefield.IsCellOpened(row, col))
                {
                    userInteractionManager.ErrorMessage(ErrorType.IllegalMove);
                }
                else
                {
                    minefield.OpenCell(row, col);

                    if (minefield.IsThereMineInCell(row, col))
                    {
                        int numberOfOpenedCells = CountOpen() - 1;
                        //userInteractionManager.DrawFinalGameField(minefield, openedCells);
                        userInteractionManager.DrawFinishMessage(numberOfOpenedCells);
                        string name = userInteractionManager.UserInput(InputType.Name);
                        topScores.Add(numberOfOpenedCells, name);
                        userInteractionManager.DisplayHighScores(topScores);
                    }

                    //userInteractionManager.OpenCell(row, col, CountNeighborMines(new Position(row, col)));
                }
            }
        }

        private void Restart()
        {
            throw new NotImplementedException();
        }

        private static int CountOpen()
        {
            int res = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    /*
                    if (openedCells[i, j])
                    {
                        res++;
                    }*/
                }
            }

            return res;
        }
    }
}
