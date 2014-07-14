namespace Minesweeper
{
    using Minesweeper.Lib;

    public class CmdOpenCell : ICommand
    {
        private MinesweeperGame game;
        private CellPos cellToOpen;

        public CmdOpenCell(MinesweeperGame game, CellPos cellToOpen)
        {
            this.game = game;
            this.cellToOpen = cellToOpen;
        }

        public void Execute()
        {
            this.game.OpenCell(this.cellToOpen);
        }
    }
}
