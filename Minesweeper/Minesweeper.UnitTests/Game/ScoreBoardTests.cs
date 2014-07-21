using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minesweeper.Game;

namespace Minesweeper.UnitTests.Game
{
    [TestClass]
    public class ScoreBoardTests
    {
        [TestMethod]
        public void Test_ScoreBoard_AddScore()
        {
            var scoreBoard = new ScoreBoard();
            scoreBoard.AddScore("test", 10);

            var actual = scoreBoard.TopScores.First();
            var expected = new KeyValuePair<string, int>("test", 10);

            Assert.AreEqual<KeyValuePair<string, int>>(expected, actual);
        }
    }
}
