using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minesweeper.Lib;

namespace Minesweeper.UnitTests.Common
{
    [TestClass]
    public class CellUnitTest
    {
        [TestMethod]
        public void TestConstructorCell()
        {
            Cell pesho = new Cell();
            Cell gosho = new Cell();
            Assert.ReferenceEquals(pesho, gosho);
        }

        [TestMethod]
        public void TestIsOpenPropertyInitialization()
        {
            Cell pesho = new Cell();
            Assert.IsFalse(pesho.IsOpened);
        }

        [TestMethod]
        public void TestIsMinedPropertyInitialization()
        {
            Cell pesho = new Cell();
            Assert.IsFalse(pesho.IsMined);
        }

        [TestMethod]
        public void TestIsFalggedPropertyInitialization()
        {
            Cell pesho = new Cell();
            Assert.IsFalse(pesho.IsFlagged);
        }

        [TestMethod]
        public void TestOpenCellMethod()
        {
            Cell pesho = new Cell();
            pesho.OpenCell();
            Assert.IsTrue(pesho.IsOpened);
            Assert.IsFalse(pesho.IsFlagged);
        }

        [TestMethod]
        public void TestToggleFlagMethod()
        {
            Cell pesho = new Cell();
            Assert.IsFalse(pesho.IsFlagged);
            pesho.ToggleFlag();
            Assert.IsTrue(pesho.IsFlagged);
            pesho.ToggleFlag();
            Assert.IsFalse(pesho.IsFlagged);
        }

        [TestMethod]
        public void TestAddMineMethod()
        {
            Cell pesho = new Cell();
            pesho.AddMine();
            Assert.IsTrue(pesho.IsMined);
        }
    }
}
