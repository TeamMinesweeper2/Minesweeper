using Minesweeper.Game;

namespace Minesweeper
{
    using System;

    public class CmdShowScores : ICommand
    {
        private MinesweeperGame game;

        public CmdShowScores(MinesweeperGame game)
        {
            if (game == null)
            {
                throw new ArgumentNullException("game");
            }

            this.game = game;
        }

        public bool Execute()
        {
            this.game.ShowScores();
            return true;
        }
    }
}
