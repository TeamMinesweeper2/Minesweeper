using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minesweeper.Lib;

namespace Minesweeper.UnitTests.Game.Commands
{
    [TestClass]
    public class CommandsTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_CmdBoom_CtorWithNullThrowsEx()
        {
            var cmd = new CmdBoom(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_CmdShow_CtorWithNullThrowsEx()
        {
            var cmd = new CmdShowScores(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_CmdRestart_CtorWithNullThrowsEx()
        {
            var cmd = new CmdRestart(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_CmdInvalid_CtorWithNullThrowsEx()
        {
            var cmd = new CmdInvalid(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_CmdExit_CtorWithNullThrowsEx()
        {
            var cmd = new CmdExit(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_CmdFlagCell_CtorWithNullThrowsEx()
        {
            var cmd = new CmdFlagCell(null, CellPos.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_CmdOpenCell_CtorWithNullThrowsEx()
        {
            var cmd = new CmdOpenCell(null, CellPos.Empty);
        }
    }
}
