using Minesweeper.Game;

namespace Minesweeper
{
    using System;

    public class CmdInvalid : ICommand
    {
        private MinesweeperGame game;

        public CmdInvalid(MinesweeperGame game)
        {
            if (game == null)
            {
                throw new ArgumentNullException("game");
            }

            this.game = game;
        }

        public bool Execute()
        {
            this.game.DisplayError();
            return true;
        }
    }
}
