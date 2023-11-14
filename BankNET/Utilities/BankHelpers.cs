using BankNET.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankNET.Models;

namespace BankNET.Utilities
{
    // Class containing methods helpful for bank management.
    internal static class BankHelpers
    {

        // Method for generating new account numbers.
        public static string GenerateAccountNumber()
        {
            var random = new Random();

            // Generates a random 8-digit number.
            string newAccountNumber = random.Next(10000000, 99999999).ToString();

            // Inserts a '-' in the middle so the format becomes XXXX-XXXX.
            int middleIndex = newAccountNumber.Length / 2;

            newAccountNumber = newAccountNumber.Insert(middleIndex, "-");

            return newAccountNumber;
        }

        // Method for checking if a new account number is unique, if it is, it returns true.
        public static bool IsAccountNumberUnique(string accountNumber)
        {
            using (var context = new BankContext())
            {
                return !context.Accounts.Any(a => a.AccountNumber == accountNumber);
            }
        }

        // Iterates through all the account names of a specific user and returns true if it matches the account your searching for.
        public static bool IsAccountNameMatching(string accountName, User user)
        {          
            foreach (Account account in user.Accounts)
            {
                if (account.AccountName.ToLower() == accountName.ToLower())
                {
                    return true;
                }
            }

            return false;            
        }

        // Checks if an amount is bigger or smaller than the balance of an account.
        public static bool IsThereBalance(Account account, decimal proposedAmount)
        {
            return account.Balance >= proposedAmount;
        }
    }
}
