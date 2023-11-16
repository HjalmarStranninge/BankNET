using BankNET.Data;
using BankNET.Models;
using BankNET.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;



namespace BankNET.Utilities
{
    // Static class containing all functions available to the admin. 
    internal static class AdminFunctions
    {

        //Method for creating user
        internal static void CreateUser(BankContext context)
        {
            string newUsername;
            while (true)
            {
                List<User> users = DbHelpers.GetAllUsers(context);

                Console.WriteLine("Create user");
                Console.WriteLine("Enter user name:");
                newUsername = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(newUsername))
                {
                    Console.Clear();
                    Console.WriteLine("Cannot be blank");
                }
                else if (users.Any(user => user.UserName == newUsername))
                {
                    Console.WriteLine("Username already exists. Please try again");
                }
                else
                {
                    Console.WriteLine();
                    break;

                }
            }

            Random random = new Random();
            string pin = random.Next(0, 10000).ToString();
            while (pin.Length < 4)
            {
                pin = "0" + pin;
            }

            User newUser = new User()
            {
                UserName = newUsername,
                Pin = pin
            };

            bool success = DbHelpers.AddUser(context, newUser);
            if (success)
            {
                Console.WriteLine($"Created user {newUsername} with pin {pin}");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine($"Failed to create user with username {newUsername}");
                Console.ReadLine();
            }
        }

        //Method for viewing list of all users.
        internal static void ViewUsers(BankContext context, string username)
        {
            while (true)
            {
                Console.Clear();
                List<User> users = DbHelpers.GetAllUsers(context);
                Console.WriteLine($"Total number of users in system: {users.Count()}");
                Console.WriteLine("---");
                Console.WriteLine("Current users in system:");

                foreach (User user in users)
                {
                    Console.WriteLine($"{user.UserName}");
                }
                Console.WriteLine("---");

                SelectUser(context, username);
                break;
            }
        }

        //Selects user and shows accounts connected. Leads into Admin to user submenu
        internal static void SelectUser(BankContext context, string username)
        {
            while (true)
            {
                Console.Write("Insert name to view specific user or type 'e' to exit: ");
                string userSelect = Console.ReadLine();
                Console.WriteLine();

                var userSelection = context.Users
                    .Where(u => u.UserName == userSelect)
                    .Select(u => new
                    {
                        u.Id,
                        u.UserName,
                        u.Pin,
                        AccountCount = u.Accounts.Count(),
                        Accounts = u.Accounts.Select(a => new { a.AccountName, a.AccountNumber, a.Balance }),
                        TotalBalance = u.Accounts.Sum(a => a.Balance)
                    })
                    .ToList();

                if (userSelection.Any())
                {
                    foreach (var u in userSelection)
                    {
                        Console.WriteLine(
                            $"Name: {u.UserName}," +
                            $"\tPIN: ****" +
                            $"\r\nAccounts: {u.AccountCount} \tTotal sum: {u.TotalBalance},"
                            );
                    }

                    var userAccounts = context.Users
                    .Where(u => u.UserName == userSelect)
                    .Include(u => u.Accounts)
                    .Single()
                    .Accounts
                    .ToList();
                    foreach (var ua in userAccounts)
                    {
                        Console.WriteLine($"AccountNumber: {ua.AccountNumber}\tAccountName: {ua.AccountName}\tBalance: {ua.Balance} missing currency");
                    }

                    Console.WriteLine();

                    AdminUserView(context, userSelect, username, userAccounts);
                }
                else if (userSelect == "e")
                {
                    return;
                }
                else
                {
                    Console.WriteLine("user cannot be found. please try again");
                    break;
                }
            }
        }
        // admin to user submenu. Brings along selected username and admin username.
        internal static void AdminUserView(BankContext context, string userSelect, string username, List<Account> userAccounts)
        {
            while (true)
            {
                Console.WriteLine("1. Show pin");
                Console.WriteLine("2. Delete user");
                Console.WriteLine("e. go back");
                Console.Write("Select option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        ShowPin(context, userSelect, username, userAccounts);
                        break;
                    case "2":
                        DeleteUser(context, userSelect, username, userAccounts);
                        return;
                    case "e":
                        return;
                    default:
                        Console.WriteLine("Unknown command. Please try again");
                        break;
                }
                
            }
        }
        // method for showing pin by confirming admin pin.
        private static void ShowPin(BankContext context, string userSelect, string adminName, List<Account> userAccounts)
        {
            if (BankHelpers.SimplePinCheck(context, "Please enter Admin PIN to view user PIN: ", adminName))
            {
                var user = context.Users
                    .FirstOrDefault(u => u.UserName == userSelect);
                if (user != null)
                {
                    Console.WriteLine($"Name: {user.UserName}," +
                                      $"\tPIN: {user.Pin}");

                    foreach (var ua in userAccounts)
                    {
                        Console.WriteLine($"AccountNumber: {ua.AccountNumber}\tAccountName: {ua.AccountName}\tBalance: {ua.Balance} missing currency");
                    }

                    Console.WriteLine();
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("wrong PIN.");
                }

            }
        }
        //method for deleting user by confirming admin pin. Will only go through if accounts are zero
        private static void DeleteUser(BankContext context, string userSelect, string username, List<Account> userAccounts)
        {
            if (BankHelpers.SimplePinCheck(context, "Confirm user deletion with Admin Pin: ", username))
            {
                if (BankHelpers.CheckAccountBalanceZero(userAccounts))
                {
                    var userToDelete = context.Users.FirstOrDefault(u => u.UserName == userSelect);

                    if (userToDelete != null)
                    {
                        bool success = DbHelpers.DeleteUser(context, userToDelete);
                        if (success)
                        {
                            Console.WriteLine($"Deleted user {username} and any connected accounts");
                        }
                        else
                        {
                            Console.WriteLine($"Failed to delete user with username {username}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Cannot delete user {username}. Some accounts have a remaining balance.");
                    }
                }
            }
        }


        //* should we allow the function of deleting a user? OK
        //* should we allow function of reseting customer pin? OK
        //* adding function to check if user is logging in for first time, forcing them to change pin?
        //* adding function for user to change pin, but not to a pin last used / used within the last 6 months?
        //* should we be able to view all info on a specific user incl. transaction history? OK

    }
}

