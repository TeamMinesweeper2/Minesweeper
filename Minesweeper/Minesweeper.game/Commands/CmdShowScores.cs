using Minesweeper.Game;

namespace Minesweeper
{
    public class CmdShowScores : ICommand
    {
        private MinesweeperGame game;

        public CmdShowScores(MinesweeperGame game)
        {
            this.game = game;
        }

        public bool Execute()
        {
            this.game.ShowScores();
            return true;
        }
    }
}
