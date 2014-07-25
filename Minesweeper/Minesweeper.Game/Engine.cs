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
            IUIManager consoleUIManager = new UIManager(new ConsoleRenderer(), new ConsoleReader());

            MinesweeperGame game = new MinesweeperGameEasy(consoleUIManager);
            CommandParser commandParser = new CommandParser(game);
            CommandExecutor cmdExecutor = new CommandExecutor();
          
            // Start game loop
            bool gameRunning = true;
            while (gameRunning)
            {
                string input = consoleUIManager.ReadInput();

                ICommand command = commandParser.ParseCommand(input);

                gameRunning = cmdExecutor.ExecuteCommand(command);
            }
        }
    }
}
