using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankNET.Utilities
{
    // Class containing methods for handling invalid inputs
    internal class InvalidInputHandling
    {
        internal static void IncorrectLogin(int i)
        {
            MenuUI.ClearAndPrintFooter();
            if (i == 0)
            {
                Console.WriteLine("\n\t    Invalid username and/or pin.");
                Console.WriteLine("\n\t     You have 2 attempts left.");
                Thread.Sleep(2000);
            }
            else if (i == 1)
            {
                Console.WriteLine("\n\t    Invalid username and/or pin.");
                Console.WriteLine("\n\t     You have 1 attempt left.");
                Thread.Sleep(2000);
            }
            else
            {
                Console.WriteLine("   Too many incorrect tries, program shutting down.");
                Thread.Sleep(3000);
                Environment.Exit(0);
            }
        }
        internal static void InvalidWithdrawal(string message)
        {
            Console.WriteLine($"\n{message}");
            Console.Write("Returning to main menu...");
            Thread.Sleep(2000);
        }
    }
}
