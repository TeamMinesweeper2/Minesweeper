namespace Minesweeper
{
    public class CmdShowScores : ICommand
    {
        private MinesweeperGame game;

        public CmdShowScores(MinesweeperGame game)
        {
            this.game = game;
        }

        public void Execute()
        {
            this.game.ShowScores();
        }
    }
}
