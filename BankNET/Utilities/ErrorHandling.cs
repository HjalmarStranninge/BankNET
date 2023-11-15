using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankNET.Utilities
{
    // Class containing methods for handling errors (Invalid inputs etc).
    internal class ErrorHandling
    {
        internal static void IncorrectLogin()
        {
            Console.Clear();
            Console.WriteLine("Too many incorrect tries, program shutting down.");
            Thread.Sleep(1500);
            Environment.Exit(0);
        }
        // Add method that checks for invalid input when doing console.readline.

        // Add method that checks for multiple wrong password attempts. (Extra)
    }
}
