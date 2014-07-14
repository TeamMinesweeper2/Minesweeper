namespace Minesweeper
{
    public interface IUserInputReader
    {
        string ReadLine();

        void WaitForKey();
    }
}
