namespace Minesweeper
{
    /// <summary>
    /// Defines the possible results after an open cell command.
    /// </summary>
    public enum MinefieldState
    {
        /// <summary>Cell out of range.</summary>
        OutOfRange,

        /// <summary>Cell already opened.</summary>
        AlreadyOpened,

        /// <summary>Cell has a mine.</summary>
        Boom,

        /// <summary>Cell has no mine and can be safely opened.</summary>
        Normal
    }
}
