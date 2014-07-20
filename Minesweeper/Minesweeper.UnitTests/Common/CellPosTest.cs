using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minesweeper.Lib;

namespace Minesweeper.UnitTests.Common
{
    [TestClass]
    public class CellPosTest
    {
        private CellPos cell;

        [TestInitialize]
        public void TestInitialize()
        {
            int row = 1;
            int col = 1;
            cell = new CellPos(row, col);
        }

        [TestMethod]
        public void TestFieldInitialization()
        {
            int row = 1;
            int col = 1;
            Assert.AreEqual(cell.Row, row);
            Assert.AreEqual(cell.Col, col);
        }

        
        [TestMethod]
        [TestProperty("Row", "-1")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestPropRowSetNegativeValue()
        {
            cell.Row = -1;
        }

        [TestMethod]
        [TestProperty("Col", "-1")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestPropColSetNegativeValue()
        {
            cell.Col = -1;
        }
    }
}
