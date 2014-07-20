namespace Minesweeper.UnitTests.Game
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Minesweeper.Game;
    using Minesweeper.Lib;
    using Moq;
    using System.IO;

    [TestClass]
    public class BoardDrawerTestClass
    {
        private static Mock<IRenderer> testRenderer = new Mock<IRenderer>();
        private static BoardDrawer testBoardDrawer;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            testBoardDrawer = new BoardDrawer(testRenderer.Object);
        }
        /*
        [TestInitialize]
        public void Initialize()
        {
            testRenderer.Setup(tr => tr.WriteAt(3, 3, It.Is<string>(s => s == testTable))).Verifiable();
        }*/

        [TestMethod]
        public void DrawTableShouldSendProperStringToRenderer()
        {
            // Arrange
            var testTable = "\r\n    0 1 2 3 4 \r\n" +
                        "    ---------\r\n" +
                        "0 | \r\n" +
                        "1 | \r\n" +
                        "2 | \r\n" +
                        "3 | \r\n" +
                        "4 | \r\n" +
                        "    ---------\r\n\r\n";
            testRenderer.Setup(tr => tr.WriteAt(3, 3, It.Is<string>(s => s == testTable))).Verifiable();

            // Act
            testBoardDrawer.DrawTable(3, 3, 5, 5);

            // Assert
            testRenderer.Verify();
        }


        [TestMethod]
        public void DrawGameFieldShouldIterateExactTimes()
        {
            // Arrange
            int counter = 0;
            testRenderer.Setup(tr => tr.WriteAt(It.IsInRange(0, 25, Range.Inclusive), It.IsInRange(0, 25, Range.Inclusive), It.IsAny<string>())).Callback(() => counter += 1);

            CellImage[,] sendedImageArray = new CellImage[,] {
                {CellImage.Num, CellImage.Flagged, CellImage.NotFlagged, CellImage.NotFlagged, CellImage.NotFlagged}, 
                {CellImage.NotFlagged, CellImage.NotFlagged, CellImage.NotFlagged, CellImage.NotFlagged, CellImage.NotFlagged},
                {CellImage.NotFlagged, CellImage.NotFlagged, CellImage.NotFlagged, CellImage.NotFlagged, CellImage.NotFlagged},
                {CellImage.NotFlagged, CellImage.NotFlagged, CellImage.NotFlagged, CellImage.NotFlagged, CellImage.NotFlagged},
                {CellImage.NotFlagged, CellImage.NotFlagged, CellImage.NotFlagged, CellImage.NotFlagged, CellImage.NotFlagged}
            };

            int[,] sendedNeighborMinesArray = new int[,] {
                {1, 0, 1, 0, 0}, 
                {1, 1, 1, 0, 0},
                {0, 0, 0, 0, 0},
                {0, 0, 0, 1, 1},
                {0, 0, 0, 1, 0}
            };

            // Act
            testBoardDrawer.DrawGameField(sendedImageArray, sendedNeighborMinesArray, CellPos.Empty);

            // Assert
            Assert.AreEqual(25, counter, "DrawGamefield has not iterated 25 times as it should.");
        }


        [TestMethod]
        public void DrawGameFieldShouldSendProperStringToRenderer()
        {
            // Arrange
            int counter = 0;
            testRenderer.Setup(tr => tr.WriteAt(It.IsInRange(0, 25, Range.Inclusive), It.IsInRange(0, 25, Range.Inclusive), It.Is<string>(s => s.Equals("#")))).Callback(() => counter += 1);

            CellImage[,] sendedImageArray = new CellImage[,] {
                {CellImage.NotFlagged, CellImage.NotFlagged, CellImage.NotFlagged, CellImage.NotFlagged, CellImage.NotFlagged}, 
                {CellImage.NotFlagged, CellImage.NotFlagged, CellImage.NotFlagged, CellImage.NotFlagged, CellImage.NotFlagged},
                {CellImage.NotFlagged, CellImage.NotFlagged, CellImage.NotFlagged, CellImage.NotFlagged, CellImage.NotFlagged},
                {CellImage.NotFlagged, CellImage.NotFlagged, CellImage.NotFlagged, CellImage.NotFlagged, CellImage.NotFlagged},
                {CellImage.NotFlagged, CellImage.NotFlagged, CellImage.NotFlagged, CellImage.NotFlagged, CellImage.NotFlagged}
            };

            int[,] sendedNeighborMinesArray = new int[,] {
                {1, 0, 1, 0, 0}, 
                {1, 1, 1, 0, 0},
                {0, 0, 0, 0, 0},
                {0, 0, 0, 1, 1},
                {0, 0, 0, 1, 0}
            };

            // Act
            testBoardDrawer.DrawGameField(sendedImageArray, sendedNeighborMinesArray, CellPos.Empty);

            // Assert
            Assert.AreEqual(25, counter, "DrawGamefield has not send 25 times required string.");
        }
    }
}