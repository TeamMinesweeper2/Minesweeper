namespace Minesweeper
{
    public class CmdRestart : ICommand
    {
        private MinesweeperGame game;

        public CmdRestart(MinesweeperGame game)
        {
            this.game = game;
        }

        public void Execute()
        {
            this.game.GenerateMinefield();
        }
    }
}
