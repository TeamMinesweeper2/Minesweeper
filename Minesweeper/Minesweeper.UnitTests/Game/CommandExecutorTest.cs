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
            var exector = new CommandExecutor();
            exector.ExecuteCommand(null);
        }
    }
}
