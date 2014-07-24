using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minesweeper.Game;
using Moq;
using Minesweeper.Game.Interfaces;

namespace Minesweeper.UnitTests.Game
{
    [TestClass]
    public class CommandParserTest
    {
        private MinesweeperGame game;
        private CommandFactory parser;

        [TestInitialize]
        public void TestInitialize()
        {
            Mock<IUIManager> uiManagerMock = new Mock<IUIManager>();
            game = new MinesweeperGameEasy(uiManagerMock.Object);
            parser = new CommandFactory(game);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_CommandParser_CtorWithNullThrowsEx()
        {
            var parser = new CommandFactory(null);
        }
    }
}
