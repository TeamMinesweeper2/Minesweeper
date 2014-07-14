namespace Minesweeper
{
    public class CmdBoom : ICommand
    {
        private MinesweeperGame game;

        public CmdBoom(MinesweeperGame game)
        {
            this.game = game;
        }

        public void Execute()
        {
            this.game.MineBoomed();
        }
    }
}
