using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minesweeper.Game;

namespace Minesweeper.UnitTests.Game
{
    [TestClass]
    public class CommandParserTest
    {
        private MinesweeperGame game;
        private CommandParser parser;

        [TestInitialize]
        public void TestInitialize()
        {
            game = new MinesweeperGameEasy();
            parser = new CommandParser(game);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_CommandParser_CtorWithNullThrowsEx()
        {
            var parser = new CommandParser(null);
        }

        [TestMethod]
        public void Test_ParseCommand_InvalidCommands()
        {
            Assert.IsTrue(parser.ParseCommand(string.Empty) is CmdInvalid);
            Assert.IsTrue(parser.ParseCommand("0") is CmdInvalid);
            Assert.IsTrue(parser.ParseCommand("0 3.14") is CmdInvalid);
            Assert.IsTrue(parser.ParseCommand("3.14 0") is CmdInvalid);
            Assert.IsTrue(parser.ParseCommand("m 0") is CmdInvalid);
            Assert.IsTrue(parser.ParseCommand("m 0 3.14") is CmdInvalid);
            Assert.IsTrue(parser.ParseCommand("m 3.14 0") is CmdInvalid);
        }

        [TestMethod]
        public void Test_ParseCommand_SingleWordCommands()
        {
            Assert.IsTrue(parser.ParseCommand("restart") is CmdRestart);
            Assert.IsTrue(parser.ParseCommand("top") is CmdShowScores);
            Assert.IsTrue(parser.ParseCommand("exit") is CmdExit);
            Assert.IsTrue(parser.ParseCommand("boom") is CmdBoom);
        }

        [TestMethod]
        public void Test_ParseCommand_OpenAndFlagCell()
        {
            // Open and flag cell commands
            Assert.IsTrue(parser.ParseCommand("0 0") is CmdOpenCell);
            Assert.IsTrue(parser.ParseCommand("m 0 0") is CmdFlagCell);
        }
    }
}
