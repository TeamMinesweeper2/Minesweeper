//-----------------------------------------------------------------------
// <copyright file="IRenderer.cs" company="Telerik Academy">
//     Copyright (c) 2014 Telerik Academy. All rights reserved.
// </copyright>
// <summary>Class library for the Minesweeper game.</summary>
//-----------------------------------------------------------------------
namespace Minesweeper.Lib.Interfaces
{
    /// <summary>
    /// Defines methods for rendering text.
    /// </summary>
    public interface IRenderer
    {
        /// <summary>
        /// Writes the current line terminator to the output stream.
        /// </summary>
        void WriteLine();

        /// <summary>
        /// Writes the text representation of the specified array of objects 
        /// to the output stream using the specified format information, 
        /// followed by the current line terminator.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using format.</param>
        void WriteLine(string format, params object[] args);

        /// <summary>
        /// Writes the text representation of the specified array of objects 
        /// to the output stream using the specified format information.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using format.</param>
        void Write(string format, params object[] args);

        /// <summary>
        /// Writes the text representation of the specified array of objects 
        /// to the output stream using the specified format information at a specific position.
        /// </summary>
        /// <param name="left">The column position.</param>
        /// <param name="top">The row position.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using format.</param>
        void WriteAt(int left, int top, string format, params object[] args);

        /// <summary>
        /// Clear specific number of lines.
        /// </summary>
        /// <param name="left">Start column position of the clearing.</param>
        /// <param name="top">Start row position of the clearing.</param>
        /// <param name="numLines">Number of lines that must be cleared.</param>
        void ClearLines(int left, int top, int numLines);
    }
}
