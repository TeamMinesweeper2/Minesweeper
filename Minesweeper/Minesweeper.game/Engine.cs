namespace Minesweeper
{
    using Minesweeper.Lib;

    public class Engine
    {
        public void Run()
        {
            MinesweeperGame game = new MinesweeperGame();
            IUserInputReader inputReader = new ConsoleReader();
            CommandParser commandParser = new CommandParser(game);
            CommandExecutor cmdExecutor = new CommandExecutor();
          
            // Start game loop
            bool gameRunning = true;
            while (gameRunning)
            {
                string input = inputReader.ReadLine();

                ICommand command = commandParser.ParseCommand(input);

                gameRunning = cmdExecutor.ExecuteCommand(command);
            }
        }
    }
}
