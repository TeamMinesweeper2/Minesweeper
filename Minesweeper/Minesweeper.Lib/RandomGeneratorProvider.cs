namespace Minesweeper.Lib
{
    using System;

    /// <summary>
    /// Random generator singleton.
    /// </summary>
    public class RandomGeneratorProvider : IRandomGeneratorProvider
    {
        /// <summary>Singleton instance of RandomGeneratorProvider.</summary>
        private static RandomGeneratorProvider instance;

        /// <summary>Readonly instance of Random class.</summary>
        private readonly Random randomGenerator;

        /// <summary>
        /// Creates a new instance of <see cref="RandomGeneratorProvider"/>
        /// </summary>
        private RandomGeneratorProvider()
        {
            this.randomGenerator = new Random();
        }

        /// <summary>
        /// Returns the only instance of RandomGeneratorProvider.
        /// </summary>
        /// <returns>RandomGeneratorProvider only instance.</returns>
        public static RandomGeneratorProvider GetInstance()
        {
            // No need for multi threading fix.
            if (instance == null)
            {
                instance = new RandomGeneratorProvider();
            }

            return instance;
        }

        /// <summary>
        /// Generates random number through Random class.
        /// </summary>
        /// <param name="minNumber">Minimal range.</param>
        /// <param name="maxNumber">Maximal range.</param>
        /// <returns>Integer random number.</returns>
        public int GetRandomNumber(int minNumber, int maxNumber)
        {
            int nextRandomNumber = this.randomGenerator.Next(minNumber, maxNumber + 1);
            return nextRandomNumber;
        }

        /// <summary>
        /// Overload with minimal range 0.
        /// </summary>
        /// <param name="maxNumber">Maximal range.</param>
        /// <returns>Integer random number.</returns>
        public int GetRandomNumber(int maxNumber)
        {
            return GetRandomNumber(0, maxNumber);
        }
    }
}
