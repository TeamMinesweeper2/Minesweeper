namespace Minesweeper.UnitTests.Game
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.IO;
    using System.Text;
    using System.Collections.Generic;
    using Minesweeper.Game;
    using Moq;
    using Minesweeper.Lib;
    using System.Collections;

    [TestClass]
    public class UIManagerTest
    {
        private static IUIManager manager;
        private static IUIManager mockedManager;
        private static Mock<IRenderer> renderer;
        private static Mock<IUserInputReader> inputReader;

        [ClassInitialize]
        public static void ClassInitialization(TestContext ctx)
        {
            manager = new UIManager();
            renderer = new Mock<IRenderer>();
            inputReader = new Mock<IUserInputReader>();
            mockedManager = new UIManager(renderer.Object, inputReader.Object);
        }

        [TestMethod]
        public void DisplayIntroShouldPrintInTheConsoleProperMessage()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                manager.DisplayIntro("Message");

                string expected = string.Format("{0}", "Message");
                Assert.AreEqual<string>(expected, sw.ToString(), "Printed intro message is not correct.");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Display intro accepted incorrect value null.")]
        public void DisplayIntroWhenMessageIsNullShouldThrowAnException()
        {
            manager.DisplayIntro(null);
        }

        [TestMethod]
        public void DisplayEndShouldPrintInTheConsoleProperMessage()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                int openCells = 0;
                String message = "End Message";
                manager.DisplayEnd(message, openCells);

                string expected = string.Format("{0}", message);
                Assert.AreEqual<string>(expected, sw.ToString(), "DisplayEnd method printed inscorrect message.");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "DisplayEnd method accepted incorrectly negative opened cells.")]
        public void DisplayEndWhenOpenedCellsIsNegativeShouldThrowAnException()
        {
            int openCells = -1;
            manager.DisplayEnd("Test message", openCells);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "DisplayEnd method accepted incorrectly null message.")]
        public void DisplayEndWhenMessageIsNullShouldThrowAnException()
        {
            int openCells = 0;
            manager.DisplayEnd(null, openCells);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "DisplayEnd method accepted incorrectly empty message.")]
        public void DisplayEndWhenMessageIsEmptyShouldThrowAnException()
        {
            int openCells = 0;
            manager.DisplayEnd(string.Empty, openCells);
        }

        [TestMethod]
        public void GoodByeShouldPrintInTheConsoleProperMessage()
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
                Assert.AreEqual<string>(expected.ToString(), sw.ToString(), "Expected message from GoodBye method is not received.");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "GoodBye method accepted incorrectly null value.")]
        public void GoodByeWhenMessageIsNullShouldThrowAnException()
        {
            manager.GoodBye(null);
        }

        [TestMethod]
        public void DisplayErrorShouldPrintInTheConsoleProperMessage()
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
                Assert.AreEqual<string>(expected.ToString(), sw.ToString(), "Expected message from DisplayError method is not received.");
            }
        }

        [TestMethod]
        public void DisplayErrorShouldCallRendererCorrectly()
        {
            mockedManager.DisplayError("Test message!");
            renderer.Verify(r => r.WriteAt(0, It.IsAny<int>(), "Test message!"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "DisplayError method accepted incorrectly empty value.")]
        public void DisplayErrorWhenMessageIsEmptyShouldThrowAnException()
        {
            manager.DisplayError(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "DisplayError method accepted incorrectly null reference.")]
        public void DisplayErrorWhenMessageIsNullShouldThrowAnException()
        {
            manager.DisplayError(null);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException), "DisplayError method accepted incorrectly null value.")]
        public void DisplayHighScoresWhenArgumentIsNullShouldThrowAnException()
        {
            manager.DisplayHighScores(null);
        }

        [TestMethod]
        public void DisplayHighScoresShouldPrintProperListInTheConsole()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                var list = new List<KeyValuePair<string, int>>();
                KeyValuePair<string, int> firstPlayer = new KeyValuePair<string, int>("Ivan", 20);
                KeyValuePair<string, int> secondPlayer = new KeyValuePair<string, int>("Goro", 10);
                list.Add(firstPlayer);
                list.Add(secondPlayer);
                manager.DisplayHighScores(list);

                StringBuilder expected = new StringBuilder();

                expected.Append("Scoreboard:\r\n");
                expected.Append("1. Ivan --> 20 cells\r\n");
                expected.Append("2. Goro --> 10 cells");

                Assert.AreEqual<string>(expected.ToString(), sw.ToString().Trim(), "Expected high scores list from DisplayHighScores method is not received.");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "ClearCommandLine accepted null parameter incorrectly.")]
        public void ClearCommandLineWithNullParameterShouldThrowAnException()
        {
            manager.ClearCommandLine(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "DrawGameField accepted arrays with different dimensions incorrectly.")]
        public void DrawGameFieldWithDifferentMatricesBySizeShouldThrowAnException()
        {
            CellImage[,] minefield = new CellImage[3, 3];
            int[,] neighborMines = new int[5, 3];
            mockedManager.DrawGameField(minefield, neighborMines);
        }


        [Ignore]
        public void TestReadName()
        {
        }
    }
}