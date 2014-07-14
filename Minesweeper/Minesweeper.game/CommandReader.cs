﻿namespace Minesweeper
{
    using System.Linq;
    using Minesweeper.Lib;
    using System.Collections.Generic;

    internal class CommandReader
    {
        private readonly Dictionary<string, Command> commands = new Dictionary<string, Command>()
        {
            { "restart", Command.Restart },
            { "top", Command.ShowTopScores },
            { "exit", Command.Exit },
            { "boom", Command.Boom }
        };

        public CommandReader()
        {
        }

        public Command ExtractCommand(string input, out CellPos cellToOpen)
        {
            cellToOpen = CellPos.Empty;

            Command command;
            if (commands.TryGetValue(input, out command))
            {
                return command;
            }

            var tokens = input.Split(' ');
            tokens = tokens.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            if (tokens.Length != 2)
            {
                return Command.Invalid;
            }

            int parseCommandInteger;

            if (int.TryParse(tokens[0], out parseCommandInteger))
            {
                cellToOpen.Row = parseCommandInteger;
            }
            else
            {
                return Command.Invalid;
            }

            if (int.TryParse(tokens[1], out parseCommandInteger))
            {
                cellToOpen.Col = parseCommandInteger;
            }
            else
            {
                return Command.Invalid;
            }

            // Parsing was successful, the parsed integers are assigned to the out parameter cellToOpen
            return Command.OpenCell;
        }
    }
}
