//-----------------------------------------------------------------------
// <copyright file="IUserInputReader.cs" company="Telerik Academy">
//     Copyright (c) 2014 Telerik Academy. All rights reserved.
// </copyright>
// <summary>Class library for the Minesweeper game.</summary>
//-----------------------------------------------------------------------
namespace Minesweeper.Lib.Interfaces
{
    /// <summary>
    /// Defines methods for reading user input.
    /// </summary>
    public interface IUserInputReader
    {
        /// <summary>
        /// Reads the next line of characters from the input stream.
        /// </summary>
        /// <returns>The next line of characters from the input stream, or null if no more lines are available.</returns>
        string ReadLine();

        /// <summary>
        /// Waits for the user to press a key.
        /// </summary>
        void WaitForKey();
    }
}
