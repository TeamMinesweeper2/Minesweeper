namespace Minesweeper.Game
{
    using System;
    using System.Collections.Generic;
    using Minesweeper.Lib;

    /// <summary>
    /// User Interface Manager class.
    /// </summary>
    public class UIManager : IUIManager
    {
        /// <summary>Default value for the <see cref="cmdLineRow"/> field.</summary>
        private const int CmdLineRowDefault = 0;

        /// <summary>The board generator which handles drawing of the game board.</summary>
        private readonly BoardDrawer boardGenerator;

        /// <summary>The top left position of the minefield.</summary>
        private readonly CellPos minefieldTopLeft;

        /// <summary>The renderer which is going to be used by the application.</summary>
        private readonly IRenderer renderer;

        /// <summary>The user input reader.</summary>
        private readonly IUserInputReader inputReader;

        /// <summary>The current command prompt row.</summary>
        private int cmdLineRow;

        /// <summary>
        /// Initializes a new instance of the <see cref="UIManager"/> class with default ConsoleRenderer and ConsoleReader.
        /// </summary>
        public UIManager()
            : this(new ConsoleRenderer(), new ConsoleReader())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIManager"/> class.
        /// </summary>
        /// <param name="renderer">The renderer which is going to be used by the UIManager.</param>
        /// <param name="inputReader">The input reader which UIManager is going to use.</param>
        public UIManager(IRenderer renderer, IUserInputReader inputReader)
        {
            this.renderer = renderer;
            this.inputReader = inputReader;

            this.cmdLineRow = UIManager.CmdLineRowDefault;
            this.minefieldTopLeft = new CellPos(3, 0);
            this.boardGenerator = new BoardDrawer(renderer);
        }

        /// <summary>
        /// Displays the introduction text of the game.
        /// </summary>
        /// <param name="msg">The introduction message.</param>
        public void DisplayIntro(string msg)
        {
            this.ValidateMessage(msg);
            this.renderer.WriteAt(0, 0, msg);
        }

        /// <summary>
        /// Displays the ending messages of the game.
        /// </summary>
        /// <param name="msg">The ending message.</param>
        /// <param name="numberOfOpenedCells">The number of cells the user opened during the game.</param>
        public void DisplayEnd(string msg, int numberOfOpenedCells)
        {
            this.ValidateMessage(msg);
            this.renderer.WriteAt(0, this.cmdLineRow + 1, msg, numberOfOpenedCells);
        }

        /// <summary>
        /// Displays the messages when the user quits the game.
        /// </summary>
        /// <param name="goodByeMsg">The goodbye message.</param>
        public void GoodBye(string goodByeMsg)
        {
            this.ValidateMessage(goodByeMsg);
            this.renderer.WriteLine();
            this.renderer.WriteLine(goodByeMsg);
        }

        /// <summary>
        /// Displays the high scores.
        /// </summary>
        /// <param name="topScores">The top scores to be displayed.</param>
        public void DisplayHighScores(IEnumerable<KeyValuePair<string, int>> topScores)
        {
            if (topScores == null)
            {
                throw new NullReferenceException("Top score list can not be null!");
            }

            // Clear the old board (6 lines)
            this.renderer.ClearLines(0, this.cmdLineRow + 4, 6);

            this.renderer.WriteAt(0, this.cmdLineRow + 4, "Scoreboard:");
            this.renderer.WriteLine();

            var place = 1;
            foreach (var result in topScores)
            {
                this.renderer.WriteLine("{0}. {1} --> {2} cells", place, result.Key, result.Value);
                place++;
            }
        }

        /// <summary>
        /// Displays error messages.
        /// </summary>
        /// <param name="errorMsg">The error message to be displayed.</param>
        public void DisplayError(string errorMsg)
        {
            this.ValidateMessage(errorMsg);
            this.ClearCommandLine(string.Empty);
            this.renderer.WriteAt(0, this.cmdLineRow, errorMsg);
            this.WaitForKey(" Press any key to continue...");
        }

        /// <summary>
        /// Reads the player name.
        /// </summary>
        /// <returns>The player name.</returns>
        public string ReadInput()
        {
            string name = this.inputReader.ReadLine();
            return name;
        }

        /// <summary>
        /// Draws the game table.
        /// </summary>
        /// <param name="mineFieldRows">The count of the minefield rows.</param>
        /// <param name="minefieldCols">The count of the minefield columns.</param>
        public void DrawTable(int mineFieldRows, int minefieldCols)
        {
            int left = this.minefieldTopLeft.Col;
            int top = this.minefieldTopLeft.Row;
            this.boardGenerator.DrawTable(left, top, mineFieldRows, minefieldCols);

            // Update command line position
            this.cmdLineRow = Console.CursorTop;
        }

        /// <summary>
        /// Clears the command line.
        /// </summary>
        /// <param name="commandPrompt">The prompt that is going to be shown to the user.</param>
        public void ClearCommandLine(string commandPrompt)
        {
            this.renderer.ClearLines(0, this.cmdLineRow, 3);
            this.renderer.WriteAt(0, this.cmdLineRow, commandPrompt);
        }

        /// <summary>
        /// Draws the game field.
        /// </summary>
        /// <param name="minefield">The minefield to be drawn.</param>
        /// <param name="neighborMines">The minefield with all values of neighboring mines.</param>
        public void DrawGameField(CellImage[,] minefield, int[,] neighborMines)
        {
            this.boardGenerator.DrawGameField(minefield, neighborMines, this.minefieldTopLeft);
        }

        /// <summary>
        /// Enters a state where the game waits for the user to press any key.
        /// </summary>
        /// <param name="promptMsg">The message to be prompted to the user.</param>
        private void WaitForKey(string promptMsg)
        {
            this.renderer.Write(promptMsg);
            this.inputReader.WaitForKey();
        }

        /// <summary>
        /// Validates the passed message.
        /// </summary>
        /// <param name="message">The message to be validated.</param>
        private void ValidateMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentException("Message can not be null or empty!");
            }
        }
    }
}
