namespace Minesweeper.UnitTests.Game
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Minesweeper.Game;
    using Minesweeper.Game.Enums;
    using Minesweeper.Game.Interfaces;
    using Moq;
    using Minesweeper.Lib;
    using Minesweeper.Lib.Interfaces;

    [TestClass]
    public class UIManagerTest
    {
        private IUIManagerBridge orgManager;
        private Mock<IRenderer> renderer;
        Mock<IUserInputReader> inputReader;
        private IUIManagerBridge mockedManager;

        [TestInitialize]
        public void TestINitialization()
        {
            orgManager = new UIConsoleManager();

            renderer = new Mock<IRenderer>();
            inputReader = new Mock<IUserInputReader>();
            mockedManager = new UIConsoleManager(renderer.Object, inputReader.Object);
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
                orgManager.DrawGameScreen(5, 5);

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
               
                orgManager.DisplayEnd(endState, openCells);

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
            orgManager.DisplayEnd(fakeEnum, openCells);
        }

        [TestMethod]
        public void TestGameExitMethodShouldReturnProperMessage()
        {
            mockedManager.GameExit();
            
            string expected = "Good bye!";
            renderer.Verify(ren => ren.WriteLine(), Times.Exactly(1));

            renderer.Verify(ren => ren.WriteLine(expected), Times.Exactly(1));
        }
        
        [TestMethod]
        public void HandleErrorMethod_CellOutOfRange_ShouldSendProperCallsToIRenderer()
        {
            var error = GameErrors.CellOutOfRange;
            mockedManager.HandleError(error);

            renderer.Verify(ren => ren.ClearLines(It.IsAny<int>(), It.IsAny<int>(), 3));
            renderer.Verify(ren => ren.WriteAt(It.IsAny<int>(), It.IsAny<int>(), string.Empty));
            renderer.Verify(ren => ren.WriteAt(0, It.IsAny<int>(), "Cell is out of range of the minefield!"),
                "First call to WriteAt is not correct.");
            renderer.Verify(ren => ren.Write(" Press any key to continue..."));
        }

        [TestMethod]
        public void HandleErrorMethod_CellAlreadyOpened_ShouldSendProperCallsToIRenderer()
        {
            var error = GameErrors.CellAlreadyOpened;
            mockedManager.HandleError(error);

            renderer.Verify(ren => ren.ClearLines(It.IsAny<int>(), It.IsAny<int>(), 3));
            renderer.Verify(ren => ren.WriteAt(It.IsAny<int>(), It.IsAny<int>(), string.Empty));
            renderer.Verify(ren => ren.WriteAt(0, It.IsAny<int>(), "Cell already opened!"),
                "First call to WriteAt is not correct.");
            renderer.Verify(ren => ren.Write(" Press any key to continue..."));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void HandleError_WithFakeEnum_ShouldThrowAnException()
        {
            var error = (GameErrors)int.MaxValue;
            orgManager.HandleError(error);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void DisplayHighScores_WhenArgumentIsNull_SouldThrowAnException()
        {
            orgManager.DisplayHighScores(null);
        }

        [TestMethod]
        public void DisplayHighScores_ShouldSendProperValuesToRenderer()
        {
            var list = new List<KeyValuePair<string, int>>();
            KeyValuePair<string, int> firstPlayer = new KeyValuePair<string, int>("Ivan", 20);
            KeyValuePair<string, int> secondPlayer = new KeyValuePair<string, int>("Gosho", 10);
            list.Add(firstPlayer);
            list.Add(secondPlayer);
            IEnumerable<KeyValuePair<string, int>> topScores = list;
            mockedManager.DisplayHighScores(list);

            renderer.Verify(ren => ren.ClearLines(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()));
            renderer.Verify(ren => ren.WriteAt(0, It.IsAny<int>(), "Scoreboard:"));
            renderer.Verify(ren => ren.WriteLine());
            int place = 1;
            foreach (var result in topScores)
            {
                renderer.Verify(ren => ren.WriteLine("{0}. {1} --> {2} cells", place, result.Key, result.Value));
                place++;
            }
        }

        [Ignore]
        public void TestReadName()
        {
        }
    }
}