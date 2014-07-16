using Minesweeper.Game;

namespace Minesweeper
{
    using System;

    public class CmdRestart : ICommand
    {
        private MinesweeperGame game;

        public CmdRestart(MinesweeperGame game)
        {
            if (game == null)
            {
                throw new ArgumentNullException("game");
            }

            this.game = game;
        }

        public bool Execute()
        {
            this.game.GenerateMinefield();
            return true;
        }
    }
}
