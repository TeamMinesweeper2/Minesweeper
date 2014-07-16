namespace Minesweeper
{
    using System;
    using Minesweeper.Lib;

    public class CmdFlagCell : ICommand
    {
        private MinesweeperGame game;
        private CellPos targetCell;

        public CmdFlagCell(MinesweeperGame game, CellPos targetCell)
        {
            if (game == null)
            {
                throw new ArgumentNullException("game");
            }

            this.game = game;
            this.targetCell = targetCell;
        }

        public bool Execute()
        {
            this.game.FlagCell(this.targetCell);
            return true;
        }
    }
}
