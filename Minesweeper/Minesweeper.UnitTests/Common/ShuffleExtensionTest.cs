namespace Minesweeper.UnitTests.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Minesweeper.Lib;
    using Moq;

    [TestClass]
    public class ShuffleExtensionTest
    {
        private IEnumerable<int> testArray;
        private Mock<IRandomGeneratorProvider> rgp;

        [TestInitialize]
        public void TestInitialize()
        {
            this.testArray = new int[5]{1, 2, 3, 4, 5};
            this.rgp = new Mock<IRandomGeneratorProvider>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Shuffle accepted incorrectly null value for random generator provider.")]
        public void ShuffleWithNullRandomGeneratorProviderShouldThrowAnException()
        {
            testArray.Shuffle(null);
        }

        [TestMethod]
        public void ShuffleWithMockedRandomGeneratorProviderShouldShuffleCorrectly()
        {
            this.rgp.Setup(r => r.Next(It.IsAny<int>())).Returns(new Queue<int>(new[] { 0, 3, 1, 2, 4 }).Dequeue);
            var resultArray = testArray.Shuffle(this.rgp.Object);
            var expected = new List<int>(){1, 4, 2, 3, 5};
            CollectionAssert.AreEqual(expected, resultArray.ToList(), "Shuffling is not correct. Result should be 1, 4, 2, 3, 5");
        }
    }
}
