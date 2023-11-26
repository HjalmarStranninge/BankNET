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
        // Method for user menu after successful login
        internal static void UserMainMenu(BankContext context, string username)
        {
            bool runMenu = true;

            // Runs menu as long as user is not locked out or chooses to log out.
            while (runMenu && !InvalidInputHandling.IsLockedOut())
            {
                int selectedOption = 0;

                // Array with menu options. 
                string[] userMenuOptions = { "Accounts/Balance", "Transfer between accounts", "Withdrawal", "Deposit", "Open new account", "Log out" };

                // Key variable for navigating menu with arrow keys.
                ConsoleKeyInfo key;

                do
                {
                    MenuUI.ClearAndPrintFooter();

                    // Prints menu options and highlights the currently selected option.
                    for (int i = 0; i < userMenuOptions.Length; i += 2)
                    {
                        Console.Write(" ");

                        if (i == selectedOption)
                        {
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write($"{userMenuOptions[i]}");
                            Console.ResetColor();

                            // Adjusts padding depending on length of the word. The longest word is 16 characters long so the padding is based on that.
                            if (userMenuOptions[i].Length == 16)
                            {
                                Console.Write($"".PadRight(9));
                            }
                            else
                            {
                                Console.Write($"".PadRight(25 - userMenuOptions[i].Length));
                            }
                        }
                        else
                        {
                            Console.Write($"{userMenuOptions[i]}".PadRight(25));
                        }

                        if (i + 1 < userMenuOptions.Length)
                        {
                            Console.Write(" ");

                            if (i + 1 == selectedOption)
                            {
                                Console.BackgroundColor = ConsoleColor.Gray;
                                Console.ForegroundColor = ConsoleColor.Black;
                                Console.Write($"{userMenuOptions[i + 1]}");
                                Console.ResetColor();
                                Console.Write($"".PadRight(14));
                            }
                            else
                            {
                                Console.Write($"{userMenuOptions[i + 1]}".PadRight(25));
                            }
                        }

                        Console.WriteLine();
                    }
                    
                    key = Console.ReadKey();
                    Console.Beep();

                    // Update selected option based on arrow key input.
                    switch (key.Key)
                    {
                        case ConsoleKey.UpArrow:
                            selectedOption = (selectedOption - 2 + userMenuOptions.Length) % userMenuOptions.Length;
                            break;

                        case ConsoleKey.DownArrow:
                            selectedOption = (selectedOption + 2) % userMenuOptions.Length;
                            break;

                        case ConsoleKey.LeftArrow:
                            if (selectedOption % 2 == 1 && selectedOption > 0)
                            {
                                // Move left only if the selected option is on the right and not on the first column.
                                selectedOption = (selectedOption - 1) % userMenuOptions.Length;
                            }
                            break;

                        case ConsoleKey.RightArrow:
                            if (selectedOption % 2 == 0 && selectedOption + 1 < userMenuOptions.Length)
                            {
                                // Move right only if the selected option is on the left and there's an option on the right.
                                selectedOption = (selectedOption + 1) % userMenuOptions.Length;
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
                            // Returning to the start page
                            runMenu = LogInLogOut.LogOut();
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

            while (runMenu && !InvalidInputHandling.IsLockedOut())
            {
                int selectedOption = 0;

                string[] adminMenuOptions = { "View all users", "Create new user", "Log out" };
                // Showing menu with the option to move between menu options with arrow keys until user chooses option with ENTER
                do
                {
                    MenuUI.ClearAndPrintFooter();

                    for (int i = 0; i < adminMenuOptions.Length; i++)
                    {
                        if (i == selectedOption)
                        {
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.ForegroundColor = ConsoleColor.Black;

                            Console.WriteLine($"\n\t{adminMenuOptions[i]}");
                            Console.ResetColor();
                        }

                        else
                        {
                            Console.WriteLine($"\n\t{adminMenuOptions[i]}");
                        }
                    }
                
                    key = Console.ReadKey();
                    Console.Beep();

                    switch (key.Key)
                    {
                        case ConsoleKey.DownArrow:
                            selectedOption = (selectedOption - 2 + adminMenuOptions.Length) % adminMenuOptions.Length;
                            break;

                        case ConsoleKey.UpArrow:
                            selectedOption = (selectedOption + 2) % adminMenuOptions.Length;
                            break;

                    }
                } while (key.Key != ConsoleKey.Enter);

                // If user presses enter, the option highlighted in the menu is chosen
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
                            runMenu = LogInLogOut.LogOut();
                            break;

                    }
                }
            }
        }
    }
}
