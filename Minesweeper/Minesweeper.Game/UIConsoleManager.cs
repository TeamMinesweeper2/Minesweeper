//-----------------------------------------------------------------------
// <copyright file="UIConsoleManager.cs" company="Telerik Academy">
//     Copyright (c) 2014 Telerik Academy. All rights reserved.
// </copyright>
// <summary> User Interface Manager class.</summary>
//-----------------------------------------------------------------------

namespace Minesweeper.Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Minesweeper.Game.Enums;
    using Minesweeper.Game.Interfaces;
    using Minesweeper.Lib;
    using Minesweeper.Lib.Interfaces;

    /// <summary>
    /// User Interface Manager class.
    /// </summary>
    public class UIConsoleManager : IUIManagerBridge
    {
        /// <summary>Default value for the <see cref="cmdLineRow"/> field.</summary>
        private const int CmdLineRowDefault = 0;

        /// <summary> Symbol for command to flag cell. </summary>
        private const string FlagCommandSymbol = "m";

        /// <summary>Message for the client to press any key.</summary>
        private const string PressAnyKeyMessage = " Press any key to continue...";

        /// <summary>
        /// Format to display highscores on screen.
        /// </summary>
        private const string HighScoresDisplayFormat = "{0}. {1} --> {2} cells";

        /// <summary>Space for tabulation.</summary>
        private const int TabSpace = 4;

        /// <summary>The board generator which handles drawing of the game board.</summary>
        private readonly BoardDrawer boardGenerator;

        /// <summary>The top left position of the minefield.</summary>
        private readonly ICellPosition minefieldTopLeft;

        /// <summary>The renderer which is going to be used by the application.</summary>
        private readonly IRenderer renderer;

        /// <summary>The user input reader.</summary>
        private readonly IUserInputReader inputReader;

        /// <summary>The message which is going to prompt the user of the expected input.</summary>
        private readonly string prompt;

        /// <summary>
        /// Holds the string commands, triggered directly from the console.
        /// </summary>
        private readonly IList<string> wordCommands = new List<string>()
        {
            "restart",
            "top",
            "exit",
            "boom"
        };

        /// <summary>The current command prompt row.</summary>
        private int cmdLineRow;

        /// <summary>
        /// Initializes a new instance of the <see cref="UIConsoleManager"/> class with default ConsoleRenderer and ConsoleReader.
        /// </summary>
        public UIConsoleManager() : this(new ConsoleRenderer(), new ConsoleReader())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIConsoleManager"/> class.
        /// </summary>
        /// <param name="renderer">The renderer which is going to be used by the UIConsoleManager.</param>
        /// <param name="inputReader">The input reader which UIConsoleManager is going to use.</param>
        public UIConsoleManager(IRenderer renderer, IUserInputReader inputReader)
        {
            this.renderer = renderer;
            this.inputReader = inputReader;

            this.cmdLineRow = UIConsoleManager.CmdLineRowDefault;
            this.prompt = Messages.EnterRowCol;
            this.minefieldTopLeft = new CellPos(3, 0);
            this.boardGenerator = new BoardDrawer(renderer);
        }

        /// <summary>Triggers 'Reset' command event.</summary>
        public event CommandEventHandler ResetCommandEvent;

        /// <summary>Triggers 'Boom' command event.</summary>
        public event CommandEventHandler BoomCommandEvent;

        /// <summary>Triggers 'Exit' command event.</summary>
        public event CommandEventHandler ExitCommandEvent;

        /// <summary>Triggers 'FlagCell' command event.</summary>
        public event CommandEventHandler FlagCellCommandEvent;

        /// <summary>Triggers 'Invalid command' command event.</summary>
        public event CommandEventHandler InvalidCommandEvent;

        /// <summary>Triggers 'Open cell' command event.</summary>
        public event CommandEventHandler OpenCellCommandEvent;

        /// <summary>Triggers 'Show high scores' command event.</summary>
        public event CommandEventHandler ShowHighScoresCommandEvent;

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
            int numberOfLinesToBeCleardForHighScores = 6;

            if (topScores == null)
            {
                throw new NullReferenceException("Top score list can not be null!");
            }

            this.renderer.ClearLines(0, this.cmdLineRow + TabSpace, numberOfLinesToBeCleardForHighScores);

            this.renderer.WriteAt(0, this.cmdLineRow + TabSpace, "Scoreboard:");
            this.renderer.WriteLine();

            var place = 1;
            foreach (var result in topScores)
            {
                this.renderer.WriteLine(HighScoresDisplayFormat, place, result.Key, result.Value);
                place++;
            }

            this.WaitForKey(PressAnyKeyMessage);
            this.renderer.ClearLines(0, this.cmdLineRow + TabSpace, numberOfLinesToBeCleardForHighScores);
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
            this.WaitForKey(PressAnyKeyMessage);
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
        /// Reads a command from the console and sends it to the parser.
        /// </summary>
        public void ReadCommand()
        {
            string input = this.inputReader.ReadLine();
            this.CommandParser(input.Trim());
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
        /// Parses the command from string, raising the corresponding event.
        /// </summary>
        /// <param name="input">Input string from console.</param>
        private void CommandParser(string input)
        {
            if (this.wordCommands.Contains(input))
            {
                this.HandleDirectCommands(input);
                return;
            }

            bool toggleFlag = false;
            if (input.StartsWith(FlagCommandSymbol))
            {
                // remove the "m"
                input = input.Substring(1);
                toggleFlag = true;
            }

            // Extract row and col
            var tokens = input.Split(' ');
            tokens = tokens.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            if (tokens.Length != 2)
            {
                this.TriggerInvalidCommandEvent();
                return;
            }

            var targetCell = CellPos.Empty;
            int parseCommandInteger;

            if (int.TryParse(tokens[0], out parseCommandInteger))
            {
                targetCell.Row = parseCommandInteger;
            }
            else
            {
                this.TriggerInvalidCommandEvent();
                return;
            }

            if (int.TryParse(tokens[1], out parseCommandInteger))
            {
                targetCell.Col = parseCommandInteger;
            }
            else
            {
                this.TriggerInvalidCommandEvent();
                return;
            }

            // Parsing was successful, the parsed integers are assigned to targetCell
            if (toggleFlag)
            {
                if (this.FlagCellCommandEvent != null)
                {
                    this.FlagCellCommandEvent(this, targetCell);
                    return;
                }
            }
            else
            {
                if (this.OpenCellCommandEvent != null)
                {
                    this.OpenCellCommandEvent(this, targetCell);
                    return;
                }
            }

            throw new ApplicationException(string.Format("Error in command parsing. Command: {0}", input));
        }
           
        /// <summary>
        /// Triggers invalid command event.
        /// </summary>
        private void TriggerInvalidCommandEvent()
        {
            if (this.InvalidCommandEvent != null)
            {
                this.InvalidCommandEvent(this, CellPos.Empty);
            }
        }
  
        /// <summary>
        /// Handles commands typed directly in the console.
        /// </summary>
        /// <param name="input">Typed command.</param>
        private void HandleDirectCommands(string input)
        {
            int index = this.wordCommands.IndexOf(input);

            switch (index)
            {
                case 0:
                    if (this.ResetCommandEvent != null)
                    {
                        this.ResetCommandEvent(this, CellPos.Empty);
                    }

                    return;
                case 1:
                    if (this.ShowHighScoresCommandEvent != null)
                    {
                        this.ShowHighScoresCommandEvent(this, CellPos.Empty);
                    }

                    return;
                case 2:
                    if (this.ExitCommandEvent != null)
                    {
                        this.ExitCommandEvent(this, CellPos.Empty);
                    }

                    return;
                case 3:
                    if (this.BoomCommandEvent != null)
                    {
                        this.BoomCommandEvent(this, CellPos.Empty);
                    }

                    return;
                default:
                    throw new ArgumentException("No event for direct input");
            }
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