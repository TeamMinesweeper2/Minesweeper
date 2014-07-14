namespace Minesweeper
{
    using System;

    public class ConsoleReader : IUserInputReader
    {
        public ConsoleReader()
        {
        }

        public void WaitForKey()
        {
            Console.ReadKey();
        }

        public string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}
