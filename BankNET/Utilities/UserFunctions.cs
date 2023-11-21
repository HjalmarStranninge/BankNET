using BankNET.Data;
using BankNET.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace BankNET.Utilities
{
    // Static class containing methods for all functions available to regular users.
    internal static class UserFunctions
    {

        // Method for displaying user account/accounts name and balance.
        internal static void ViewAccountBalance(BankContext context, string username)
        {
            Console.Clear();
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

        // Method for withdraw money.
        internal static void Withdraw(BankContext context, string username)
        {
            Console.Clear();

            // Retrieve user info from the database.
            User? user = context.Users
                .Where(u => u.UserName == username)
                .Include(u => u.Accounts)
                .SingleOrDefault();

            // Displaying the user´s accounts.
            foreach (var account in user.Accounts)
            {
                Console.WriteLine($"{account.Id}. {account.AccountNumber} {account.AccountName}\nBalance: {account.Balance,2} SEK\n");
            }

            Console.Write("Select the account you would like to withdraw from: ");

            Account selectedAccount = null;

            if (!int.TryParse(Console.ReadLine(), out int selectedAccountId) ||
                (selectedAccount = user.Accounts.SingleOrDefault(account => account.Id == selectedAccountId)) == null)
            {
                Console.Write("Invalid input or account number. Withdrawal canceled.");
                Console.Write("Returning to the main menu...");
                Thread.Sleep(2000);
                return;
            }

            Console.Write("How much would you like to withdraw: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
            {
                Console.Write("Invalid input for withdrawal amount. Withdrawal canceled. ");
                Console.Write("Returning to the main menu...");
                Thread.Sleep(2000);
                return;
            }

            // Check if there is sufficient balance.
            if (amount > selectedAccount.Balance)
            {
                Console.Write("Insufficient balance. Withdrawal canceled. ");
                Console.Write("Returning to the main menu...");
                Thread.Sleep(2000);
                return;
            }

            // Update the account balance with the withdrawal.
            selectedAccount.Balance -= amount;

            try
            {
                context.SaveChanges();

                // Displaying the withdrawal details and the updated balance.
                Console.WriteLine($"\nYou have withdrawn {amount,2} SEK from {selectedAccount.AccountName}.");
                Console.WriteLine($"Your new balance for {selectedAccount.AccountNumber} {selectedAccount.AccountName} is: {selectedAccount.Balance,2} SEK\n");
                Console.WriteLine("Press enter to continue. ");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                // Handling any error that might occur during saving.
                Console.WriteLine($"\nError saving changes to the database.");
                Console.Write("Returning to the main menu...");
                Thread.Sleep(2000);
            }
        }


        // Method for deposit money.
        internal static void Deposit(BankContext context, string username)
        {
            Console.Clear();
            // Retrieve user info from the database.
            User? user = context.Users
                .Where(u => u.UserName == username)
                .Include(u => u.Accounts)
                .SingleOrDefault();

            // Displaying the user's accounts.
            foreach (var account in user.Accounts)
            {
                Console.WriteLine($"{account.Id}. {account.AccountName}: {account.Balance,2} SEK\n");
            }

            Console.Write("Select the account you would like to deposit to: ");
            if (!int.TryParse(Console.ReadLine(), out int selectedAccountId))
            {
                Console.Clear();
                Console.WriteLine("Invalid input for account number. Deposit canceled.");
                Console.Write("Returning to the main menu...");
                Thread.Sleep(2000);
                return;
            }

            // Find the selected account.
            var selectedAccount = user.Accounts.SingleOrDefault(account => account.Id == selectedAccountId);

            if (selectedAccount == null)
            {
                Console.Clear();
                Console.WriteLine("Invalid account number. Deposit canceled.");
                Console.Write("Returning to the main menu...");
                Thread.Sleep(2000);
                return;
            }

            Console.Write("How much would you like to deposit: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
            {
                Console.Clear();
                Console.WriteLine("Invalid input for deposit amount. Deposit canceled.");
                Console.Write("Returning to the main menu...");
                Thread.Sleep(2000);
                return;
            }

            // Updating the balance with the deposit.
            selectedAccount.Balance += amount;

            try
            {
                context.SaveChanges();
                // Displaying the deposit details and updated balance.
                Console.Clear();
                Console.WriteLine($"You have deposited {amount,2} SEK into {selectedAccount.AccountName}.");
                Console.WriteLine($"Your new balance for {selectedAccount.AccountName} is: {selectedAccount.Balance,2} SEK.\n");
                Console.Write("Press enter to continue. ");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                // Handling any error that might occur during saving.
                Console.Clear();
                Console.WriteLine($"Error saving changes to the database.");
                Console.Write("Returning to the main menu...");
                Thread.Sleep(2000);
                return;
            }
        }



        // Method for creating new accounts.
        internal static void CreateNewAccount(BankContext context, string username)
        {

            User? user = context.Users
                    .Where(u => u.UserName == username)
                    .Include(u => u.Accounts)
                    .SingleOrDefault();

            Console.Clear();
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

                Console.Clear() ;
                Console.WriteLine($"Successfully created new account with account name: {newAccountName}\n" +
                    $"Your account number is {newAccountNumber}");

                Thread.Sleep(2000);
            }

            else
            {
                Console.Clear();
                Console.Write("Account name cannot be empty. ");
                Console.Write("Returning to the main menu...");
                Thread.Sleep(2000);
            }
            
        }


        // Method for transferring money internally between 2 accounts belonging to the same user.
        internal static void TransferInternal(BankContext context, string username)
        {

            User user = context.Users
            .Include(u => u.Accounts)
            .FirstOrDefault(u => u.UserName == username);

            Console.Clear();
            Console.Write("Transfer to which account? ");
            string accountRecieving = Console.ReadLine();

            Console.Write("\nTransfer from which account? ");
            string accountSending = Console.ReadLine();

            // Checks if the accounts the user wants to transfer between exist.
            if (BankHelpers.IsAccountNameMatching(accountRecieving, user) && BankHelpers.IsAccountNameMatching(accountSending, user))
            {                            
                decimal transferAmount = 0;
                bool validInput = false;

                // Keeps the user stuck in a loop until they enter a valid numeric amount.
                do
                {
                    Console.Clear();
                    Console.Write("Enter amount you wish to transfer: ");
                    try
                    {
                        transferAmount = decimal.Parse(Console.ReadLine());
                        validInput = true;
                    }
                    catch (Exception ex)
                    {
                        Console.Clear();
                        Console.Write($"An error occurred: {ex.Message}");
                        Console.Write("Please enter a valid amount, numbers only. ");
                        Console.Write("Returning to the main menu...");
                        Thread.Sleep(2000);
                    }

                }while(!validInput);
                
                // Creating reference accounts for the sending and recieving accounts.
                Account sendingAccount = user.Accounts.FirstOrDefault(account => account.AccountName.ToLower()  == accountSending.ToLower());
                Account recievingAccount = user.Accounts.FirstOrDefault(account => account.AccountName.ToLower()  == accountRecieving.ToLower());

                // Check if there is enough balance to complete the transaction and proceeds to do so if there is.
                if(BankHelpers.IsThereBalance(sendingAccount, transferAmount))
                {                   
                    DbHelpers.TransferInternal(context, sendingAccount, recievingAccount, transferAmount);                    
                }
                
                // Error message if there isn't enough balance.
                else
                {
                    Console.Clear();
                    Console.Write("Transaction failed. Not enough funds in the account. ");
                    Console.Write("Returning to the main menu...");
                    Thread.Sleep(2000);
                }
            }

            // Error message if the user enters an account name that doesn't match any existing account.
            else
            {
                Console.Clear();
                Console.Write("Account names do not match. Transaction failed. ");
                Console.Write("Returning to the main menu...");
                Thread.Sleep(2000);
            }
        }
        internal static void TransferExternal(BankContext context, string senderUsername)
        {
            User? sender = context.Users
                .Include(u => u.Accounts)
                .FirstOrDefault(u => u.UserName == senderUsername);

            if (sender == null)
            {
                Console.Clear();
                Console.WriteLine("Invalid sender username. Transaction failed. ");
                Console.Write("Returning to the main menu...");
                Thread.Sleep(2000);
                return;
            }

            Console.Clear();
            Console.Write("Transfer to which user: ");
            string receiverUsername = Console.ReadLine();

            User receiver = context.Users
                .Include(u => u.Accounts)
                .FirstOrDefault(u => u.UserName == receiverUsername);

            if (receiver == null)
            {
                Console.Clear();
                Console.WriteLine("Invalid receiver username. Transaction failed. ");
                Console.Write("Returning to the main menu...");
                Thread.Sleep(2000);
                return;
            }

            Console.Write("Transfer from which account: ");
            string accountSending = Console.ReadLine();

            // Move the account input and validation outside of the existence check.
            Account sendingAccount = sender.Accounts.FirstOrDefault(account => account.AccountName.Equals(accountSending, StringComparison.OrdinalIgnoreCase));

            if (sendingAccount == null)
            {
                Console.Clear();
                Console.Write("Invalid sending account. Transaction failed. ");
                Console.Write("Returning to the main menu...");
                Thread.Sleep(2000);
                return;
            }

            Console.Write("Transfer to which account: ");
            string accountReceiving = Console.ReadLine();

            // Checks if the accounts the user wants to transfer between exist.
            if (!BankHelpers.IsAccountNameMatching(accountReceiving, receiver))
            {
                Console.Clear();
                Console.Write("Account names do not match. Transaction failed. ");
                Console.Write("Returning to the main menu...");
                Thread.Sleep(2000);
                return;
            }

            decimal transferAmount;
            bool validInput = false;

            // Keeps the user stuck in a loop until they enter a valid numeric amount.
            do
            {
                Console.Clear();
                Console.Write("Enter amount you wish to transfer: ");

                if (decimal.TryParse(Console.ReadLine(), out transferAmount) && transferAmount > 0)
                {
                    validInput = true;
                }
                else
                {
                    Console.Clear();
                    Console.Write("Invalid input. Please enter a valid amount. ");
                    Console.Write("Returning to the main menu...");
                    Thread.Sleep(2000);
                }
            } while (!validInput);

            // Creating reference accounts for the sending and receiving accounts.
            Account receivingAccount = receiver.Accounts.FirstOrDefault(account => account.AccountName.Equals(accountReceiving, StringComparison.OrdinalIgnoreCase));

            // Check if there is enough balance to complete the transaction and proceeds to do so if there is.
            if (BankHelpers.IsThereBalance(sendingAccount, transferAmount))
            {
                DbHelpers.TransferExternal(context, sendingAccount, receivingAccount, transferAmount);
            }
            // Error message if there isn't enough balance.
            else
            {
                Console.Clear();
                Console.Write("Not enough funds in the account. Transaction failed. ");
                Console.Write("Returning to the main menu...");
                Thread.Sleep(2000);
                return;
            }
        }
    }
}


