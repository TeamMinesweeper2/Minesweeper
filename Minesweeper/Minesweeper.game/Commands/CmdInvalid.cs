using Minesweeper.Game;

namespace Minesweeper
{
    public class CmdInvalid : ICommand
    {
        private MinesweeperGame game;

        public CmdInvalid(MinesweeperGame game)
        {
            this.game = game;
        }

        public bool Execute()
        {
            this.game.DisplayError();
            return true;
        }
    }
}
