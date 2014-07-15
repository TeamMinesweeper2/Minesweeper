namespace Minesweeper
{
    using System.Linq;
    using Minesweeper.Lib;
    using System.Collections.Generic;

    internal class CommandParser
    {
        private readonly Dictionary<string, ICommand> commands;
        private MinesweeperGame game;

        public CommandParser(MinesweeperGame game)
        {
            this.game = game;
            // Create commands
            ICommand cmdRestart = new CmdRestart(game);
            ICommand cmdBoom = new CmdBoom(game);
            ICommand cmdShowScores = new CmdShowScores(game);
            ICommand cmdEndGame = new CmdEndGame(game);
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

        public ICommand ParseCommand(string input)
        {            
            input = input.Trim();

            ICommand command;
            if (commands.TryGetValue(input, out command))
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
