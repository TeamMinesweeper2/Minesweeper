namespace Minesweeper
{
    public interface IMinefield
    {
        void OpenCell(int row, int column);

        bool IsCellOpened(int row, int column);

        bool IsThereMineInCell(int row, int column);
    }
}