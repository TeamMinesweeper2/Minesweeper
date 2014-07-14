namespace Minesweeper
{
    public class CmdEndGame : ICommand
    {
        private MinesweeperGame game;

        public CmdEndGame(MinesweeperGame game)
        {
            this.game = game;
        }

        public void Execute()
        {
            this.game.EndGame();
        }
    }
}
