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
            Console.Clear();
            // Retrive user information from database.
            User? user = context.Users
                .Where(u => u.UserName == username)
                .Include(u => u.Accounts)
                .SingleOrDefault();

            // Displaying the account and balance for the user.
            foreach (var account in user.Accounts)
            {
                Console.WriteLine($"{account.AccountName}: {account.Balance} SEK");
            }
            
            Console.Write("Press enter to continue.");
            Console.ReadLine();
        }

        // Method for withdraw money.
        internal static void Withdraw(BankContext context, string username)
        {
            Console.Clear();

            // Retrive user info from database.
            User? user = context.Users
                    .Where(u => u.UserName == username)
                    .Include(u => u.Accounts)
                    .SingleOrDefault();

            // Displaying the user´s accounts.
            foreach (var account in user.Accounts)
            {
                Console.WriteLine($"{account.Id}. {account.AccountName}: {account.Balance}");
            }
            
            Console.WriteLine("Select the account you would like to withdraw from : ");
            if (int.TryParse(Console.ReadLine(), out int selectedAccountId))
            {
                Console.WriteLine("How much would you like to withdraw: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
                {
                    // Find the selected account.
                    var selectedAccount = user.Accounts.SingleOrDefault(account => account.Id == selectedAccountId);

                    if (selectedAccount != null)
                    {
                        // Check if there is sufficient balance.
                        if (amount <= selectedAccount.Balance)
                        {
                            // Update the account balance with the withdrawal.
                            selectedAccount.Balance -= amount;

                            try
                            {
                                context.SaveChanges();

                                // Displaying the withdraw details and the updated balance.
                                Console.WriteLine($"You have withdrawn {amount:C} from {selectedAccount.AccountName}.");
                                Console.WriteLine($"Your new balance for {selectedAccount.AccountName} is: {selectedAccount.Balance:C}");
                                Console.Write("Press enter to continue.");
                                Console.ReadLine();
                            }
                            catch (Exception e)
                            {
                                // Handling any error that might occur during saving.
                                Console.WriteLine($"Error saving changes to the database");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Insufficient balance. Withdrawal canceled.");
                        }
                    }
                    else
                    {            
                        Console.WriteLine("Invalid account number. Withdrawal canceled.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input for account number. Withdrawal canceled.");
                }
            }
            else
            {               
                Console.WriteLine("Invalid input for withdrawal amount. Withdrawal canceled.");
            }
        }

        // Method for deposit money.
        internal static void Deposit(BankContext context, string username)
        {
            Console.Clear();
            // Retrive user info from database.
            User? user = context.Users
                    .Where(u => u.UserName == username)
                    .Include(u => u.Accounts)
                    .SingleOrDefault();

            //Displaying the user´s account.
            foreach (var account in user.Accounts)
            {
                Console.WriteLine($"{account.Id}. {account.AccountName}: {account.Balance}");
            }
            Console.WriteLine("Select the account you would like to deposit to: ");
            if (int.TryParse(Console.ReadLine(), out int selectedAccountId))
            {
                Console.WriteLine("How much would you like to deposit: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
                {
                    // Find the selected account.
                    var selectedAccount = user.Accounts.SingleOrDefault(account => account.Id == selectedAccountId);

                    if (selectedAccount != null)
                    {
                        // Updating the balance with the deposit.
                        selectedAccount.Balance += amount;

                        try
                        {
                            context.SaveChanges();
                            // Displaying the deposit details and updated balance.
                            Console.WriteLine($"You have deposited {amount:C} into {selectedAccount.AccountName}.");
                            Console.WriteLine($"Your new balance for {selectedAccount.AccountName} is: {selectedAccount.Balance:C}");
                            Console.Write("Press enter to continue.");
                            Console.ReadLine();

                        }
                        catch (Exception e)
                        {
                            // Handling any error that might occur during saving.
                            Console.WriteLine($"Error saving changes to the database");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid account number. Deposit canceled.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input for account number. Deposit canceled.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input for deposit amount. Deposit canceled.");
            }

        }

     
        // Method for creating new accounts.
        public static void CreateNewAccount(BankContext context, string username)
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

                Thread.Sleep(1000);
            }

            else
            {
                Console.Clear();
                Console.WriteLine("Account name cannot be empty");

                Thread.Sleep(1000);
            }
            
        }


        // Method for transferring money internally between 2 accounts belonging to the same user.
        public static void TransferInternal(BankContext context, string username)
        {

            User user = context.Users
            .Include(u => u.Accounts)
            .FirstOrDefault(u => u.UserName == username);

            Console.Clear();
            Console.Write("Transfer to which account? \n");
            string accountRecieving = Console.ReadLine();

            Console.Write("Transfer from which account? \n");
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
                        Console.WriteLine($"An error occurred: {ex.Message}");
                        Console.WriteLine("Please enter a valid amount, numbers only.");
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
                    Console.WriteLine("Transaction failed. Not enough funds in the account.");
                    Thread.Sleep(1000);
                }
            }

            // Error message if the user enters an account name that doesn't match any existing account.
            else
            {
                Console.Clear();
                Console.WriteLine("Account names do not match. Transaction failed.");
                Thread.Sleep(1000);
            }
        }
    }
}


