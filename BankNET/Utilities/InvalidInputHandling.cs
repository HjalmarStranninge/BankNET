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
                Console.WriteLine("Invalid username and/or pin. You have 2 attempts left.");
                Thread.Sleep(1500);
            }
            else if ((!validPin || !validUsername) && i == 1)
            {
                MenuUI.ClearAndPrintFooter();
                Console.WriteLine("Invalid username and/or pin. You have 1 attempt left.");
                Thread.Sleep(1500);
            }
            else
            {
                MenuUI.ClearAndPrintFooter();
                Console.WriteLine("Too many incorrect tries, program shutting down.");
                Thread.Sleep(1500);
                Environment.Exit(0);
            }
        }

        internal static void InvalidInput()
        {
            Console.Clear();
            Console.WriteLine("Invalid input, try again.");
            Thread.Sleep(1200);
        }
        // Add method that checks for invalid input when doing console.readline.

        // Add method that checks for multiple wrong password attempts. (Extra)
    }
}
