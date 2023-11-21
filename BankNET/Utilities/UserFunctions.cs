using BankNET.Data;
using BankNET.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BankNET.Utilities
{
    // Static class containing methods for all functions available to regular users.
    internal static class UserFunctions
    {

        // Method for displaying user account/accounts name and balance.
        internal static void ViewAccountBalance(BankContext context, string username)
        {
            MenuUI.ClearAndPrintFooter();
            

            // Retrive user information from database.
            User? user = context.Users
                .Where(u => u.UserName == username)
                .Include(u => u.Accounts)
                .SingleOrDefault();

            // Displaying the account and balance for the user.
            foreach (var account in user.Accounts)
            {
                Console.WriteLine($"{account.AccountNumber} {account.AccountName}\nBalance: {account.Balance,2} SEK\n");
            }
            Console.Write("Press enter to return to menu. ");
            Console.ReadLine();
        }

        // Method for withdrawing money.
        public static void Withdraw(BankContext context, string username)
        {
            User? user = context.Users
                .Where(u => u.UserName == username)
                .Include(u => u.Accounts)
                .SingleOrDefault();

            int selectedOption = 0;

            // Creates a list of accounts and adds all accounts of the current user to it.
            List<Account> menuOptions = new List<Account>();

            foreach (var account in user.Accounts)
            {
                menuOptions.Add(account);
            }

            ConsoleKeyInfo key;

            // Displays the accounts and lets the user choose account using arrow keys.
            do
            {

                MenuUI.ClearAndPrintFooter();
                Console.WriteLine($"   Which account would you like to withdraw from?\n");

                for (int i = 0; i < menuOptions.Count; i++)
                {
                    if (i == selectedOption)
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Black;

                        Console.WriteLine($"{menuOptions[i].AccountNumber} {menuOptions[i].AccountName}\nBalance: {menuOptions[i].Balance,2} SEK");
                        Console.ResetColor();
                    }

                    else
                    {
                        Console.WriteLine($"{menuOptions[i].AccountNumber} {menuOptions[i].AccountName}\nBalance: {menuOptions[i].Balance,2} SEK");
                    }                 
                }

                key = Console.ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:

                        if (selectedOption > 0)
                        {
                            Console.Beep();
                            selectedOption = (selectedOption - 1 + menuOptions.Count) % menuOptions.Count;
                        }
                        break;

                    case ConsoleKey.DownArrow:

                        if (selectedOption < menuOptions.Count - 1)
                        {
                            Console.Beep();
                            selectedOption = (selectedOption + 1) % menuOptions.Count;
                        }


                        break;

                }
            } while (key.Key != ConsoleKey.Enter);

            // Perform action based on the selected option
            if (key.Key == ConsoleKey.Enter)
            {
                Account selectedAccount = menuOptions[selectedOption];
                MenuUI.ClearAndPrintFooter();

                Console.CursorVisible = true;
                Console.WriteLine($"{selectedAccount.AccountNumber} {selectedAccount.AccountName}\nBalance: {selectedAccount.Balance,2} SEK");
                Console.WriteLine("\nHow much would you like to withdraw? ");               

                if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
                {
                    HandleInvalidInput("Invalid input for withdrawal amount. Withdrawal canceled.");
                    return;
                }

                // Check if there is sufficient balance.
                if (amount > selectedAccount.Balance)
                {
                    HandleInvalidInput("Insufficient balance. Withdrawal canceled.");
                    return;
                }

                // Update the account balance with the withdrawal.
                selectedAccount.Balance -= amount;

                // Displaying the withdrawal details and the updated balance.
                try
                {
                    context.SaveChanges();
                    MenuUI.ClearAndPrintFooter();
                    
                    Console.WriteLine($"\nYou have withdrawn {amount,2} SEK from {selectedAccount.AccountName}");
                    Console.WriteLine($"Updated balance: {selectedAccount.Balance,2} SEK\n");
                    Console.Write("\t\tPress ENTER to continue");

                    Console.ReadLine();
                }
              
                // Handling any error that might occur during saving.
                catch (Exception e)
                {
                    MenuUI.ClearAndPrintFooter();
                    
                    Console.WriteLine($"\nError saving changes to the database.");
                    Console.Write("Returning to the main menu...");
                    Thread.Sleep(2000);
                }
                Console.CursorVisible = false;
            }
        }


        // Method for deposit money.
        internal static void Deposit(BankContext context, string username)
        {
            User? user = context.Users
                .Where(u => u.UserName == username)
                .Include(u => u.Accounts)
                .SingleOrDefault();

            int selectedOption = 0;

            // Creates a list of accounts and adds all accounts of the current user to it.
            List<Account> menuOptions = new List<Account>();

            foreach (var account in user.Accounts)
            {
                menuOptions.Add(account);
            }

            ConsoleKeyInfo key;

            // Displays the accounts and lets the user choose account using arrow keys.
            do
            {
                MenuUI.ClearAndPrintFooter();
                Console.WriteLine($"   Which account would you like to deposit into?\n");

                for (int i = 0; i < menuOptions.Count; i++)
                {
                    if (i == selectedOption)
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Black;

                        Console.WriteLine($"{menuOptions[i].AccountNumber} {menuOptions[i].AccountName}\nBalance: {menuOptions[i].Balance,2} SEK");
                        Console.ResetColor();
                    }

                    else
                    {
                        Console.WriteLine($"{menuOptions[i].AccountNumber} {menuOptions[i].AccountName}\nBalance: {menuOptions[i].Balance,2} SEK");
                    }
                }

                key = Console.ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        
                        if(selectedOption > 0)
                        {
                            Console.Beep();
                            selectedOption = (selectedOption - 1 + menuOptions.Count) % menuOptions.Count;
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        
                        if (selectedOption < menuOptions.Count - 1)
                        {
                            Console.Beep();
                            selectedOption = (selectedOption + 1) % menuOptions.Count;
                        }
                        
                        break;

                }
            } while (key.Key != ConsoleKey.Enter);

            // Perform action based on the selected option
            if (key.Key == ConsoleKey.Enter)
            {
                Account selectedAccount = menuOptions[selectedOption];
                MenuUI.ClearAndPrintFooter();

                Console.CursorVisible = true;
                Console.WriteLine($"{selectedAccount.AccountNumber} {selectedAccount.AccountName}\nBalance: {selectedAccount.Balance,2} SEK");
                Console.WriteLine("\nHow much would you like to deposit? ");

                if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
                {
                    // Updating the balance with the deposit.
                    selectedAccount.Balance += amount;

                    // Displaying the deposit details and updated balance.
                    try
                    {
                        MenuUI.ClearAndPrintFooter();
                        context.SaveChanges();                       
                        Console.WriteLine($"\nYou have deposited {amount,2} SEK into {selectedAccount.AccountName}.");
                        Console.WriteLine($"Updated balance: {selectedAccount.Balance,2} SEK\n");
                        Console.Write("\t\tPress ENTER to continue");
                        Console.ReadLine();
                    }

                    // Handling any error that might occur during saving.
                    catch (Exception e)
                    {                      
                        Console.WriteLine($"\nError saving changes to the database.");
                        Console.Write("Returning to the main menu...");
                        Thread.Sleep(2000);
                    }
                }             
                Console.CursorVisible = false;
            }
        }



        // Method for creating new accounts.
        internal static void CreateNewAccount(BankContext context, string username)
        {

            User? user = context.Users
                    .Where(u => u.UserName == username)
                    .Include(u => u.Accounts)
                    .SingleOrDefault();

            MenuUI.ClearAndPrintFooter();
            
            Console.Write("\n    Enter new account name: ");

            string newAccountName = Console.ReadLine();
                
            // Generates a new account number and checks if it is unique. If it isn't, a new one is generated until it becomes unique.
            string newAccountNumber;
            do
            {
                newAccountNumber = BankHelpers.GenerateAccountNumber();

            } while (!BankHelpers.IsAccountNumberUnique(newAccountNumber));

            // Checks if the proposed name that the user entered isn't null. If it isn't, a new account is created.
            if (!string.IsNullOrWhiteSpace(newAccountName))
            {
                DbHelpers.CreateNewAccount(context, newAccountName, newAccountNumber, user);

                MenuUI.ClearAndPrintFooter();
                Console.WriteLine($"\n\t\t   Account created!\n" +
                    $"\t   Your account number is {newAccountNumber}");

                Thread.Sleep(2000);
            }

            else
            {
                MenuUI.ClearAndPrintFooter();
                Console.WriteLine("\t\tAccount name cannot be empty");

                Thread.Sleep(2000);
            }
            
        }

        // Method for transferring money. User can choose whether they want to transfer between their own accounts or to another user.
        internal static void TransferOptions(BankContext context, string username)
        {

            User? user = context.Users
            .Include(u => u.Accounts)
            .FirstOrDefault(u => u.UserName == username);

            MenuUI.ClearAndPrintFooter();

            int selectedOption = 0;
            string[] menuOptions = { "Internal", "External" };
            ConsoleKeyInfo key;

            do
            {
                MenuUI.ClearAndPrintFooter();

                Console.WriteLine("\n\t\tChoose transfer option");               

                for (int i = 0; i < menuOptions.Length; i += 2)
                {
                    Console.Write(" ");

                    // Highlights the currently selected option.
                    if (i == selectedOption)
                    {
                        Console.Write("\n\t     ");
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Black;

                        Console.Write($"{menuOptions[i]}");
                        Console.ResetColor();
                        Console.Write($"".PadRight(19 - menuOptions[i].Length));
                    }

                    else
                    {
                        Console.Write("\n\t     ");
                        Console.Write($"{menuOptions[i]}".PadRight(19));
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

                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        if (selectedOption % 2 == 1 && selectedOption > 0)
                        {
                            Console.Beep();
                            selectedOption = (selectedOption - 1) % menuOptions.Length;
                        }
                        break;

                    case ConsoleKey.RightArrow:
                        if (selectedOption % 2 == 0 && selectedOption + 1 < menuOptions.Length)
                        {
                            Console.Beep();
                            selectedOption = (selectedOption + 1) % menuOptions.Length;
                        }
                        break;
                }

            } while (key.Key != ConsoleKey.Enter);

            if (key.Key == ConsoleKey.Enter)
            {
                switch (selectedOption)
                {
                    
                    case 0:
                        Transfer.TransferInternal(context, username);
                        break;
                   
                    case 1:

                        Transfer.TransferExternal(context, username);
                        break;
                }
            }          
         
        }
        private static void HandleInvalidInput(string message)
        {
            Console.WriteLine($"\n{message}");
            Console.Write("Returning to the main menu...");
            Thread.Sleep(2000);
        }
    }
}


