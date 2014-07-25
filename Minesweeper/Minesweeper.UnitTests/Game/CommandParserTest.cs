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
        private CommandExecutor parser;

        [TestInitialize]
        public void TestInitialize()
        {
            Mock<IUIManager> uiManagerMock = new Mock<IUIManager>();
            game = new MinesweeperGameEasy(uiManagerMock.Object);
            parser = new CommandExecutor(game);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_CommandFactory_CtorWithNullThrowsEx()
        {
            var parser = new CommandExecutor(null);
        }
    }
}
