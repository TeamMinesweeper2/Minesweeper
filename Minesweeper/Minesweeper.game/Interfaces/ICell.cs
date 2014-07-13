namespace Minesweeper.Interfaces
{
    using System;
    using System.Linq;

    public interface ICell
    {
        /// <summary>
        /// Opens the current cell
        /// </summary>
        void OpenCell();

        /// <summary>
        /// Toggles the flag of the current cell
        /// </summary>
        void ToggleFlag();

        /// <summary>
        /// Adds mine to the current cell
        /// </summary>
        void AddMine();

        bool IsOpened
        {
            get;
        }

        bool IsMined
        {
            get;
        }

        bool IsFlagged
        {
            get;
        }
    }
}
