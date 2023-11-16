using BankNET.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankNET.Utilities
{
    internal class LogInLogOut
    {
        internal static void Login(BankContext context)
        {
            bool tryAgainLogin = true;
            int i = 0;
            Console.Clear();

            while (tryAgainLogin && i < 3)
            {
                Console.WriteLine("Log in");
                Console.Write("Enter username: ");
                string username = Console.ReadLine();

                Console.Write("Enter pin: ");

                ConsoleKeyInfo keyInfo;
                string pin = "";
                do
                {
                    keyInfo = Console.ReadKey(true); // Read a key without displaying it
                    if (keyInfo.Key != ConsoleKey.Backspace && keyInfo.Key != ConsoleKey.Enter)
                    {
                        pin += keyInfo.KeyChar;
                        Console.Write("*"); // Display a star for each character
                    }
                    else if (keyInfo.Key == ConsoleKey.Backspace && pin.Length > 0)
                    {
                        pin = pin.Substring(0, pin.Length - 1);
                        Console.Write("\b \b"); // Clear the last character and move the cursor back
                    }
                } while (keyInfo.Key != ConsoleKey.Enter);

                bool validUsername = context.Users.Any(uN => uN.UserName.Equals(username));
                bool validPin = context.Users.Any(p => p.UserName.Equals(username) && p.Pin.Equals(pin));

                if (validUsername && validPin)
                {
                    Console.WriteLine("Login successful!");
                    tryAgainLogin = false;

                    if (username == "admin") MainMenus.AdminMenu(context);
                    else MainMenus.UserMainMenu(context, username);
                }
                else
                {
                    InvalidInputHandling.IncorrectLogin(validUsername, validPin, i);
                    i++;
                }
            }
        }

        internal static void LogOut()
        {
            Console.Clear();
            Console.WriteLine("You are logged out.");
            Thread.Sleep(1500);
            Console.Clear();
        }

    }
}
