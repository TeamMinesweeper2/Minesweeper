namespace Minesweeper.Game
{
    /// <summary>
    /// The 'Command' interface.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Invokes the action on the needed object.
        /// </summary>
        /// <returns>Returns true if more commands can be executed.</returns>
        bool Execute();
    }
}
