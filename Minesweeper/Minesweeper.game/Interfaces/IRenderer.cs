namespace Minesweeper
{
    public interface IRenderer
    {
        void WriteLine();

        void WriteLine(string format, params object[] args);

        void Write(string format, params object[] args);        

        void WriteAt(int row, int col, string format, params object[] args);
    }
}
