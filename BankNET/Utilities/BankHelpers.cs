﻿using BankNET.Data;
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
        internal static bool IsThereBalance(Account account, decimal proposedAmount, string senderUsername)
        {
            return account.Balance >= proposedAmount;
        }

        // Generates a radom pin and returns new pin
        internal static string GeneratePin()
        {
            Random random = new Random();
            string pin = random.Next(0, 10000).ToString();
            while (pin.Length < 4)
            {
                pin = "0" + pin;
            }

            return pin;
        }

        // Checks pin of user
        internal static bool PinCheck(BankContext context, string username)
        {
            try
            {
                string pincheck = MenuUI.EnterPinHidden();

                bool validPin = context.Users.Any(p => p.UserName.Equals(username) && p.Pin.Equals(pincheck));
                return validPin;
            }
            catch  
            {
                Console.WriteLine();
                return false;
            }
            
        }

        // Check if any account has a balance greater than zero
        internal static bool CheckAccountBalanceZero(List<Account> userAccounts)
        {
            return userAccounts.All(account => account.Balance == 0);
        }
    }
}
