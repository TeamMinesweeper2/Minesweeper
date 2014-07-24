//-----------------------------------------------------------------------
// <copyright file="ConsoleReader.cs" company="Telerik Academy">
//     Copyright (c) 2014 Telerik Academy. All rights reserved.
// </copyright>
// <summary>Class library for the Minesweeper game.</summary>
//-----------------------------------------------------------------------
namespace Minesweeper.Lib
{
    using System;
    using Minesweeper.Lib.Interfaces;

    /// <summary>
    /// Implements the <see cref="IUserInputReader"/> interface with the <see cref="System.Console"/>.
    /// </summary>
    public class ConsoleReader : IUserInputReader
    {
        /// <summary>
        /// Waits for the user to press a key.
        /// </summary>
        public void WaitForKey()
        {
            Console.ReadKey(true);
        }

        /// <summary>
        /// Reads the next line of characters from the standard input stream.
        /// </summary>
        /// <returns>The next line of characters from the input stream, or null if no more lines are available.</returns>
        public string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}
