namespace Minesweeper
{
    /// <summary>
    /// Defines the error types when the command cannot be executed.
    /// </summary>
    public enum ErrorType
    {
        /// <summary>Cell out of range.</summary>
        CellOutOfRange,

        /// <summary>Cell already opened.</summary>
        AlreadyOpened,

        /// <summary>Invalid command.</summary>
        IvalidCommand
    }
}
