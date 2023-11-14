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
            catch
            {
                Console.WriteLine($"Error adding user: {user}");
                return false;
            }
            return true;
        }

        


        // Method for saving new accounts to the database.
        public static void CreateNewAccount(BankContext context, string accountName, string accountNumber, User user)
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
        public static void TransferInternal(BankContext context, Account accountSending, Account accountReceiving, decimal ammountToTransfer)
        {
            accountSending.Balance -= ammountToTransfer;
            accountReceiving.Balance += ammountToTransfer;
            context.SaveChanges();

            Console.Clear();
            Console.WriteLine($"Transfer successful! Updated account balances: \n" +
                $"{accountSending.AccountName}: {accountSending.Balance}\n"+
                $"{accountReceiving.AccountName}: {accountReceiving.Balance}");

            Thread.Sleep(1000);
        }

    }
}