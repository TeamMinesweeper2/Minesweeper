namespace Minesweeper
{
    using System;
    using Minesweeper.Lib;

    public class Engine
    {
        public void Run()
        {
            MinesweeperGame game = new MinesweeperGame();
            IUserInputReader inputReader = new ConsoleReader();
            CommandParser commandParser = new CommandParser();
            CommandExecutor cmdExecutor = new CommandExecutor();            

            // Create commands
            ICommand cmdRestart = new CmdRestart(game);
            ICommand cmdBoom = new CmdBoom(game);
            ICommand cmdShowScores = new CmdShowScores(game);
            ICommand cmdEndGame = new CmdEndGame(game);
            ICommand cmdInvalid = new CmdInvalid(game);
            
            // Start game loop
            bool gameEnded = false;
            while (!gameEnded)
            {
                CellPos cellToOpen;
                var input = inputReader.ReadLine();
                var command = commandParser.ParseCommand(input, out cellToOpen);

                ICommand cmdOpenCell = new CmdOpenCell(game, cellToOpen);

                switch (command)
                {
                    case CommandType.Restart:
                        cmdExecutor.ExecuteCommand(cmdRestart);
                        break;
                    case CommandType.ShowTopScores:
                        cmdExecutor.ExecuteCommand(cmdShowScores);
                        break;
                    case CommandType.Exit:
                        cmdExecutor.ExecuteCommand(cmdEndGame);
                        gameEnded = true;
                        break;
                    case CommandType.Invalid:
                        cmdExecutor.ExecuteCommand(cmdInvalid);
                        break;
                    case CommandType.OpenCell:
                        cmdExecutor.ExecuteCommand(cmdOpenCell);
                        break;
                    case CommandType.Boom:
                        cmdExecutor.ExecuteCommand(cmdBoom);
                        break;
                    default:
                        throw new ArgumentException("Unrecognized command!");
                }
            }
        }
    }
}
