using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        //static bool isLockedOut;
        static DateTime lockoutTime;

        internal static void IncorrectNameOrPin(int i, string pinFailMessage, string lockedOutMessage
            )
        {
            MenuUI.ClearAndPrintFooter();
            if (IsLockedOut())
            {
                Console.WriteLine("\n\t       Temporarily locked out.");
                Console.WriteLine("\n\t Please try again in a couple of minutes.");
                Thread.Sleep(2000);
                return;
            }
            else if (i == 2)
            {
                Console.WriteLine(pinFailMessage);
                Console.WriteLine("\n\t     You have 2 attempts left.");
                Thread.Sleep(2000);
            }
            else if (i == 1)
            {
                Console.WriteLine(pinFailMessage);
                Console.WriteLine("\n\t     You have 1 attempt left.");
                Thread.Sleep(2000);
            }
            else
            {
                LockOutUser(3, lockedOutMessage);
            }
        }
        internal static void InvalidWithdrawal(string message)
        {
            Console.WriteLine($"\n{message}");
            Console.Write("Returning to main menu...");
            Thread.Sleep(2000);
        }
        internal static void LockOutUser(int minutes, string lockedOutMessage)
        {
            lockoutTime = DateTime.Now.AddMinutes(minutes);
            Console.WriteLine($"\n\t{lockedOutMessage}");
            Console.WriteLine("\n\t      Temporarily locked out.");
            Thread.Sleep(2000);
        }
        internal static bool IsLockedOut()
        {
            return DateTime.Now < lockoutTime;
        }
    }
}
