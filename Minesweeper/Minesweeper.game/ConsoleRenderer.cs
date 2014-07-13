using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minesweeper
{
    class ConsoleRenderer : IRenderer
    {
        public void WriteLine()
        {
            Console.WriteLine();
        }

        public void WriteLine(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }

        public void Write(string format, params object[] args)
        {
            Console.Write(format, args);
        }

        public void WriteAt(int row, int col, string format, params object[] args)
        {
            // TODO: validate row, col
            Console.SetCursorPosition(row, col);
            Console.Write(format, args);
        }
    }
}
