using BankNET.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankNET.Utilities
{
    // Class that handles the UI elements of the application. This include header/footer details and arrow key selection.
    internal static class MenuUI
    {

        // Prints user menu and takes input with arrow keys.
        public static void UserMenu(BankContext context, string username)
        {
            int selectedOption = 0;

            // Array with menu options. 
            string[] menuOptions = { "Accounts/Balance", "Transfer between accounts", "Withdrawal", "Deposit", "Open new account", "Log out" };

            // Key variable for navigating menu with arrow keys.
            ConsoleKeyInfo key;

            do
            {
                ClearAndPrintFooter();
                PrintHeader();

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
                        UserFunctions.TransferInternal(context, username);
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
                        Console.WriteLine("Logging you out. Press any key to close.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        // Prints a header with logo.
        public static void PrintHeader()
        {
            Console.Write($"------------------- <> Bank");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("NET");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"\u2122 <> ------------------\n\n\n");
        }

        // Clears console and prints footer at the bottom before resetting cursor position.
        public static void ClearAndPrintFooter()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 9);
            Console.WriteLine("\n\n\n----------------------------------------------------");
            Console.SetCursorPosition(0, 0);

        }

        // Prints login screen and handles input. Reuses a lot of code from UserMenu method.
        public static void LoginScreen(BankContext context)
        {
            int selectedOption = 0;
            string[] menuOptions = { "Login", "Exit" };
            ConsoleKeyInfo key;

            do
            {
                ClearAndPrintFooter();
                PrintHeader();

                Console.WriteLine("\n\t\tWelcome to BankNET!");
                Console.WriteLine("\t     Your trust - our priority\n");

                for (int i = 0; i < menuOptions.Length; i += 2)
                {
                    Console.Write(" ");

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
                    case 0:
                        // Handle Option 1
                        Menus.Login(context);
                        break;

                    case 1:
                        // Exit the application
                        Console.WriteLine("Thank you for using BankNET. Press any key to close.");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
