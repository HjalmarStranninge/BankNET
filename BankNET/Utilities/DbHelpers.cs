using BankNET.Data;
using BankNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankNET.Utilities
{
    // Static class containing methods for accessing the database in different ways.
    internal static class DbHelpers
    {
        // Returns a list of all users in the database.
        internal static List<User> GetAllUsers(BankContext context)
        {
            List<User> users = context.Users.ToList();
            return users;
        }

        // Add method for adding users.


        internal static bool AddUser (BankContext context, User user)
        {
            context.Users.Add(user);
            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding user: {user}. Error message: {ex.Message}");
                return false;
            }
            return true;
        }
        internal static bool DeleteUser(BankContext context, User user)
        {
            var userToDelete = context.Users
                .SingleOrDefault(u => u.UserName == user.UserName);

            if (userToDelete != null)
            {
                context.Users.Remove(userToDelete);

                try
                {
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error removing user: {user.UserName}. Error message: {ex.Message}");
                    return false;
                }
            }

            Console.WriteLine($"User {user.UserName} not found.");
            return false;
        }

        // Method for saving new accounts to the database.
        internal static void CreateNewAccount(BankContext context, string accountName, string accountNumber, User user)
        {
            Account newAccount = new Account
            {
                AccountName = accountName,
                AccountNumber = accountNumber,
                Balance = 0,
                User = user
            };
            
            context.Accounts.Add(newAccount);
            context.SaveChanges();
        }

        // Transfers money between two accounts belonging to the same users.
        internal static void TransferInternal(BankContext context, Account accountSending, Account accountReceiving, decimal ammountToTransfer)
        {
            accountSending.Balance -= ammountToTransfer;
            accountReceiving.Balance += ammountToTransfer;
            context.SaveChanges();



            //Sounds.PlaySuccessSound();

            MenuUI.ClearAndPrintFooter();

            Console.WriteLine($"Transfer successful! Updated account balances: \n" +
                $"{accountSending.AccountName}: {accountSending.Balance,2}\n"+
                $"{accountReceiving.AccountName}: {accountReceiving.Balance,2}");
            Thread.Sleep(3000);
        }

        internal static void TransferExternal(BankContext context, Account sendingAccount, Account receivingAccount, decimal transferAmount)
        {
            sendingAccount.Balance -= transferAmount;
            receivingAccount.Balance += transferAmount;
            context.SaveChanges();

            MenuUI.ClearAndPrintFooter();
            //Sounds.PlaySuccessSound();
            Console.WriteLine($"\n\t\tTransaction successful!");
            Console.WriteLine($"\n\tAmount: {transferAmount}");
            Console.WriteLine($"\tSent from: {sendingAccount.AccountName} {sendingAccount.AccountNumber}");
            Console.WriteLine($"\tRecipient: {receivingAccount.AccountName} {receivingAccount.AccountNumber}");
            Console.ReadLine();
        }
    }
}