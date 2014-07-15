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
        private static Mock<IRandomGeneratorProvider> randomGenerator = new Mock<IRandomGeneratorProvider>();
        
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
            randomGenerator.Setup(x => x.GetRandomNumber(5)).Returns(0);
            var testMineField = new Minefield(3, 3, 2, randomGenerator.Object);
            int[,] expectedNeighborMinesArray = new int[,] {
                {1, 0, 1}, 
                {1, 2, 2},
                {0, 1, 0}};

            // Act
            var neighborMines = testMineField.AllNeighborMines;

            // Assert
            Assert.IsTrue(expectedNeighborMinesArray.ContentEquals(neighborMines));
        }
    }
}
