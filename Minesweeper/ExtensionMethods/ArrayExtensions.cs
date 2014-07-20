//-----------------------------------------------------------------------
// <copyright file="ArrayExtensions.cs" company="Telerik Academy">
//     Copyright (c) 2014 Telerik Academy. All rights reserved.
// </copyright>
// <summary>Extension methods for the Minesweeper game.</summary>
//-----------------------------------------------------------------------
namespace ExtensionMethods
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Minesweeper.Lib;

    /// <summary>
    /// Extensions of IEnumerable needed in the game.
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Handles shuffling of an IEnumerable of T with Fisher-Yates shuffle algorithm.
        /// </summary>
        /// <typeparam name="T">Generic type.</typeparam>
        /// <param name="source">IEnumerable of T to be shuffled randomly.</param>
        /// <returns>Shuffled IEnumerable.</returns>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            var randomGenerator = RandomGeneratorProvider.GetInstance();
            return source.Shuffle(randomGenerator);
        }

        /// <summary>
        /// Handles shuffling of an IEnumerable of T with Fisher-Yates shuffle algorithm with given random generator.
        /// </summary>
        /// <typeparam name="T">Generic type.</typeparam>
        /// <param name="source">IEnumerable of T to be shuffled randomly.</param>
        /// <param name="randomGenerator">IRandomGeneratorProvider for testable random numbers.</param>
        /// <returns>Shuffled IEnumerable.</returns>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, IRandomGeneratorProvider randomGenerator)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (randomGenerator == null)
            {
                throw new ArgumentNullException("rng");
            }

            return source.ShuffleIterator(randomGenerator);
        }

        /// <summary>
        /// Returns shuffled IEnumerable of T with Fisher-Yates shuffle algorithm with given random generator.
        /// </summary>
        /// <typeparam name="T">Generic type.</typeparam>
        /// <param name="source">IEnumerable to be shuffled.</param>
        /// <param name="randomGenerator">Provider of random generator of type IRandomGeneratorProvider.</param>
        /// <returns>Shuffled IEnumerable.</returns>
        private static IEnumerable<T> ShuffleIterator<T>(this IEnumerable<T> source, IRandomGeneratorProvider randomGenerator)
        {
            T[] elements = source.ToArray();

            for (int i = elements.Length - 1; i > 0; i--)
            {
                int swapIndex = randomGenerator.Next(i + 1);
                yield return elements[swapIndex];

                elements[swapIndex] = elements[i];
            }

            yield return elements[0];
        }
    }
}
