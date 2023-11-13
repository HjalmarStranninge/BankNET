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
                User user = context.Users
                    .Where(u => u.Name == userName)
                    .Include(u => u.Accounts)
                    .Single();

                foreach (var account in user.Accounts)
                {
                    Console.WriteLine($"{account.Name}: {account.Balance}");
                }
            }
            Console.WriteLine();

        }
            // Add method for moving money between users internal accounts.


            // Add methods for deposit/withdraw.
        internal static void Withdraw(BankContext context, string userName)
        {
            User user = context.Users
                .Include(u => u.Accounts)
                .Single(u => u.Name == userName);

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
                    var selectedAccount = user.Accounts.SingleOrDefault(account => account.Id == selectedAccountId);

                    if (selectedAccount != null)
                    {
                        if (amount <= selectedAccount.Balance)
                        {
                            selectedAccount.Balance -= amount;

                            try
                            {
                                context.SaveChanges();
                                Console.WriteLine("Changes saved to the database.");

                                Console.WriteLine($"You have withdrawn {amount:C} from {selectedAccount.Name}.");
                                Console.WriteLine($"Your new balance for {selectedAccount.Name} is: {selectedAccount.Balance:C}");
                            }
                            catch (Exception e)
                            {
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

        internal static void Deposit(BankContext context, string userName)
        {
            User user = context.Users
            .Include(u => u.Accounts)
            .Single(u => u.Name == userName);

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
                    var selectedAccount = user.Accounts.SingleOrDefault(account => account.Id == selectedAccountId);

                    if (selectedAccount != null)
                    {
                        selectedAccount.Balance += amount;

                        try
                        {
                            context.SaveChanges();
                            Console.WriteLine("Changes saved to the database.");

                            Console.WriteLine($"You have deposited {amount:C} into {selectedAccount.Name}.");
                            Console.WriteLine($"Your new balance for {selectedAccount.Name} is: {selectedAccount.Balance:C}");
                        }
                        catch (Exception e)
                        {
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
        //public static void Withdraw() // which user that is logged in between the ()
        //{
        //    using(BankContext context = new BankContext())
        //    {
        //        Console.Clear();
        //        Console.WriteLine(); // Which account the user would like to withdraw from
        //        Console.WriteLine("How much would you like to withdraw: ");
        //        try
        //        {
        //            decimal withdrawal = decimal.Parse(Console.ReadLine());
        //            if (withdrawal > 0 && withdrawal <= ) // The current user and the choosen account, ex user.SelectedAccount.Balance
        //            {
        //                //The user and account - the withdrawal ex, User.SelectedAccount.Balance -= withdrawal
        //                context.SaveChanges();

        //                Console.WriteLine("You have withdrawn: $" + withdrawal);
        //                Console.WriteLine("Your current balance is: $" ); // ex, user.SelectedAccount.Balance
        //            }
        //            else
        //            {
        //                Console.WriteLine("Insufficient balance. ");
        //            }
        //        }
        //        catch
        //        {
        //            Console.WriteLine("Invalid input. Please enter a valid amount.");
        //        }
        //    }
        //}


        // Add method for creating new account

        // Add method for transactions between separate users accounts. (Extra)


    }
}


