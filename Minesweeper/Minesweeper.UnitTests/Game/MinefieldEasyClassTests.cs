namespace Minesweeper.UnitTests.Game
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Minesweeper.Game.Enums;
    using Minesweeper.Game;
    using Minesweeper.Lib.Interfaces;
    using Moq;

    [TestClass]
    public class MinefieldEasyClassTests
    {
        /// <summary>Mocking IRandomGeneratorProvider ensures that test are consistent.</summary>
        private static Mock<IRandomGeneratorProvider> randomGenerator;
        private static Mock<ICellPosition> cellPosition;
        private static Minefield testMinefield;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            randomGenerator = new Mock<IRandomGeneratorProvider>();
            cellPosition = new Mock<ICellPosition>();
            randomGenerator.Setup(x => x.Next(5)).Returns(0);
        }
     
        [TestMethod]
        public void OpenCellHandlerShouldReturnCorrectStateEnumerationValueNormalWthChainedOpening()
        {
            // Arrange;
            testMinefield = new MinefieldEasy(5, 5, 2, randomGenerator.Object);

            cellPosition.Setup(x => x.Col).Returns(2);
            cellPosition.Setup(x => x.Row).Returns(2);

            // Act
            var result = testMinefield.OpenCellHandler(cellPosition.Object);
            var opened = testMinefield.OpenedCellsCount;

            // Assert
            Assert.AreEqual(23, opened);
            Assert.AreEqual(CellActionResult.Normal, result);
        }

        [TestMethod]
        public void OpenCellHandlerShouldOpenOneCell()
        {
            // Arrange;
            testMinefield = new MinefieldEasy(5, 5, 2, randomGenerator.Object);
            cellPosition.Setup(x => x.Col).Returns(0);
            cellPosition.Setup(x => x.Row).Returns(0);

            // Act
            var result = testMinefield.OpenCellHandler(cellPosition.Object);
            var opened = testMinefield.OpenedCellsCount;

            // Assert
            Assert.AreEqual(1, opened);
            Assert.AreEqual(CellActionResult.Normal, result);
        }
    }
}
