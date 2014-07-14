namespace Minesweeper
{
    using System.Linq;
    using Minesweeper.Lib;
    using System.Collections.Generic;

    internal class CommandReader
    {
        private readonly Dictionary<string, CommandType> commands = new Dictionary<string, CommandType>()
        {
            { "restart", CommandType.Restart },
            { "top", CommandType.ShowTopScores },
            { "exit", CommandType.Exit },
            { "boom", CommandType.Boom }
        };

        public CommandReader()
        {
        }

        public CommandType ExtractCommand(string input, out CellPos cellToOpen)
        {
            cellToOpen = CellPos.Empty;

            CommandType command;
            if (commands.TryGetValue(input, out command))
            {
                return command;
            }

            var tokens = input.Split(' ');
            tokens = tokens.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            if (tokens.Length != 2)
            {
                return CommandType.Invalid;
            }

            int parseCommandInteger;

            if (int.TryParse(tokens[0], out parseCommandInteger))
            {
                cellToOpen.Row = parseCommandInteger;
            }
            else
            {
                return CommandType.Invalid;
            }

            if (int.TryParse(tokens[1], out parseCommandInteger))
            {
                cellToOpen.Col = parseCommandInteger;
            }
            else
            {
                return CommandType.Invalid;
            }

            // Parsing was successful, the parsed integers are assigned to the out parameter cellToOpen
            return CommandType.OpenCell;
        }
    }
}
