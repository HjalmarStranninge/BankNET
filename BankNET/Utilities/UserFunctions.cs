using BankNET.Data;
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
        // Add method for viewing all user accounts with balance.

        // Add method for moving money between users internal accounts. - h

        // Add methods for deposit/withdraw.

        // Add method for transactions between separate users accounts. (Extra)

        // Method for creating new accounts.
        public static void CreateNewAccount(BankContext context, User user)
        {
            
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
        public static void TransferInternal(User user)
        {
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
                    using(var context = new BankContext())
                    {
                        DbHelpers.TransferInternal(context, sendingAccount, recievingAccount, transferAmount);
                    }
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
