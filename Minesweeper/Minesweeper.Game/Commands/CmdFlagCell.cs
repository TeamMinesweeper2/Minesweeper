namespace Minesweeper
{
    using System;
    using Minesweeper.Game;
    using Minesweeper.Lib;

    /// <summary>
    /// Implements the Execute method by invoking an action on <see cref="MinesweeperGame"/>.
    /// </summary>
    public class CmdFlagCell : ICommand
    {
        /// <summary>
        /// The <see cref="MinesweeperGame"/> object on which the action will be invoked.
        /// </summary>
        private MinesweeperGame game;

        /// <summary>
        /// The <see cref="CellPos"/> of the target cell on which the action will be invoked.
        /// </summary>
        private CellPos targetCell;

        /// <summary>
        /// Initializes a new instance of the <see cref="CmdFlagCell"/> class.
        /// </summary>
        /// <param name="game">The <see cref="MinesweeperGame"/> object on which the action will be invoked.</param>
        /// <param name="targetCell">The position of the cell on which the action will be invoked.</param>
        public CmdFlagCell(MinesweeperGame game, CellPos targetCell)
        {
            if (game == null)
            {
                throw new ArgumentNullException("game");
            }

            this.game = game;
            this.targetCell = targetCell;
        }

        /// <summary>
        /// Invokes the action on the <see cref="MinesweeperGame"/> object.
        /// </summary>
        /// <returns>Returns true if more commands can be executed.</returns>
        public bool Execute()
        {
            this.game.FlagCell(this.targetCell);
            return true;
        }
    }
}
