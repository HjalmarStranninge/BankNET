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
        internal static void IncorrectNameOrPin(string username,int attemptsLeft, string pinFailMessage, string lockedOutMessage)
        {
            MenuUI.ClearAndPrintFooter();
            if (LogInLogOut.userLogInAttempts[username] == 1)
            {
                Console.WriteLine(pinFailMessage);
                Console.WriteLine("\n\t       You have 2 attempts left.");
                Thread.Sleep(2000);
            }
            else if (LogInLogOut.userLogInAttempts[username] == 2)
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
                // After third failed attempt LockOutUser will be called to lock out user.
                int lockOutMinutes = 3;
                LockOutUser(username, lockOutMinutes, lockedOutMessage);

                // Resets the attempts
                LogInLogOut.userLogInAttempts[username] = 0;
            }
        }

        // Locks user out from the moment this method is called and add (int minutes) to the time ate the moment. 
        internal static void LockOutUser(string username, int lockOutMinutes, string lockedOutMessage)
        {
            // Adds 3 minutes from now where the user is locked out
            DateTime lockoutTime = DateTime.Now.AddMinutes(lockOutMinutes);

            // Assigns the lockouttime to the specific username
            LogInLogOut.userLockOutTime[username] = lockoutTime;

            Console.WriteLine($"\n\t{lockedOutMessage}");
            Console.WriteLine($"\n\t User {username} is temporarily locked out.");
            Thread.Sleep(2000);
            return;
        }
        // Method used for checking if user is still locked out
        internal static bool IsLockedOut(string username)
        {
            return LogInLogOut.userLockOutTime.ContainsKey(username) && DateTime.Now < LogInLogOut.userLockOutTime[username];
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
            Console.WriteLine("\n\n\t    Name cannot be empty, try again.");
            Thread.Sleep(2000);
        }
    }
}
