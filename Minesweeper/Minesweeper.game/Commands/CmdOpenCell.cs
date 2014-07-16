namespace Minesweeper
{
    using System;
    using Minesweeper.Lib;

    public class CmdOpenCell : ICommand
    {
        private MinesweeperGame game;
        private CellPos cellToOpen;

        public CmdOpenCell(MinesweeperGame game, CellPos cellToOpen)
        {
            if (game == null)
            {
                throw new ArgumentNullException("game");
            }

            this.game = game;
            this.cellToOpen = cellToOpen;
        }

        public bool Execute()
        {
            this.game.OpenCell(this.cellToOpen);
            return true;
        }
    }
}
