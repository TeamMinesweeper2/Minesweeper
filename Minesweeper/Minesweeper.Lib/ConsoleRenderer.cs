namespace Minesweeper.Lib
{
    using System;

    public class ConsoleRenderer : IRenderer
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

        public void WriteAt(int left, int top, string format, params object[] args)
        {
            Console.SetCursorPosition(left, top);
            Console.Write(format, args);
        }

        public void ClearLines(int left, int top, int numLines)
        {
            if (numLines < 1)
            {
                throw new ArgumentOutOfRangeException("numLines", "Number of lines to clear must be at least '1'.");
            }

            // TODO: validate left, top
            string spaces = new string(' ', numLines * Console.WindowWidth - left);
            this.WriteAt(left, top, spaces);

            Console.SetCursorPosition(left, top);
        }
    }
}
