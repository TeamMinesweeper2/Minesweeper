using Minesweeper.Game;

namespace Minesweeper
{
    public class CmdExit : ICommand
    {
        private MinesweeperGame game;

        public CmdExit(MinesweeperGame game)
        {
            this.game = game;
        }

        public bool Execute()
        {
            this.game.ExitGame();
            return false;
        }
    }
}
