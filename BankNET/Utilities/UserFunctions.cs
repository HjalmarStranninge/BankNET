using BankNET.Data;
using BankNET.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankNET.Models;
using Microsoft.EntityFrameworkCore.Storage;

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
            Console.Write("Press enter to return to menu.");
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
                    case ConsoleKey.DownArrow:
                        Console.Beep();
                        selectedOption = (selectedOption - 2 + menuOptions.Count) % menuOptions.Count;
                        break;

                    case ConsoleKey.UpArrow:
                        Console.Beep();
                        selectedOption = (selectedOption + 2) % menuOptions.Count;
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

        private static void HandleInvalidInput(string message)
        {
            Console.WriteLine($"\n{message}");
            Console.Write("Returning to the main menu...");
            Thread.Sleep(2000);
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
                    case ConsoleKey.DownArrow:
                        Console.Beep();
                        selectedOption = (selectedOption - 2 + menuOptions.Count) % menuOptions.Count;
                        break;

                    case ConsoleKey.UpArrow:
                        Console.Beep();
                        selectedOption = (selectedOption + 2) % menuOptions.Count;
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
        public static void CreateNewAccount(BankContext context, string username)
        {

            User? user = context.Users
                    .Where(u => u.UserName == username)
                    .Include(u => u.Accounts)
                    .SingleOrDefault();

            MenuUI.ClearAndPrintFooter();
            
            Console.Write("Enter new account name: ");

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
                Console.WriteLine($"Successfully created new account with account name: {newAccountName}\n" +
                    $"Your account number is {newAccountNumber}");

                Thread.Sleep(2000);
            }

            else
            {
                MenuUI.ClearAndPrintFooter();
                Console.WriteLine("\t\tAccount name cannot be empty");

                Thread.Sleep(2000);
            }
            
        }

        // Method for transferring money internally between 2 accounts belonging to the same user.
        public static void TransferInternal(BankContext context, string username)
        {

            User? user = context.Users
            .Include(u => u.Accounts)
            .FirstOrDefault(u => u.UserName == username);

            MenuUI.ClearAndPrintFooter();

            Console.CursorVisible = true;
            Console.WriteLine("\t      Transfer to which account?");
            Console.Write("\t      ");
            string? accountReceiving = Console.ReadLine();

            Console.WriteLine("\n\t      Transfer from which account?");
            Console.Write("\t      ");
            string? accountSending = Console.ReadLine();
            

            // Checks if the accounts the user wants to transfer between exist.
            if (BankHelpers.IsAccountNameMatching(accountReceiving, user) && BankHelpers.IsAccountNameMatching(accountSending, user))
            {                            
                decimal transferAmount = 0;
                bool validInput = false;

                // Keeps the user stuck in a loop until they enter a valid numeric amount.
                do
                {
                    MenuUI.ClearAndPrintFooter();
                    
                    Console.Write("\t Enter amount you wish to transfer: ");
                    try
                    {
                        transferAmount = decimal.Parse(Console.ReadLine());
                        validInput = true;
                        Console.CursorVisible = false;
                    }
                    catch (Exception ex)
                    {
                        MenuUI.ClearAndPrintFooter();
                        
                        Console.WriteLine($"An error occurred: {ex.Message}");
                        Console.WriteLine("Please enter a valid amount, numbers only.");
                        Thread.Sleep(2000);
                    }

                }while(!validInput);
                
                // Creating reference accounts for the sending and recieving accounts.
                Account sendingAccount = user.Accounts.FirstOrDefault(account => account.AccountName.ToLower()  == accountSending.ToLower());
                Account recievingAccount = user.Accounts.FirstOrDefault(account => account.AccountName.ToLower()  == accountReceiving.ToLower());

                // Check if there is enough balance to complete the transaction and proceeds to do so if there is.
                if(BankHelpers.IsThereBalance(sendingAccount, transferAmount))
                {                   
                    DbHelpers.TransferInternal(context, sendingAccount, recievingAccount, transferAmount);                    
                }
                
                // Error message if there isn't enough balance.
                else
                {
                    MenuUI.ClearAndPrintFooter();
                    Console.WriteLine("Transaction failed. Not enough funds in the account.");
                    Thread.Sleep(2000);
                }
            }

            // Error message if the user enters an account name that doesn't match any existing account.
            else
            {
                MenuUI.ClearAndPrintFooter();
                Console.WriteLine("Account names do not match. Transaction failed.");
                Thread.Sleep(2000);
            }
        }
    }
}


