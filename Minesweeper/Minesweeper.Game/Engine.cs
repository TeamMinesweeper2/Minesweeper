namespace Minesweeper.Game
{
    using Minesweeper.Lib;

    /// <summary>
    /// A class that runs the main game loop.
    /// </summary>
    public class Engine
    {
        /// <summary>
        /// Runs the main game loop - accepts user input, parses the input and executes the command.
        /// </summary>
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
