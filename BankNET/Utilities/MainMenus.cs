using BankNET.Data;
using BankNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace BankNET.Utilities
{
    // Class containing methods that print the different menus.
    internal static class MainMenus
    {
        // Method for user main menu after successful login
        internal static void UserMainMenu(BankContext context, string username)
        {
            bool runMenu = true;

            while (runMenu)
            {
                int selectedOption = 0;

                // Array with menu options. 
                string[] menuOptions = { "Accounts/Balance", "Transfer between accounts", "Withdrawal", "Deposit", "Open new account", "Log out" };

                // Key variable for navigating menu with arrow keys.
                ConsoleKeyInfo key;

                do
                {
                    MenuUI.ClearAndPrintFooter();

                    // Prints menu options and highlights the currently selected option.
                    for (int i = 0; i < menuOptions.Length; i += 2)
                    {
                        Console.Write(" ");

                        if (i == selectedOption)
                        {
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write($"{menuOptions[i]}");
                            Console.ResetColor();

                            // Adjusts padding depending on length of the word. The longest word is 16 characters long so the padding is based on that.
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
                            Console.Write($"{menuOptions[i]}".PadRight(25));
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

                    // Update selected option based on arrow key input.
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
                        case 0:
                            // Handle Option 1
                            UserFunctions.ViewAccountBalance(context, username);
                            break;

                        case 1:
                            // Handle Option 2
                            UserFunctions.TransferOptions(context, username);
                            break;

                        case 2:
                            // Handle Option 3
                            UserFunctions.Withdraw(context, username);
                            break;
                        case 3:
                            // Handle Option 4
                            UserFunctions.Deposit(context, username);
                            break;
                        case 4:
                            // Handle Option 5
                            UserFunctions.CreateNewAccount(context, username);
                            break;

                        case 5:
                            // Exit the application
                            MenuUI.ClearAndPrintFooter();
                            Console.WriteLine("\n\n\t   Logging you out. Please wait..");
                            runMenu = false;
                            Thread.Sleep(2000);
                            break;
                    }
                }
            }          
        }

        //Method for adminstrator menu after successful log in
        internal static void AdminMenu(BankContext context, string adminName)
        {
            bool runMenu = true;

            // Key variable for navigating menu with arrow keys.
            ConsoleKeyInfo key;

            while (runMenu)
            {
                int selectedOption = 0;

                string[] menuOptions = { "View all users", "Create new user", "Log out" };
                do
                {
                    MenuUI.ClearAndPrintFooter();

                    for (int i = 0; i < menuOptions.Length; i++)
                    {
                        if (i == selectedOption)
                        {
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.ForegroundColor = ConsoleColor.Black;

                            Console.WriteLine($"\n\t{menuOptions[i]}");
                            Console.ResetColor();
                        }

                        else
                        {
                            Console.WriteLine($"\n\t{menuOptions[i]}");
                        }
                    }

                    key = Console.ReadKey();

                    switch (key.Key)
                    {
                        case ConsoleKey.DownArrow:
                            Console.Beep();
                            selectedOption = (selectedOption - 2 + menuOptions.Length) % menuOptions.Length;
                            break;

                        case ConsoleKey.UpArrow:
                            Console.Beep();
                            selectedOption = (selectedOption + 2) % menuOptions.Length;
                            break;

                    }
                } while (key.Key != ConsoleKey.Enter);

                if (key.Key == ConsoleKey.Enter)
                {
                    switch (selectedOption)
                    {
                        case 0:
                            // Handle Option 1
                            AdminFunctions.ViewUsers(context, adminName);
                            break;

                        case 1:
                            // Handle Option 2
                            AdminFunctions.CreateUser(context);
                            break;

                        case 2:
                            // Handle Option 3
                            MenuUI.ClearAndPrintFooter();
                            Console.WriteLine("\n\n\t   Logging you out. Please wait..");
                            runMenu = false;
                            Thread.Sleep(2000);
                            break;

                    }
                }
            }
        }
    }
}
