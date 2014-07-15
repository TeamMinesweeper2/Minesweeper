using Minesweeper.Game;

namespace Minesweeper
{
    public class CmdRestart : ICommand
    {
        private MinesweeperGame game;

        public CmdRestart(MinesweeperGame game)
        {
            this.game = game;
        }

        public bool Execute()
        {
            this.game.GenerateMinefield();
            return true;
        }
    }
}
