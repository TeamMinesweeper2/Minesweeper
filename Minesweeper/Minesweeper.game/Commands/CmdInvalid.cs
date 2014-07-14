namespace Minesweeper
{
    public class CmdInvalid : ICommand
    {
        private MinesweeperGame game;

        public CmdInvalid(MinesweeperGame game)
        {
            this.game = game;
        }

        public void Execute()
        {
            this.game.DisplayError();
        }
    }
}
