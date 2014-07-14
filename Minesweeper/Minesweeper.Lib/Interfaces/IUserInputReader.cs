namespace Minesweeper.Lib
{
    public interface IUserInputReader
    {
        string ReadLine();

        void WaitForKey();
    }
}
