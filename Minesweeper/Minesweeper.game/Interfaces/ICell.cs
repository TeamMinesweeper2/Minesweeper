namespace Minesweeper.Interfaces
{
    using System;
    using System.Linq;

    public interface ICell
    {
        void OpenCell();

        void ToggleFlag();

        void AddMine();
    }
}
