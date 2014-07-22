using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minesweeper.Game;

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
            MinesweeperGame game = new MinesweeperGameEasy();
            CommandParser commandParser = new CommandParser(game);
            ICommand restartCommand = commandParser.ParseCommand("restart");
            ICommand topCommand = commandParser.ParseCommand("top");
            var executedRestart = executor.ExecuteCommand(restartCommand);
            var executedTop = executor.ExecuteCommand(topCommand);
            Assert.ReferenceEquals(executedRestart, executedTop);
        }
    }
}
