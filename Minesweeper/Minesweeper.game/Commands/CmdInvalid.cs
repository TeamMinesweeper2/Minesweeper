namespace Minesweeper
{
    using System;
    using Minesweeper.Game;

    /// <summary>
    /// Implements the Execute method by invoking an action on <see cref="MinesweeperGame"/>.
    /// </summary>
    public class CmdInvalid : ICommand
    {
        /// <summary>
        /// The <see cref="MinesweeperGame"/> object on which the action will be invoked.
        /// </summary>
        private MinesweeperGame game;

        /// <summary>
        /// Initializes a new instance of the <see cref="CmdInvalid"/> class.
        /// </summary>
        /// <param name="game">The <see cref="MinesweeperGame"/> object on which the action will be invoked.</param>
        public CmdInvalid(MinesweeperGame game)
        {
            if (game == null)
            {
                throw new ArgumentNullException("game");
            }

            this.game = game;
        }

        /// <summary>
        /// Invokes the action on the <see cref="MinesweeperGame"/> object.
        /// </summary>
        /// <returns>Returns true if more commands can be executed.</returns>
        public bool Execute()
        {
            this.game.DisplayError();
            return true;
        }
    }
}
