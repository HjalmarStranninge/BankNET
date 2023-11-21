using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankNET.Utilities
{
    // Class containing methods for handling errors (Invalid inputs etc).
    internal class InvalidInputHandling
    {
        internal static void IncorrectLogin(bool validUsername, bool validPin, int i)
        {
            if ((!validPin || !validUsername) && i == 0)
            {
                MenuUI.ClearAndPrintFooter();
                Console.WriteLine("\n\t    Invalid username and/or pin");
                Console.WriteLine("\n\t     You have 2 attempts left.");
                Thread.Sleep(2000);
            }
            else if ((!validPin || !validUsername) && i == 1)
            {
                MenuUI.ClearAndPrintFooter();
                Console.WriteLine("\n\t    Invalid username and/or pin");
                Console.WriteLine("\n\t     You have 1 attempt left.");
                Thread.Sleep(2000);
            }
            else
            {
                MenuUI.ClearAndPrintFooter();
                Console.WriteLine("Too many incorrect tries, program shutting down.");
                Thread.Sleep(2000);
                Environment.Exit(0);
            }
        }

        internal static void InvalidInput()
        {
            Console.Clear();
            Console.WriteLine("Invalid input, try again.");
            Thread.Sleep(1200);
        }      
    }
}
