namespace Minesweeper
{
    using Minesweeper.Game;
    using Minesweeper.Lib;

    public class CmdFlagCell : ICommand
    {
        private MinesweeperGame game;
        private CellPos targetCell;

        public CmdFlagCell(MinesweeperGame game, CellPos targetCell)
        {
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
