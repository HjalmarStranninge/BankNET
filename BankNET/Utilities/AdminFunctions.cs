using BankNET.Data;
using BankNET.Models;
using BankNET.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
            }
            else
            {
                Console.WriteLine($"Failed to create user with username {newUsername}");
            }
        }

        //Method for viewing list of all users.
        internal static void ViewUsers(BankContext context)
        {
            Console.Clear();
            List<User> users = DbHelpers.GetAllUsers(context);
            Console.WriteLine($"Total number of users in system: {users.Count()}");
            Console.WriteLine("Current users in system:");

            foreach (User user in users)
            {
                Console.WriteLine($"{user.UserName}");
            }
            while (true)
            {
                Console.WriteLine("---");
                Console.WriteLine("1. View user");
                Console.WriteLine("e. Go back");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        SelectUser(context);
                        return;
                    case "e":
                        Console.Clear();
                        return;
                    default:
                        //Console.Clear();
                        //Console.WriteLine("Current users in system:");

                        //foreach (User user in users)
                        //{
                        //    Console.WriteLine($"{user.UserName}");
                        //}
                        Console.WriteLine("Unknown command");
                        break;

                }
            }
        }
    

        //Selects user and shows accounts connected
        internal static void SelectUser(BankContext context)
        {
            Console.WriteLine("Insert name: ");
            string userSelect = Console.ReadLine();

            var userSelection = context.Users
                .Where(u => u.UserName == userSelect)
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.Pin,
                    AccountCount = u.Accounts.Count(),
                    Accounts = u.Accounts.Select(a => new { a.AccountName, a.AccountNumber, a.Balance })
                })
                .ToList();

                foreach (var u in userSelection)
                {
                    Console.WriteLine(
                        $"Name: {u.UserName}," +
                        $"\tPIN: xxxx" +
                        $"\tAccounts: {u.AccountCount}," 
                        );
                }
                var UserAccounts = context.Users
                    .Where (u => u.UserName == userSelect)
                    .Include (u=> u.Accounts)
                    .Single()
                    .Accounts
                    .ToList();
                foreach (var ua in UserAccounts)
                {
                    Console.WriteLine($"AccountNumber: {ua.AccountNumber}\tAccountName: {ua.AccountName}\tBalance: {ua.Balance} missing currency");
                }
            Console.WriteLine();
            AdminUserView(context);
        }
        
        internal static void AdminUserView(BankContext context)
        {
            while (true)
            {
                Console.WriteLine("1. Show pin");
                Console.WriteLine("2. Delete user");
                Console.WriteLine("e. go back");
                Console.Write(": ");

                switch (Console.ReadLine())
                {
                    case "1":
                        ChangePin(context);
                        return;
                    case "2":
                        DeleteUser(context);
                        return;
                    case "e":
                        return;
                    default:
                        Console.WriteLine("Unknown command. Please try again");
                        break;
                }
            }
        }

        internal static void AdminPinCheck(BankContext context, string adminuser, string pincheck)
        {
            Console.Write("To view User PIN, enter Admin PIN:");
            pincheck = Console.ReadLine();

            var admin = context.Users
                .Where(u => u.UserName == "admin");
            if ()
            {
                if (admin.Pin == pincheck)
            }
        }
        internal static void DeleteUser(BankContext context)
        {

            Console.Write("Confirm user deletion with Admin PIN: ");
            //while (true)
            //{
            //    bool tryPin = true;
            //    string pin = Console.ReadLine();

            //    bool validPin = context.Users.Any(p => p.Pin.Equals(pin));

            //    while (tryPin)
            //    {
            //        if (validPin)
            //        {
            //            Console.WriteLine("Login Successful!");
            //            tryPin = false;

            //            if (username == "admin") AdminMenu(context);
            //        }
            //    }
            //}

        }

        private static void ChangePin(BankContext context)
        {
            
            

            AdminPinCheck(context);

        }

        //* should we allow the function of deleting a user? OK
        //* should we allow function of reseting customer pin? OK
        //* adding function to check if user is logging in for first time, forcing them to change pin?
        //* adding function for user to change pin, but not to a pin last used / used within the last 6 months?
        //* should we be able to view all info on a specific user incl. transaction history? OK
        
    }
}
