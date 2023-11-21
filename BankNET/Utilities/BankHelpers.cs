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
        internal static string GenerateAccountNumber()
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
        internal static bool IsAccountNumberUnique(string accountNumber)
        {
            using (var context = new BankContext())
            {
                return !context.Accounts.Any(a => a.AccountNumber == accountNumber);
            }
        }

        // Checks if an amount is bigger or smaller than the balance of an account.
        public static bool IsThereBalance(Account account, decimal proposedAmount)
        {
            return account.Balance >= proposedAmount;
        }

        // Checks pin of user
        public static bool SimplePinCheck(BankContext context, string username)
        {
            string pincheck = MenuUI.EnterPinHidden();

            bool validPin = context.Users.Any(p => p.UserName.Equals(username) && p.Pin.Equals(pincheck));
            if (validPin)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Check if any account has a balance greater than zero
        public static bool CheckAccountBalanceZero(List<Account> userAccounts)
        {
            return userAccounts.All(account => account.Balance == 0);
        }
    }
}
