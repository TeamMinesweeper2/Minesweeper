namespace Minesweeper.Lib
{
    /// <summary>
    /// Random number provider interface.
    /// </summary>
    public interface IRandomGeneratorProvider
    {
        /// <summary>
        /// Generates random number in range.
        /// </summary>
        /// <param name="min">Minimal range.</param>
        /// <param name="max">Maximal range.</param>
        /// <returns>Random integer number.</returns>
        int GetRandomNumber(int min, int max);

        /// <summary>
        /// Overload with min range 0.
        /// </summary>
        /// <param name="max">Maximal range.</param>
        /// <returns>Random integer number.</returns>
        int GetRandomNumber(int max);
    }
}
