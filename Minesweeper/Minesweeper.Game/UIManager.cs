//-----------------------------------------------------------------------
// <copyright file="UIManager.cs" company="Telerik Academy">
//     Copyright (c) 2014 Telerik Academy. All rights reserved.
// </copyright>
// <summary>User interface manager class.</summary>
//-----------------------------------------------------------------------

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

        /// <summary>Space for vertical tabulation.</summary>
        private const int TabSpace = 4;

        /// <summary>Format of the scoreboard.</summary>
        private const string ScoreboardFormat = "{0}. {1} --> {2} cells";

        /// <summary>The board generator which handles drawing of the game board.</summary>
        private readonly BoardDrawer boardGenerator;

        /// <summary>The top left position of the minefield.</summary>
        private readonly CellPos minefieldTopLeft;

        /// <summary>The renderer used by the application.</summary>
        private IRenderer renderer;

        /// <summary>The user input reader.</summary>
        private IUserInputReader inputReader;

        /// <summary>The current command prompt row.</summary>
        private int cmdLineRow;

        /// <summary>
        /// Initializes a new instance of the <see cref="UIManager"/> class with default ConsoleRenderer and ConsoleReader.
        /// </summary>
        public UIManager() : this(new ConsoleRenderer(), new ConsoleReader())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIManager"/> class.
        /// </summary>
        /// <param name="renderer">The renderer which is going to be used by the UIManager.</param>
        /// <param name="inputReader">The input reader which UIManager is going to use.</param>
        public UIManager(IRenderer renderer, IUserInputReader inputReader)
        {
            this.Renderer = renderer;
            
            this.InputReader = inputReader;

            this.cmdLineRow = CmdLineRowDefault;
            this.minefieldTopLeft = new CellPos(3, 0);
            this.boardGenerator = new BoardDrawer(renderer);
        }

        /// <summary>Gets or sets the user input reader.</summary>
        private IUserInputReader InputReader
        {
            get
            {
                return this.inputReader;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Reference for input reader instance cannot be null!");
                }

                this.inputReader = value;
            }
        }

        /// <summary>Gets or sets the renderer used by the application.</summary>
        private IRenderer Renderer
        {
            get
            {
                return this.renderer;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Reference for renderer instance cannot be null!");
                }

                this.renderer = value;
            }
        }
        
        /// <summary>
        /// Displays the introduction text of the game.
        /// </summary>
        /// <param name="msg">The introduction message.</param>
        public void DisplayIntro(string msg)
        {
            this.ValidateMessage(msg);
            this.Renderer.WriteAt(0, 0, msg);
        }

        /// <summary>
        /// Displays the ending messages of the game.
        /// </summary>
        /// <param name="msg">The ending message.</param>
        /// <param name="numberOfOpenedCells">The number of cells the user opened during the game.</param>
        public void DisplayEnd(string msg, int numberOfOpenedCells)
        {
            if (numberOfOpenedCells < 0)
            {
                throw new ArgumentException("Number of opened cells value cannot be negative.");
            }

            this.ValidateMessage(msg);
            this.Renderer.WriteAt(0, this.cmdLineRow + 1, msg, numberOfOpenedCells);
        }

        /// <summary>
        /// Displays the messages when the user quits the game.
        /// </summary>
        /// <param name="goodByeMsg">The goodbye message.</param>
        public void GoodBye(string goodByeMsg)
        {
            this.ValidateMessage(goodByeMsg);
            this.Renderer.WriteLine();
            this.Renderer.WriteLine(goodByeMsg);
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

            int numberOfLinesToBeCleard = 1;
            this.Renderer.ClearLines(0, this.cmdLineRow + TabSpace, numberOfLinesToBeCleard);

            this.Renderer.WriteAt(0, this.cmdLineRow + TabSpace, "Scoreboard:");
            this.Renderer.WriteLine();

            var place = 1;
            foreach (var result in topScores)
            {
                this.Renderer.WriteLine(ScoreboardFormat, place, result.Key, result.Value);
                place++;
            }

            numberOfLinesToBeCleard = place;
        }

        /// <summary>
        /// Displays error messages.
        /// </summary>
        /// <param name="errorMsg">The error message to be displayed.</param>
        public void DisplayError(string errorMsg)
        {
            this.ValidateMessage(errorMsg);
            this.ClearCommandLine(string.Empty);
            this.Renderer.WriteAt(0, this.cmdLineRow, errorMsg);
            this.WaitForKey(" Press any key to continue...");
        }

        /// <summary>
        /// Reads input from the user.
        /// </summary>
        /// <returns>The user input.</returns>
        public string ReadInput()
        {
            string input = this.InputReader.ReadLine();
            return input;
        }

        /// <summary>
        /// Draws the game table.
        /// </summary>
        /// <param name="minefieldRows">The count of the minefield rows.</param>
        /// <param name="minefieldCols">The count of the minefield columns.</param>
        public void DrawTable(int minefieldRows, int minefieldCols)
        {
            if (minefieldCols <= 0 || minefieldRows <= 0)
            {
                throw new ArgumentException("Values for minefield rows and columns cannot be negative or zero.");
            }

            int left = this.minefieldTopLeft.Col;
            int top = this.minefieldTopLeft.Row;
            this.boardGenerator.DrawTable(left, top, minefieldRows, minefieldCols);

            // Update command line position
            this.cmdLineRow = Console.CursorTop;
        }

        /// <summary>
        /// Clears the command line.
        /// </summary>
        /// <param name="commandPrompt">The prompt that is going to be shown to the user.</param>
        public void ClearCommandLine(string commandPrompt)
        {
            if (string.IsNullOrEmpty(commandPrompt))
            {
                throw new ArgumentNullException("Value for command prompt cannot be null ot empty!");
            }

            this.Renderer.ClearLines(0, this.cmdLineRow, 3);
            this.Renderer.WriteAt(0, this.cmdLineRow, commandPrompt);
        }

        /// <summary>
        /// Draws the game field.
        /// </summary>
        /// <param name="minefield">The minefield to be drawn.</param>
        /// <param name="neighborMines">The minefield with all values of neighboring mines.</param>
        public void DrawGameField(CellImage[,] minefield, int[,] neighborMines)
        {
            if (minefield.GetLength(0) != neighborMines.GetLength(0) ||
                minefield.GetLength(1) != neighborMines.GetLength(1))
            {
                throw new ArgumentException("Matrices dimensions are not equal!");
            }

            this.boardGenerator.DrawGameField(minefield, neighborMines, this.minefieldTopLeft);
        }

        /// <summary>
        /// Enters a state where the game waits for the user to press any key.
        /// </summary>
        /// <param name="promptMsg">The message to be prompted to the user.</param>
        private void WaitForKey(string promptMsg)
        {
            this.Renderer.Write(promptMsg);
            this.InputReader.WaitForKey();
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