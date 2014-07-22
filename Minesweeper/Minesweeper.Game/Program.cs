//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Telerik Academy">
//     Copyright (c) 2014 Telerik Academy. All rights reserved.
// </copyright>
// <summary> Class for the main entry point of the program.</summary>
//-----------------------------------------------------------------------
namespace Minesweeper.Game
{
    /// <summary>
    /// Class for the main entry point of the program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The entry point of the console application.
        /// </summary>
        public static void Main()
        {
            var gameEngine = new Engine();
            gameEngine.Run();
        }       
    }
}
