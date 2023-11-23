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
            int loginAttempts = 3;
           
            while (tryAgainLogin && loginAttempts > 1)
            {
                int selectedOption = 0;
                string[] menuOptions = { "Login", "Exit" };
                ConsoleKeyInfo key;

                // Prints menu UI and lets user choose either login or exit with arrow keys.
                do
                {
                    MenuUI.ClearAndPrintFooter();

                    Console.WriteLine("\n\t\t Welcome to BankNET!");
                    Console.WriteLine("\t      Your trust - our priority\n");

                    for (int i = 0; i < menuOptions.Length; i += 2)
                    {
                        Console.Write(" ");

                        // Highlights the currently selected option.
                        if (i == selectedOption)
                        {
                            Console.Write("\n\t    ");
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.ForegroundColor = ConsoleColor.Black;

                            Console.Write($"{menuOptions[i]}");
                            Console.ResetColor();                        
                            Console.Write($"".PadRight(23 - menuOptions[i].Length));                            
                        }
                        else
                        {
                            Console.Write("\n\t    ");
                            Console.Write($"{menuOptions[i]}".PadRight(23));
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
                            }
                            else
                            {
                                Console.Write($"{menuOptions[i + 1]}");
                            }
                        }
                        Console.WriteLine();
                    }                                      
                   
                    key = Console.ReadKey();
                    Console.Beep();

                    switch (key.Key)
                    {                      
                        case ConsoleKey.LeftArrow:
                            if (selectedOption % 2 == 1 && selectedOption > 0)
                            {
                                selectedOption = (selectedOption - 1) % menuOptions.Length;
                            }
                            break;

                        case ConsoleKey.RightArrow:
                            if (selectedOption % 2 == 0 && selectedOption + 1 < menuOptions.Length)
                            {
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
                            InvalidInputHandling.LockOutReleaseTimer();
                            bool loginSuccesful = false;
                            while (!loginSuccesful)
                            {
                                MenuUI.ClearAndPrintFooter();
                                Console.CursorVisible = true;

                                Console.Write("\n\tEnter username: ");

                                string username = Console.ReadLine();
                                Console.CursorVisible = false;
                                Console.Beep();

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
                                    if (validUsernameAndPin)
                                    {
                                        Console.WriteLine("Login successful!");
                                        tryAgainLogin = false;

                                        loginSuccesful = true;
                                        return username;

                                    }
                                    else
                                    {
                                        loginAttempts--;
                                        InvalidInputHandling.IncorrectNameOrPin(loginAttempts, "\n\t    Invalid username and/or pin.");
                                    }
                                }
                                else
                                {
                                    break;
                                }        
                            }
                            break;
                            
                        // Exits the application
                        case 1:      
                            MenuUI.ClearAndPrintFooter();
                            Console.WriteLine("\n\t   Thank you for using BankNET!");
                            Thread.Sleep(1000);
                            Console.WriteLine("\n\t      Exiting application...");
                            Thread.Sleep(3000);

                            Console.Clear();
                            Environment.Exit(0);
                            break;
                    }
                }
            }
            return null;
        }

    }
}
