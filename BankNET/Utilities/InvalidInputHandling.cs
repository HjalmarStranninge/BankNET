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
        static DateTime lockoutTime;
        // Method for displaying failed input and then locking user out. Counter is outside used together with calling the method.
        internal static void IncorrectNameOrPin(int attemptsLeft, string pinFailMessage, string lockedOutMessage
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
            else if (attemptsLeft == 2)
            {
                Console.WriteLine(pinFailMessage);
                Console.WriteLine("\n\t     You have 2 attempts left.");
                Thread.Sleep(2000);
            }
            else if (attemptsLeft == 1)
            {
                Console.WriteLine(pinFailMessage);
                Console.WriteLine("\n\t     You have 1 attempt left.");
                Thread.Sleep(2000);
            }
            else
            {
                // After third failed attempt will call LockOutUser to lock out user.
                LockOutUser(3, lockedOutMessage);
            }
        }
        // Used with specified prompt during invalid user withdrawal.
        internal static void InvalidWithdrawal(string message)
        {
            Console.WriteLine($"\n{message}");
            Console.Write("Returning to main menu...");
            Thread.Sleep(2000);
        }

        // Locks user out from the moment this method is called and add (int minutes) to the time ate the moment. 
        internal static void LockOutUser(int minutes, string lockedOutMessage)
        {
            lockoutTime = DateTime.Now.AddMinutes(minutes);
            Console.WriteLine($"\n\t{lockedOutMessage}");
            Console.WriteLine("\n\t      Temporarily locked out.");
            Thread.Sleep(2000);
        }
        // Method used for checking if user is still locked out
        internal static bool IsLockedOut()
        {
            return DateTime.Now < lockoutTime;
        }
    }
}
