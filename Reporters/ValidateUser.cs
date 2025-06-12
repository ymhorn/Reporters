using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporters
{
    //Checks if the user has permission to access the information
    internal static class ValidateUser
    {
        //Ask the user for the correct password and checks it
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
