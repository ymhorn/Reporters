using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporters
{
    internal static class ValidateUser
    {
        public static bool Password()
        {
            Console.WriteLine("Please insert the password:");
            string password = Console.ReadLine();
            if (password == "Esther@29")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
