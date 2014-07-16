using Minesweeper.Game;

namespace Minesweeper
{
    using System;

    public class CmdBoom : ICommand
    {
        private MinesweeperGame game;

        public CmdBoom(MinesweeperGame game)
        {
            if (game == null)
            {
                throw new ArgumentNullException("game");  
            }

            this.game = game;
        }

        public bool Execute()
        {
            this.game.MineBoomed();
            return true;
        }
    }
}
