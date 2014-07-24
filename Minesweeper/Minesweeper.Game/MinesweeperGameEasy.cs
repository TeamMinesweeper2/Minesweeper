//-----------------------------------------------------------------------
// <copyright file="MinesweeperGameEasy.cs" company="Telerik Academy">
//     Copyright (c) 2014 Telerik Academy. All rights reserved.
// </copyright>
// <summary> Overrides the factory method to return an instance of a Minefield.</summary>
//-----------------------------------------------------------------------
namespace Minesweeper.Game
{
    using Minesweeper.Game.Interfaces;
    using Minesweeper.Lib;

    /// <summary>
    /// Overrides the factory method to return an instance of a <see cref="Minefield"/>.
    /// </summary>
    public class MinesweeperGameEasy : MinesweeperGame
    {
        /// <summary>The coefficient of how many mines to be placed on the minefield.</summary>
        private const decimal MinesCountCoeficient = 0.16m; // 0.2m -> 10 mines; 0.16m -> 8 mines

        /// <summary>
        /// Initializes a new instance of the <see cref="MinesweeperGameEasy"/> class.
        /// </summary>
        /// <param name="uiManager">UI Manager for the game.</param>
        public MinesweeperGameEasy(IUIManager uiManager)
            : base(uiManager)
        {
        }

        /// <summary>
        /// Factory Method implementation to create a new minefield.
        /// </summary>
        /// <param name="rows">Rows in the minefield.</param>
        /// <param name="cols">Columns in the minefield.</param>
        /// <returns>Returns a new minefield.</returns>
        protected override Minefield CreateMinefield(int rows, int cols)
        {
            int minesCount = (int)(rows * cols * MinesCountCoeficient);
            var randomNumberProvider = RandomGeneratorProvider.GetInstance();
            return new MinefieldEasy(rows, cols, minesCount, randomNumberProvider);
        }
    }
}
