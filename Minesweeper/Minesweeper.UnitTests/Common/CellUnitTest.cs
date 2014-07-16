using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minesweeper.Lib;

namespace Minesweeper.UnitTests.Common
{
    [TestClass]
    public class CellUnitTest
    {
        private readonly Cell firstCell = new Cell();
        private readonly Cell secondCell = new Cell();
        [TestMethod]
        public void TestConstructorCell()
        {
            Assert.ReferenceEquals(firstCell, secondCell);
        }

        [TestMethod]
        public void TestIsOpenPropertyInitialization()
        {
            Assert.IsFalse(firstCell.IsOpened);
        }

        [TestMethod]
        public void TestIsMinedPropertyInitialization()
        {
            Assert.IsFalse(firstCell.IsMined);
        }

        [TestMethod]
        public void TestIsFalggedPropertyInitialization()
        {
            Assert.IsFalse(firstCell.IsFlagged);
        }

        [TestMethod]
        public void TestOpenCellMethod()
        {
            firstCell.OpenCell();
            Assert.IsTrue(firstCell.IsOpened);
            Assert.IsFalse(firstCell.IsFlagged);
        }

        [TestMethod]
        public void TestToggleFlagMethod()
        {
            Assert.IsFalse(secondCell.IsFlagged);
            secondCell.ToggleFlag();
            Assert.IsTrue(secondCell.IsFlagged);
            secondCell.ToggleFlag();
            Assert.IsFalse(secondCell.IsFlagged);
        }

        [TestMethod]
        public void TestAddMineMethod()
        {
            secondCell.AddMine();
            Assert.IsTrue(secondCell.IsMined);
        }
    }
}
