using BankNET.Data;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankNET.Utilities
{
    internal class LogInLogOut
    {
        // Dictionary of usernames that has entered the wrong pin with their number of tries
        internal static Dictionary<string, int> userLogInAttempts = new Dictionary<string, int>();

        internal static Dictionary<string, DateTime> userLockOutTime = new Dictionary<string, DateTime>();
      
        // Method for login to check if valid login credentials, and redirects to menu if login accepted
        internal static string LogIn(BankContext context)
        {
            bool loginSuccesful = false;
            int numberOfTries = 1;
            while (!loginSuccesful)
            {
                MenuUI.ClearAndPrintFooter();
                Console.CursorVisible = true;

                Console.Write("\n\tEnter username: ");

                string username = Console.ReadLine();
                Console.CursorVisible = false;
                Console.Beep();
                userLogInAttempts.Add(username, numberOfTries);

                if (username.Length != 0)
                {
                    Console.Write("\n\tEnter pin: ");

                    ConsoleKeyInfo keyInfo;

                    // Checks if username and pin matches any users in the database.
                    Console.CursorVisible = true;
                    bool validUsernameAndPin = BankHelpers.PinCheck(context, username);

                    Console.CursorVisible = false;
                    Console.Beep();

                    // Returns username if everything checks out.
                    if (validUsernameAndPin && !InvalidInputHandling.IsLockedOut(username))
                    {
                        Console.WriteLine("Login successful!");
                        return username;
                    }
                    else
                    {
                        InvalidInputHandling.IncorrectNameOrPin(username, "\n\t      Invalid username and/or pin.");
                        numberOfTries++;
                    }
                }
                // If user just presses enter without writing anything after "Enter username: " user returns to start page
                else
                {
                    MenuUI.ClearAndPrintFooter();
                    InvalidInputHandling.InvalidInputName();
                }
            }
            return null;
        }

        // Method to log out user, returns to startpage
        internal static bool LogOut()
        {
            MenuUI.ClearAndPrintFooter();
            Console.WriteLine("\n\n\t   Logging you out. Please wait..");
            Thread.Sleep(2000);
            return false;
        }
    }
}
