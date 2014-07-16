namespace Minesweeper.Game
{
    /// <summary>
    /// Class for the main entry point of the program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The entry point of the console application.
        /// </summary>
        public static void Main()
        {
            var gameEngine = new Engine();
            gameEngine.Run();
        }       
    }
}
