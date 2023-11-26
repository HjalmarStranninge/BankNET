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
        static int numberOfTries = 1;

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
            Console.WriteLine("\n\n\t    Press ENTER to return to menu");
            Console.ReadLine();
        }

        // Method for withdrawing money.
        internal static void Withdraw(BankContext context, string username)
        {
            bool validPin;

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
                Console.Beep();

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:

                        if (selectedOption > 0)
                        {
                            selectedOption = (selectedOption - 1 + menuOptions.Count) % menuOptions.Count;
                        }
                        break;

                    case ConsoleKey.DownArrow:

                        if (selectedOption < menuOptions.Count - 1)
                        {
                            selectedOption = (selectedOption + 1) % menuOptions.Count;
                        }

                        break;

                }
            } while (key.Key != ConsoleKey.Enter);

            // Perform action based on the selected option
            if (key.Key == ConsoleKey.Enter)
            {
                bool invalidInputAmount = false;
 
                Account selectedAccount = menuOptions[selectedOption];
                MenuUI.ClearAndPrintFooter();

                Console.CursorVisible = true;
                Console.WriteLine($"{selectedAccount.AccountNumber} {selectedAccount.AccountName}\nBalance: {selectedAccount.Balance,2} SEK");
                Console.WriteLine("\nHow much would you like to withdraw? ");
                
                // Loop to give correct response to user input on amount to withdraw
                do 
                {
                    if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
                    {
                        InvalidInputHandling.InvalidInputAmount();
                        Console.CursorVisible = false;
                        invalidInputAmount = true;
                    }
                    // If not sufficient balance, returns user to menu.
                    if (amount > selectedAccount.Balance)
                    {
                        MenuUI.ClearAndPrintFooter();
                        Console.WriteLine($"\n\nInsufficient balance. Withdrawal canceled.");
                        Console.Write("Returning to main menu...");
                        Thread.Sleep(2000);
                        Console.CursorVisible = false;
                        return;
                    }
                    // Loop to make sure user cannot write wrong pin more than 3 times
                    do
                    {
                        // User PIN confirmation needed after accepted amount
                        Console.Write("\nPlease enter PIN to confirm: ");
                        validPin = BankHelpers.PinCheck(context, username);
                        if (validPin)
                        {
                            // Update the account balance with the withdrawal.
                            selectedAccount.Balance -= amount;

                            // Displaying the withdrawal details and the updated balance.
                            try
                            {
                                context.SaveChanges();
                                MenuUI.ClearAndPrintFooter();
                                Console.CursorVisible = false;

                                Console.WriteLine($"\nYou have withdrawn {amount,2} SEK from {selectedAccount.AccountName}");
                                Console.WriteLine($"Updated balance: {selectedAccount.Balance,2} SEK");
                                Console.Write("\n\t\tPress ENTER to continue");
                                LogInLogOut.UserPinInputAttempts[username] = 0;

                                Console.ReadLine();
                                Console.Beep();
                            }
                            // Handling any error that might occur during saving.
                            catch (Exception e)
                            {
                                MenuUI.ClearAndPrintFooter();
                                Console.CursorVisible = false;

                                Console.WriteLine($"\nError saving changes to the account.");
                                Console.Write("Returning to the main menu...");

                                Thread.Sleep(2000);
                            }
                        }
                        else
                        {
                            Console.CursorVisible = false;
                            //InvalidInputHandling.IncorrectNameOrPin(username, "\n\t            Incorrect pin.");
                            InvalidInputHandling.AttemptsTracker(username, numberOfTries);
                        }
                    } while (!InvalidInputHandling.IsLockedOut(username) && !validPin);
                } while (invalidInputAmount && !InvalidInputHandling.IsLockedOut(username));
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
                Console.Beep();

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        
                        if(selectedOption > 0)
                        {
                            selectedOption = (selectedOption - 1 + menuOptions.Count) % menuOptions.Count;
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        
                        if (selectedOption < menuOptions.Count - 1)
                        {
                            selectedOption = (selectedOption + 1) % menuOptions.Count;
                        }
                        
                        break;

                }
            } while (key.Key != ConsoleKey.Enter);
            
            // Perform action based on the selected option
            if (key.Key == ConsoleKey.Enter)
            {
                Account selectedAccount = menuOptions[selectedOption];
                //MenuUI.ClearAndPrintFooter();

                bool invalidInput = true;
                do
                {
                    MenuUI.ClearAndPrintFooter();
                    Console.CursorVisible = true;
                    Console.WriteLine($"{selectedAccount.AccountNumber} {selectedAccount.AccountName}\nBalance: {selectedAccount.Balance,2} SEK");
                    Console.Write("\nHow much would you like to deposit? ");
                    string userInputDeposit = Console.ReadLine();

                    Console.CursorVisible = false;
                    MenuUI.ClearAndPrintFooter();

                    if (decimal.TryParse(userInputDeposit, out decimal amount) && amount > 0)
                    {
                        // Updating the balance with the deposit.
                        selectedAccount.Balance += amount;

                        // Displaying the deposit details and updated balance.
                        try
                        {
                            context.SaveChanges();
                            Console.WriteLine($"\nYou have deposited {amount:F2} SEK into {selectedAccount.AccountName}.");
                            Console.WriteLine($"Updated balance: {selectedAccount.Balance,2} SEK");
                            Thread.Sleep(1000);
                            Console.Write("\n\t\tPress ENTER to continue");

                            Console.ReadLine();
                            Console.Beep();
                        }
                        // Handling any error that might occur during saving.
                        catch (Exception e)
                        {
                            Console.WriteLine($"\nError saving changes to the database.");
                            Console.Write("Returning to the main menu...");
                            Thread.Sleep(2000);
                        }
                        invalidInput = false;
                    }
                    // Writes out that the input is invalid and makes sure the loop is still going.
                    else
                    {
                        InvalidInputHandling.InvalidInputAmount();
                        Console.Clear();
                    }
                } while (invalidInput);
            } 
        }

        // Method for creating new accounts.
        internal static void CreateNewAccount(BankContext context, string username)
        {
            User? user = context.Users
                    .Where(u => u.UserName == username)
                    .Include(u => u.Accounts)
                    .SingleOrDefault();
            
            bool accountNameIsNull = true;

            // Checks if the proposed name that the user enter isn't null. If it isn't, a new account is created.
            do
            {
                MenuUI.ClearAndPrintFooter();
            
                Console.Write("\n    Enter new account name: ");
                Console.CursorVisible = true;

                string newAccountName = Console.ReadLine();
                Console.CursorVisible = false;

                accountNameIsNull = string.IsNullOrWhiteSpace(newAccountName);

                MenuUI.ClearAndPrintFooter();
                Console.CursorVisible = false;
                if (!accountNameIsNull)
                {
                    // Generates a new account number and checks if it is unique. If it isn't, a new one is generated until it becomes unique.
                    string newAccountNumber;
                    do
                    {
                        newAccountNumber = BankHelpers.GenerateAccountNumber();

                    } while (!BankHelpers.IsAccountNumberUnique(newAccountNumber));
                    DbHelpers.CreateNewAccount(context, newAccountName, newAccountNumber, user);

                    Console.WriteLine($"\n\t\t   Account created!\n" +
                        $"\t   Your account number is {newAccountNumber}");
                    Thread.Sleep(3000);
                    accountNameIsNull = false;
                }
                else
                {
                    InvalidInputHandling.InvalidInputName();
                }
            } while (accountNameIsNull);
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
    }
}


