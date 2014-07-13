using System.Linq;

namespace Minesweeper
{
    internal class CommandReader
    {
        //// TODO: add constants for all commands ("top", "restart"...) - DONE
        private const string Restart = "restart";
        private const string Top = "top";
        private const string Exit = "exit";
        private const int MinCommandLength = 3;

        public CommandReader()
        {
        }

        public Command ReadCommand(IUserInputReader consoleManager, out CellPos cellToOpen)
        {
            string command = consoleManager.ReadInput();
            cellToOpen = CellPos.Empty;

            if (command.Equals(Restart))
            {
                return Command.Restart;
            }

            if (command.Equals(Top))
            {
                return Command.ShowTopScores;
            }

            if (command.Equals(Exit))
            {
                return Command.Exit;
            }

            // TODO: split string and TryParse !!!! - DONE
            var splitedCommands = command.Split(' ');
            splitedCommands =  splitedCommands.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            //check if the input for rows and cols is more/less than 2
            if (splitedCommands.Length == 2)
            {
                int parseCommandInteger;
                //if the parsing was successful assign the parsed integer value to cellToOpen.Row
                if (int.TryParse(splitedCommands[0], out parseCommandInteger))
                {
                    cellToOpen.Row = parseCommandInteger;
                }
                else
                {
                    return Command.Invalid;
                }

                if (int.TryParse(splitedCommands[1], out parseCommandInteger))
                {
                    cellToOpen.Col = parseCommandInteger;
                }
                else
                {
                    return Command.Invalid;
                }
            }
            else
            {
                //if there are more/less than 2 input values the command is invalid
                return Command.Invalid;
            }

            return Command.OpenCell;
        }
    }
}
