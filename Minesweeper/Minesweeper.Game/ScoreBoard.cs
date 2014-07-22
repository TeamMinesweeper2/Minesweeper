//-----------------------------------------------------------------------
// <copyright file="ScoreBoard.cs" company="Telerik Academy">
//     Copyright (c) 2014 Telerik Academy. All rights reserved.
// </copyright>
// <summary> ScoreBoard class that stores and updates the top scores for the game.</summary>
//-----------------------------------------------------------------------
namespace Minesweeper.Game
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// ScoreBoard class that stores and updates the top scores for the game.
    /// </summary>
    public class ScoreBoard
    {
        /// <summary>The top scores as a collection of [name, score] KeyValuePairs.</summary>
        private IList<KeyValuePair<string, int>> topScores;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScoreBoard"/> class.
        /// </summary>
        public ScoreBoard()
        {
            this.topScores = new List<KeyValuePair<string, int>>();
        }

        /// <summary>
        /// Gets a sorted list of the top scores as [name, score] pairs.
        /// </summary>
        /// <value>Not accepted.</value>
        public IEnumerable<KeyValuePair<string, int>> TopScores
        {
            get
            {
                // Deep copy
                return this.topScores.Select(item => new KeyValuePair<string, int>(item.Key, item.Value));

                //// Shallow copy
                ////return new List<KeyValuePair<string, int>>(this.topScores);
            }
        }

        /// <summary>
        /// Adds the current player high score.
        /// Player will not be added if the score is lower than the lowest score.
        /// </summary>
        /// <param name="name">Player's name.</param>
        /// <param name="numberOfOpenedCells">The high score.</param>
        public void AddScore(string name, int numberOfOpenedCells)
        {
            this.topScores.Add(new KeyValuePair<string, int>(name, numberOfOpenedCells));
            //// Limit the scoreboard to only the top five players by score
            this.topScores = this.topScores.OrderBy(kvp => -kvp.Value).Take(5).ToList();
        }
    }
}
