using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minesweeper.Lib;
using System.IO;
using Minesweeper.Lib.Interfaces;

namespace Minesweeper.UnitTests.Common
{
    [TestClass]
    public class ConsoleRendererTest
    {
        private IRenderer renderer;

        [TestInitialize]
        public void TestInitialize()
        {
            renderer = new ConsoleRenderer();
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestWriteLineWithLessArguments()
        {
            string format = "Message {0} {1}";
            renderer.WriteLine(format, "Player1");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestWriteWithLessArguments()
        {
            string format = "Message {10}";
            renderer.Write(format, null);
        }

        [TestMethod]
        public void TestWriteAt()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                renderer.WriteAt(1, 1, "Message {0}", "play");

                string expected = string.Format("Message {0}", "play");
                Assert.AreEqual<string>(expected, sw.ToString());
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestWriteAtWithNegativeTopArg()
        {
            renderer.WriteAt(-1, 0, "Message");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestClearLinesWithNegativeNumLines()
        {
            renderer.ClearLines(1, 1, 0);
        }
    }
}
