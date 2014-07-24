namespace Minesweeper.UnitTests.Game
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.IO;
    using System.Text;
    using System.Collections.Generic;
    using Minesweeper.Game;
    using Minesweeper.Game.Enums;

    [TestClass]
    public class UIManagerTest
    {
        private UIConsoleManager manager;

        [TestInitialize]
        public void TestINitialization()
        {
            manager = new UIConsoleManager();
        }

        [TestMethod]
        public void TestDrawGameScreen()
        {
            string message = "Welcome to the game “Minesweeper”.\n" +
                             "Try to open all cells without mines. Use 'top' to view the scoreboard,\n" +
                             "'restart' to start a new game and 'exit' to quit the game. Use 'm' to flag a cell.\n" +
                             "\r\n    0 1 2 3 4 \r\n" +
                             "    ---------\r\n" +
                             "0 | \r\n" +
                             "1 | \r\n" +
                             "2 | \r\n" +
                             "3 | \r\n" +
                             "4 | \r\n" +
                             "    ---------\r\n\r\n";
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                manager.DrawGameScreen(5, 5);

                string expected = string.Format("{0}", message);
                Assert.AreEqual<string>(expected, sw.ToString());
            }
        }

        [TestMethod]
        public void TestDisplayEnd()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                int openCells = 0;
                var endState = GameEndState.Fail;
               
                manager.DisplayEnd(endState, openCells);

                string expected = "Booooom! You were killed by a mine. You opened 0 cells without mines.\n" +
                                  "Please enter your name for the top scoreboard: ";
                Assert.AreEqual<string>(expected, sw.ToString());
            }
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestDisplayEndWithFakeEnumerationShouldThrowAnExceptiony()
        {
            int openCells = 0;
            var fakeEnum = (GameEndState)int.MaxValue;
            manager.DisplayEnd(fakeEnum, openCells);
        }

        [TestMethod]
        public void TestGameExitMethodShouldReturnProperMessage()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                manager.GameExit();

                StringBuilder expected = new StringBuilder();
                expected.Append(Environment.NewLine);
                expected.Append("Good bye!");
                expected.Append(Environment.NewLine);
                Assert.AreEqual<string>(expected.ToString(), sw.ToString());
            }
        }
        /*
        [TestMethod]
        public void TestHandleErrorMethod()
        {
            using (StringWriter sw = new StringWriter())
                {
                    Console.SetOut(sw);

                    var error = GameErrors.CellOutOfRange;
                    manager.HandleError(error);

                    string expected = "Cell is out of range of the minefield! Press any key to continue...";

                    Assert.AreEqual<string>(expected, sw.ToString());
                }
        }*/

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestHandleErrorWithFakeEnumShouldThrowAnException()
        {
            var error = (GameErrors)int.MaxValue;
            manager.HandleError(error);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestDisplayHighScoresWhenArgumentIsNull()
        {
            manager.DisplayHighScores(null);
        }

        [TestMethod]
        public void TestDisplayHighScores()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                var list = new List<KeyValuePair<string, int>>();
                KeyValuePair<string, int> firstPlayer = new KeyValuePair<string, int>("Ivan", 20);
                KeyValuePair<string, int> secondPlayer = new KeyValuePair<string, int>("Goro", 10);
                list.Add(firstPlayer);
                list.Add(secondPlayer);
                IEnumerable<KeyValuePair<string, int>> topScores = list;
                manager.DisplayHighScores(list);

                StringBuilder expected = new StringBuilder();
                expected.Append(Environment.NewLine);
                expected.Append("Scoreboard:");
                expected.Append(Environment.NewLine);
                int place = 1;

                foreach (var result in topScores)
                {
                    expected.AppendFormat("{0}. {1} --> {2} cells", place++, result.Key, result.Value);
                    expected.Append(Environment.NewLine);
                }
                //expected.Append(Environment.NewLine);
                //Assert.AreEqual<string>(expected.ToString(), sw.ToString());
            }
        }

        [Ignore]
        public void TestReadName()
        {
        }
    }
}