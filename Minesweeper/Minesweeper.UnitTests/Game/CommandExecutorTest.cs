using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minesweeper.Game;
using Minesweeper.Lib;

namespace Minesweeper.UnitTests.Game
{
    [TestClass]
    public class CommandExecutorTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_ExecuteCommandWithNull_ThrowsEx()
        {
            var executor = new CommandExecutor();
            executor.ExecuteCommand(null);
        }

        [TestMethod]
        public void Test_ExecuteCommandWithValidParam()
        {
            var executor = new CommandExecutor();
            IUIManager consoleUIManager = new UIManager(new ConsoleRenderer(), new ConsoleReader());
            MinesweeperGame game = new MinesweeperGameEasy(consoleUIManager);
            CommandParser commandParser = new CommandParser(game);
            ICommand restartCommand = commandParser.ParseCommand("restart");
            ICommand topCommand = commandParser.ParseCommand("top");
            var executedRestart = executor.ExecuteCommand(restartCommand);
            var executedTop = executor.ExecuteCommand(topCommand);
            Assert.ReferenceEquals(executedRestart, executedTop);
        }
    }
}
