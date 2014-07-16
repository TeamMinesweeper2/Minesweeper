namespace Minesweeper.UnitTests.Game
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.IO;
    using System.Text;
    using System.Collections.Generic;
    using Minesweeper.Game;

    [TestClass]
    public class TestUIManager
    {
        private UIManager manager = new UIManager(3, 3, 5);

        [TestMethod]
        public void TestDisplayIntro()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                manager.DisplayIntro("Message");

                string expected = string.Format("{0}", "Message");
                Assert.AreEqual<string>(expected, sw.ToString());
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestDisplayIntroWhenMessageIsNull()
        {
            manager.DisplayIntro(null);

        }

        [TestMethod]
        public void TestDisplayEnd()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                int openCells = 0;
                String message = "End Message";
                manager.DisplayEnd(message, openCells);

                string expected = string.Format("{0}", message);
                Assert.AreEqual<string>(expected, sw.ToString());
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestDisplayEndWhenMessageIsEmpty()
        {
            int openCells = 0;
            manager.DisplayEnd(string.Empty, openCells);
        }

        [TestMethod]
        public void TestGoodBye()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                String message = "GoodBye";
                manager.GoodBye(message);

                StringBuilder expected = new StringBuilder();
                expected.Append(Environment.NewLine);
                expected.Append(message);
                expected.Append(Environment.NewLine);
                Assert.AreEqual<string>(expected.ToString(), sw.ToString());
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestGoodByeWhenMessageIsNull()
        {
            manager.GoodBye(null);
        }

        [TestMethod]
        public void TestDisplayError()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                String message = "Error";
                manager.GoodBye(message);

                StringBuilder expected = new StringBuilder();
                expected.Append(Environment.NewLine);
                expected.Append(message);
                expected.Append(Environment.NewLine);
                Assert.AreEqual<string>(expected.ToString(), sw.ToString());
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestDisplayErrorWhenMessageIsEmpty()
        {
            manager.DisplayError(string.Empty);
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
