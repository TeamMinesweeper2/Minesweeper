//-----------------------------------------------------------------------
// <copyright file="ConsoleRenderer.cs" company="Telerik Academy">
//     Copyright (c) 2014 Telerik Academy. All rights reserved.
// </copyright>
// <summary>Class library for the Minesweeper game.</summary>
//-----------------------------------------------------------------------
namespace Minesweeper.Lib
{
    using System;

    /// <summary>
    /// Writes text messages to the standard output stream.
    /// </summary>
    public class ConsoleRenderer : IRenderer
    {
        /// <summary>
        /// Writes the current line terminator to the standard output stream.
        /// </summary>
        public void WriteLine()
        {
            Console.WriteLine();
        }

        /// <summary>
        /// Writes the text representation of the specified array of objects 
        /// to the standard output stream using the specified format information, 
        /// followed by the current line terminator.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using format.</param>
        public void WriteLine(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }

        /// <summary>
        /// Writes the text representation of the specified array of objects 
        /// to the standard output stream using the specified format information.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using format.</param>
        public void Write(string format, params object[] args)
        {
            Console.Write(format, args);
        }

        /// <summary>
        /// Writes the text representation of the specified array of objects 
        /// to the standard output stream using the specified format information
        /// at a specific position.
        /// </summary>
        /// <param name="left">The column position.</param>
        /// <param name="top">The row position.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using format.</param>
        public void WriteAt(int left, int top, string format, params object[] args)
        {
            if (left < 0 || top < 0)
            {
                throw new ArgumentOutOfRangeException("left/top", "Top/left must be greater than or equal to 0.");
            }

            Console.SetCursorPosition(left, top);
            Console.Write(format, args);
        }

        /// <summary>
        /// Clear specific number of lines.
        /// </summary>
        /// <param name="left">Start column position of the clearing.</param>
        /// <param name="top">Start row position of the clearing.</param>
        /// <param name="numLines">Number of lines that must be cleared.</param>
        public void ClearLines(int left, int top, int numLines)
        {
            if (numLines < 1)
            {
                throw new ArgumentOutOfRangeException("numLines", "Number of lines to clear must be at least '1'.");
            }

            if (left < 0 || top < 0)
            {
                throw new ArgumentOutOfRangeException("left/top", "Top/left must be greater than or equal to 0.");
            }

            string spaces = new string(' ', (numLines * Console.WindowWidth) - left);
            this.WriteAt(left, top, spaces);

            Console.SetCursorPosition(left, top);
        }
    }
}
