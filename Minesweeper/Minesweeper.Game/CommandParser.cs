//-----------------------------------------------------------------------
// <copyright file="CommandParser.cs" company="Telerik Academy">
//     Copyright (c) 2014 Telerik Academy. All rights reserved.
// </copyright>
// <summary> A class that can parse string input and return a command of type <see cref="ICommand"/>.</summary>
//-----------------------------------------------------------------------
namespace Minesweeper.Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Minesweeper.Lib;

    /// <summary>
    /// A class that can parse string input and return a command of type <see cref="ICommand"/>.
    /// </summary>
    public class CommandParser
    {
        /// <summary>
        /// Holds the string commands as keys and their corresponding <see cref="ICommand"/> objects as values.
        /// </summary>
        private readonly Dictionary<string, ICommand> commands;

        /// <summary>
        /// The <see cref="MinesweeperGame"/> object for which commands will be parsed.
        /// </summary>
        private MinesweeperGame game;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandParser"/> class.
        /// </summary>
        /// <param name="game">The <see cref="MinesweeperGame"/> object for which commands will be returned.</param>
        public CommandParser(MinesweeperGame game)
        {
            if (game == null)
            {
                throw new ArgumentNullException();
            }

            this.game = game;

            // Create commands
            ICommand cmdRestart = new CmdRestart(game);
            ICommand cmdBoom = new CmdBoom(game);
            ICommand cmdShowScores = new CmdShowScores(game);
            ICommand cmdEndGame = new CmdExit(game);
            ICommand cmdInvalid = new CmdInvalid(game);

            this.commands = new Dictionary<string, ICommand>()
            {
                { "restart", cmdRestart },
                { "top", cmdShowScores },
                { "exit", cmdEndGame },
                { "boom", cmdBoom },
                { "invalid", cmdInvalid }
            };
        }

        /// <summary>
        /// Gets for the <see cref="MinesweeperGame"/> object for which commands will be parsed.
        /// </summary>
        /// <value>The MinesweeperGame property gets the value of the MinesweeperGame field, game.</value>
        protected MinesweeperGame Game
        {
            get { return this.game; }
        }

        /// <summary>
        /// Parses a string and returns its corresponding <see cref="ICommand"/>.
        /// </summary>
        /// <param name="input">The string to parse.</param>
        /// <returns>The parsed command.</returns>
        public virtual ICommand ParseCommand(string input)
        {            
            input = input.Trim();

            ICommand command;
            if (this.commands.TryGetValue(input, out command))
            {
                return command;
            }

            bool toggleFlag = false;
            if (input.StartsWith("m"))
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
                return this.commands["invalid"];
            }

            CellPos targetCell = CellPos.Empty;
            int parseCommandInteger;

            if (int.TryParse(tokens[0], out parseCommandInteger))
            {
                targetCell.Row = parseCommandInteger;
            }
            else
            {
                return this.commands["invalid"];
            }

            if (int.TryParse(tokens[1], out parseCommandInteger))
            {
                targetCell.Col = parseCommandInteger;
            }
            else
            {
                return this.commands["invalid"];
            }

            // Parsing was successful, the parsed integers are assigned to targetCell
            if (toggleFlag)
            {
                command = new CmdFlagCell(this.game, targetCell);
            }
            else
            {
                command = new CmdOpenCell(this.game, targetCell);
            }
            
            return command;
        }
    }
}
