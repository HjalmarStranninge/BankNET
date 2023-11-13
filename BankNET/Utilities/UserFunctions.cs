using BankNET.Data;
using BankNET.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankNET.Utilities
{
    // Static class containing methods for all functions available to regular users.
    internal static class UserFunctions
    {
        internal static void ViewAccountBalance(BankContext context, string userName)
        {
            {
                // Retrive user information from database
                User user = context.Users
                    .Where(u => u.Name == userName)
                    .Include(u => u.Accounts)
                    .Single();
                // Dusplaying the account and balance for the user
                foreach (var account in user.Accounts)
                {
                    Console.WriteLine($"{account.Name}: {account.Balance}");
                }
            }
            // Empty line for better output fromatting
            Console.WriteLine();

        }
            // Add method for moving money between users internal accounts.
 
        internal static void Withdraw(BankContext context, string userName)
        {
            // Retrive user info from database
            User user = context.Users
                .Include(u => u.Accounts)
                .Single(u => u.Name == userName);

            // Displaying the user´s accounts
            Console.WriteLine("Select the account you would like to withdraw from: ");
            foreach (var account in user.Accounts)
            {
                Console.WriteLine($"{account.Id}. {account.Name}: {account.Balance}");
            }

            Console.WriteLine("How much would you like to withdraw: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
            {
                Console.WriteLine("Select the account by entering the account number: ");
                if (int.TryParse(Console.ReadLine(), out int selectedAccountId))
                {
                    // Find the selected account
                    var selectedAccount = user.Accounts.SingleOrDefault(account => account.Id == selectedAccountId);

                    if (selectedAccount != null)
                    {
                        // Check if there is sufficient balance
                        if (amount <= selectedAccount.Balance)
                        {
                            // Update the account balance with the withdrawal
                            selectedAccount.Balance -= amount;

                            try
                            {
                                context.SaveChanges();

                                // Displaying the withdraw details and the updated balance
                                Console.WriteLine($"You have withdrawn {amount:C} from {selectedAccount.Name}.");
                                Console.WriteLine($"Your new balance for {selectedAccount.Name} is: {selectedAccount.Balance:C}");
                            }
                            catch (Exception e)
                            {
                                // Handling any error that occur during saving
                                Console.WriteLine($"Error saving changes to the database");
                            }
                        }
                        else
                        {
                            // Display message if their isn´t enough money
                            Console.WriteLine("Insufficient balance. Withdrawal canceled.");
                        }
                    }
                    else
                    {
                        // Invalid account number
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
                // Invalid amount
                Console.WriteLine("Invalid input for withdrawal amount. Withdrawal canceled.");
            }
        }

        internal static void Deposit(BankContext context, string userName)
        {
            // Retrive user info from database
            User user = context.Users
            .Include(u => u.Accounts)
            .Single(u => u.Name == userName);

            //Displaying the user´s account
            Console.WriteLine("Select the account you would like to deposit into: ");
            foreach (var account in user.Accounts)
            {
                Console.WriteLine($"{account.Id}. {account.Name}: {account.Balance}");
            }

            Console.WriteLine("How much would you like to deposit: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
            {
                Console.WriteLine("Select the account by entering the account number: ");
                if (int.TryParse(Console.ReadLine(), out int selectedAccountId))
                {
                    // Find the selected account
                    var selectedAccount = user.Accounts.SingleOrDefault(account => account.Id == selectedAccountId);

                    if (selectedAccount != null)
                    {
                        // Updating the balance with the deposit
                        selectedAccount.Balance += amount;

                        try
                        {
                            context.SaveChanges();
                            //Desplaying the deposit details and updated balance
                            Console.WriteLine($"You have deposited {amount:C} into {selectedAccount.Name}.");
                            Console.WriteLine($"Your new balance for {selectedAccount.Name} is: {selectedAccount.Balance:C}");
                        }
                        catch (Exception e)
                        {
                            // Handling any error that occur during saving
                            Console.WriteLine($"Error saving changes to the database");
                        }
                    }
                    else
                    {
                        // Invalid account number
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
                // Invalid amount
                Console.WriteLine("Invalid input for deposit amount. Deposit canceled.");
            }

        }

        // Add method for creating new account

        // Add method for transactions between separate users accounts. (Extra)


    }
}


