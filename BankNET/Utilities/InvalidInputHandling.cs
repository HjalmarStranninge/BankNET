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
        internal static void IncorrectNameOrPin(int attemptsLeft, string pinFailMessage)
        {
            MenuUI.ClearAndPrintFooter();

            if (attemptsLeft == 2)
            {
                Console.WriteLine(pinFailMessage);
                Console.WriteLine("\n\t       You have 2 attempts left.");
                Thread.Sleep(2000);
            }
            else if (attemptsLeft == 1)
            {
                Console.WriteLine(pinFailMessage);
                Console.WriteLine("\n\t       You have 1 attempt left.");
                Thread.Sleep(2000);
            }
            // After third failed attempt will call LockOutUser to lock out user.
            else if (attemptsLeft == 0) 
            {
                LockOutUser(3);
            }
            else
            {
                IsLockedOut();
                return;
            }
        }

        // Locks user out from the moment this method is called and add (int minutes) to the time at the moment. 
        private static void LockOutUser(int minutes)
        {
            lockoutTime = DateTime.Now.AddMinutes(minutes);
            Console.WriteLine("\n\n\t    Too many incorrect attempts.");
            Console.WriteLine("\n\t\tYou are locked out.");
            Thread.Sleep(2000);
            return;
        }
        // Method used for checking if user is still locked out
        internal static bool IsLockedOut()
        {
            return DateTime.Now < lockoutTime;
        }

        // Method to handle invalid user input where amount of money is requested.
        internal static void InvalidInputAmount()
        {
            Console.WriteLine("\n     Please enter a valid amount, numbers only.");
            Thread.Sleep(2000);
        }

        // Method to handle invalid user input where account name/username is requested.
        internal static void InvalidInputName()
        {
            Console.WriteLine("\n\n\t   Name cannot be empty, try again.");
            Thread.Sleep(2000);
        }
    }
}
