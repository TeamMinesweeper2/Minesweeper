namespace Minesweeper
{
    using System;
    using Minesweeper.Game;
    using Minesweeper.Lib;

    /// <summary>
    /// Implements the Execute method by invoking an action on <see cref="MinesweeperGame"/>.
    /// </summary>
    public class CmdOpenCell : ICommand
    {
        /// <summary>
        /// The <see cref="MinesweeperGame"/> object on which the action will be invoked.
        /// </summary>
        private MinesweeperGame game;

        /// <summary>
        /// The <see cref="CellPos"/> of the target cell on which the action will be invoked.
        /// </summary>
        private CellPos cellToOpen;

        /// <summary>
        /// Initializes a new instance of the <see cref="CmdOpenCell"/> class.
        /// </summary>
        /// <param name="game">The <see cref="MinesweeperGame"/> object on which the action will be invoked.</param>
        /// <param name="cellToOpen">The position of the cell on which the action will be invoked.</param>
        public CmdOpenCell(MinesweeperGame game, CellPos cellToOpen)
        {
            if (game == null)
            {
                throw new ArgumentNullException("game");
            }

            this.game = game;
            this.cellToOpen = cellToOpen;
        }

        /// <summary>
        /// Invokes the action on the <see cref="MinesweeperGame"/> object.
        /// </summary>
        /// <returns>Returns true if more commands can be executed.</returns>
        public bool Execute()
        {
            this.game.OpenCell(this.cellToOpen);
            return true;
        }
    }
}
