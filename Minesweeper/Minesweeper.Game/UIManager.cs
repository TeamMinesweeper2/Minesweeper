//-----------------------------------------------------------------------
// <copyright file="UIManager.cs" company="Telerik Academy">
//     Copyright (c) 2014 Telerik Academy. All rights reserved.
// </copyright>
// <summary> User Interface Manager class.</summary>
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

        /// <summary>The board generator which handles drawing of the game board.</summary>
        private readonly BoardDrawer boardGenerator;

        /// <summary>The top left position of the minefield.</summary>
        private readonly ICellPosition minefieldTopLeft;

        /// <summary>The renderer which is going to be used by the application.</summary>
        private readonly IRenderer renderer;

        /// <summary>The user input reader.</summary>
        private readonly IUserInputReader inputReader;

        /// <summary>The message which is going to prompt the user of the expected input.</summary>
        private string prompt;

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
            this.prompt = Messages.EnterRowCol;
            this.minefieldTopLeft = new CellPos(3, 0);
            this.boardGenerator = new BoardDrawer(renderer);
        }

        /// <summary>Draws the game screen.</summary>
        /// <param name="numberOfRows">Number of rows of the minefield.</param>
        /// <param name="numberOfColumns">Number of columns of the minefield.</param>
        public void DrawGameScreen(int numberOfRows, int numberOfColumns)
        {
            // Show game
            this.DrawIntro(Messages.Intro);
            this.DrawTable(numberOfRows, numberOfColumns);
        }
        
        /// <summary>
        /// Displays the ending messages of the game.
        /// </summary>
        /// <param name="gameState">Game end condition.</param>
        /// <param name="numberOfOpenedCells">The number of cells the user opened during the game.</param>
        public void DisplayEnd(GameEndState gameState, int numberOfOpenedCells)
        {
            string message = null;

            if (gameState == GameEndState.Fail)
            {
                message = Messages.Boom;
            }
            else if (gameState == GameEndState.Success)
            {
                message = Messages.Success;
            }
            else
            {
                throw new ArgumentException("Unknown game end state!");
            }

            this.renderer.WriteAt(0, this.cmdLineRow + 1, message, numberOfOpenedCells);
        }

        /// <summary>
        /// Displays the messages when the user quits the game.
        /// </summary>
        public void GameExit()
        {
            this.renderer.WriteLine();
            this.renderer.WriteLine(Messages.Bye);
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

            this.ClearCommandLine(this.prompt);
        }

        /// <summary>
        /// Displays error messages by given enumeration value.
        /// </summary>
        /// <param name="error">The error to be displayed.</param>
        public void HandleError(GameErrors error)
        {
            string errorMsg = null;

            if (error == GameErrors.CellOutOfRange)
            {
                errorMsg = Messages.CellOutOfRange;
            }
            else if (error == GameErrors.CellAlreadyOpened)
            {
                errorMsg = Messages.AlreadyOpened;
            }
            else if (error == GameErrors.InvalidCommand)
            {
                errorMsg = Messages.IvalidCommand;
            }

            this.ValidateMessage(errorMsg);
            this.ClearCommandLine(string.Empty);
            this.renderer.WriteAt(0, this.cmdLineRow, errorMsg);
            this.WaitForKey(" Press any key to continue...");
            this.ClearCommandLine(this.prompt);
        }

        /// <summary>
        /// Reads the player name.
        /// </summary>
        /// <returns>The player name.</returns>
        public string GetPlayerName()
        {
            string name = this.inputReader.ReadLine();
            return name;
        }

        /// <summary>
        /// Draws the game field.
        /// </summary>
        /// <param name="minefield">The minefield to be drawn.</param>
        /// <param name="neighborMines">The minefield with all values of neighboring mines.</param>
        public void DrawGameField(CellImage[,] minefield, int[,] neighborMines)
        {
            this.boardGenerator.DrawGameField(minefield, neighborMines, this.minefieldTopLeft);
            this.ClearCommandLine(this.prompt);
        }

        /// <summary>
        /// Displays the introduction text of the game.
        /// </summary>
        /// <param name="msg">The introduction message.</param>
        private void DrawIntro(string msg)
        {
            this.renderer.WriteAt(0, 0, msg);
        }

        /// <summary>
        /// Draws the game table.
        /// </summary>
        /// <param name="mineFieldRows">The count of the minefield rows.</param>
        /// <param name="minefieldCols">The count of the minefield columns.</param>
        private void DrawTable(int mineFieldRows, int minefieldCols)
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
        private void ClearCommandLine(string commandPrompt)
        {
            this.renderer.ClearLines(0, this.cmdLineRow, 3);
            this.renderer.WriteAt(0, this.cmdLineRow, commandPrompt);
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
