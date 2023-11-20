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
        internal static string Login(BankContext context)
        {
            bool tryAgainLogin = true;
            int loginAttempts = 0;
           
            while (tryAgainLogin && loginAttempts < 3)
            {
                int selectedOption = 0;
                string[] menuOptions = { "Login", "Exit" };
                ConsoleKeyInfo key;

                // Prints menu UI and lets user choose either login or exit with arrow keys.
                do
                {
                    MenuUI.ClearAndPrintFooter();

                    Console.WriteLine("\n\t\tWelcome to BankNET!");
                    Console.WriteLine("\t     Your trust - our priority\n");

                    for (int i = 0; i < menuOptions.Length; i += 2)
                    {
                        Console.Write(" ");

                        // Highlights the currently selected option.
                        if (i == selectedOption)
                        {
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write($"\n\t{menuOptions[i]}");
                            Console.ResetColor();

                            if (menuOptions[i].Length == 16)
                            {
                                Console.Write($"".PadRight(9));
                            }
                            else
                            {
                                Console.Write($"".PadRight(25 - menuOptions[i].Length));
                            }
                        }

                        else
                        {
                            Console.Write($"\n\t{menuOptions[i]}".PadRight(25));
                        }

                        if (i + 1 < menuOptions.Length)
                        {
                            Console.Write(" ");

                            if (i + 1 == selectedOption)
                            {
                                Console.BackgroundColor = ConsoleColor.Gray;
                                Console.ForegroundColor = ConsoleColor.Black;
                                Console.Write($"{menuOptions[i + 1]}");
                                Console.ResetColor();
                                Console.Write($"".PadRight(14));
                            }
                            else
                            {
                                Console.Write($"{menuOptions[i + 1]}".PadRight(25));
                            }
                        }
                        Console.WriteLine();
                    }

                    key = Console.ReadKey();

                    switch (key.Key)
                    {
                        case ConsoleKey.UpArrow:
                            Console.Beep();
                            selectedOption = (selectedOption - 2 + menuOptions.Length) % menuOptions.Length;
                            break;

                        case ConsoleKey.DownArrow:
                            Console.Beep();
                            selectedOption = (selectedOption + 2) % menuOptions.Length;
                            break;

                        case ConsoleKey.LeftArrow:
                            if (selectedOption % 2 == 1 && selectedOption > 0)
                            {
                                // Move left only if the selected option is on the right and not on the first column.
                                Console.Beep();
                                selectedOption = (selectedOption - 1) % menuOptions.Length;
                            }
                            break;

                        case ConsoleKey.RightArrow:
                            if (selectedOption % 2 == 0 && selectedOption + 1 < menuOptions.Length)
                            {
                                // Move right only if the selected option is on the left and there's an option on the right.
                                Console.Beep();
                                selectedOption = (selectedOption + 1) % menuOptions.Length;
                            }
                            break;
                    }

                } while (key.Key != ConsoleKey.Enter);

                // Perform action based on the selected option
                if (key.Key == ConsoleKey.Enter)
                {
                    switch (selectedOption)
                    {
                        // Login user with username and pin. Returns username.
                        case 0:

                            bool loginSuccesful = false;
                            while (!loginSuccesful)
                            {
                                MenuUI.ClearAndPrintFooter();
                                Console.CursorVisible = true;

                                Console.Write("\n\tEnter username: ");
                                string username = Console.ReadLine();

                                Console.Write("\n\tEnter pin: ");

                                ConsoleKeyInfo keyInfo;
                                string pin = "";
                                do
                                {
                                    // Read a key without displaying it.
                                    keyInfo = Console.ReadKey(true);
                                    if (keyInfo.Key != ConsoleKey.Backspace && keyInfo.Key != ConsoleKey.Enter)
                                    {
                                        // Display a star for each character.
                                        pin += keyInfo.KeyChar;
                                        Console.Write("*");
                                    }
                                    else if (keyInfo.Key == ConsoleKey.Backspace && pin.Length > 0)
                                    {
                                        // Clear the last character and move the cursor back.
                                        pin = pin.Substring(0, pin.Length - 1);
                                        Console.Write("\b \b");
                                    }
                                }
                                while (keyInfo.Key != ConsoleKey.Enter);

                                Console.CursorVisible = false;

                                // Checks if username and pin matches any users in the database.
                                bool validUsername = context.Users.Any(uN => uN.UserName.Equals(username));
                                bool validPin = context.Users.Any(p => p.UserName.Equals(username) && p.Pin.Equals(pin));

                                // Returns username if everything checks out.
                                if (validUsername && validPin)
                                {
                                    Console.WriteLine("Login successful!");
                                    tryAgainLogin = false;

                                    return username;
                                    loginSuccesful = true;
                                }

                                else
                                {
                                    InvalidInputHandling.IncorrectLogin(validUsername, validPin, loginAttempts);
                                    loginAttempts++;
                                }
                                
                            }
                            
                            break;
                            
                        // Exits the application
                        case 1:      
                            MenuUI.ClearAndPrintFooter();
                            Console.WriteLine("\n\t   Thank you for using BankNET!");
                            Console.Write("\n\t      Press any key to exit");

                            Console.CursorVisible = true;
                            Console.ReadKey();
                            

                            Console.Clear();
                            Environment.Exit(0);
                            break;
                    }
                }
            }
            return null;
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
