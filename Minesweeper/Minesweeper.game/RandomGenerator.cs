using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    public class RandomGenerator
    {
        private static RandomGenerator instance;

        private RandomGenerator()
        {
           
        }
        public static RandomGenerator Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
