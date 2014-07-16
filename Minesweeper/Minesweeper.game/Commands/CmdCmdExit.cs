using Minesweeper.Game;

namespace Minesweeper
{
    using System;

    public class CmdExit : ICommand
    {
        private MinesweeperGame game;

        public CmdExit(MinesweeperGame game)
        {
            if (game == null)
            {
                throw new ArgumentNullException("game");
            }

            this.game = game;
        }

        public bool Execute()
        {
            this.game.ExitGame();
            return false;
        }
    }
}
