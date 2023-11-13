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


            // Add method for moving money between users internal accounts.


            // Add methods for deposit/withdraw.
            //public static void Deposit() // which user that is logged in in between the ()
            //{
            //    using(BankContext context = new BankContext())
            //    {
            //        Console.Clear();
            //        Console.WriteLine();  // Choose which account to deposit to.
            //        Console.WriteLine($"How much would you like to deposit: ");
            //        try
            //        {
            //            decimal deposit = decimal.Parse(Console.ReadLine());
            //            // accessing the account choosen and the balance, ex user.SelectedAccount.Balance += deposit
            //            context.SaveChanges();

            //            Console.WriteLine("You have deposited : $" + deposit ); // the deposit to the account
            //            Console.WriteLine("Your new balance is: $" ); // , ex user.SelectedAccount.Balance
            //        }
            //        catch
            //        {
            //            Console.WriteLine("Invalid input. PLease enter a valid amount.");
            //        }
            //    }
            //}

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


