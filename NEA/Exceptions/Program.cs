using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    internal class Program
    {
        static void Main(string[] args)
        {
        }
        public class improperPlaceValueException : Exception
        {
            public improperPlaceValueException(int placeValue)
            {
                
            }
            public improperPlaceValueException() : base("Place values must be some multiple of 10.")
            {

            }
        }
    }
}
