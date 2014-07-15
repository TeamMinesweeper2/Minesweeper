namespace Minesweeper.UnitTests.Game
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Minesweeper.Lib;
    using Minesweeper.Game;
    using Moq;

    [TestClass]
    public class MinefieldClassTests
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
            randomGenerator.Setup(x => x.GetRandomNumber(5)).Returns(0);
        }

        [TestInitialize]
        public void Initialize()
        {
            testMinefield = new Minefield(5, 5, 2, randomGenerator.Object);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
            "Null parameter for RandomGeneratorProvider is inappropriately allowed.")]
        public void MinefieldConstructorRecievingNullRandomGeneratorProviderShouldThrowAnException()
        {
            // Arrange
            var testMineField = new Minefield(5, 5, 2, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Incorrect row count is inappropriately allowed.")]
        public void MinefieldConstructorRecievingIncorrectRowNumberShouldThrowAnException()
        {
            // Arrange
            var testMineField = new Minefield(0, 5, 2, randomGenerator.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Incorrect column count is inappropriately allowed.")]
        public void MinefieldConstructorRecievingIncorrectColumnNumberShouldThrowAnException()
        {
            // Arrange
            var testMineField = new Minefield(5, 0, 2, randomGenerator.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Incorrect mines count is inappropriately allowed.")]
        public void MinefieldConstructorRecievingIncorrectMinesNumberShouldThrowAnException()
        {
            // Arrange
            var testMineField = new Minefield(5, 5, 0, randomGenerator.Object);
        }

        [TestMethod]
        public void AllNeighborMinesPropertyShouldReturnCorrectTwoDimensionalArray()
        {
            // Arrange
            int[,] expectedNeighborMinesArray = new int[,] {
                {1, 0, 1, 0, 0}, 
                {1, 1, 1, 0, 0},
                {0, 0, 0, 0, 0},
                {0, 0, 0, 1, 1},
                {0, 0, 0, 1, 0}
            };

            // Act
            var neighborMines = testMinefield.AllNeighborMines;

            // Assert
            Assert.IsTrue(expectedNeighborMinesArray.ContentEquals(neighborMines));
        }

        [TestMethod]
        public void OpenCellHandlerShouldMissTheFirstMine()
        {
            // Arrange
            cellPosition.Setup(x => x.Col).Returns(1);
            cellPosition.Setup(x => x.Row).Returns(0);
            
            // Act
            var result = testMinefield.OpenCellHandler(cellPosition.Object);

            // Assert
            Assert.AreNotEqual(MinefieldState.Boom, result);
        }

        [TestMethod]
        public void OpenCellHandlerShouldReturnCorrectStateEnumerationValue()
        {
            // Arrange
            cellPosition.Setup(x => x.Col).Returns(1);
            cellPosition.Setup(x => x.Row).Returns(0);

            var result = testMinefield.OpenCellHandler(cellPosition.Object);

            Assert.AreNotEqual(MinefieldState.Boom, result);

            cellPosition.Setup(x => x.Col).Returns(4);
            cellPosition.Setup(x => x.Row).Returns(4);

            // Act
            result = testMinefield.OpenCellHandler(cellPosition.Object);

            // Assert
            Assert.AreEqual(MinefieldState.Boom, result);
        }
    }
}
