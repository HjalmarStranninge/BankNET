using BankNET.Data;
using BankNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace BankNET.Utilities
{
    // Class containing methods that print the different menus.
    internal static class MainMenus
    {
        // Method for user main menu after successful login
        internal static void UserMainMenu(BankContext context, string username)
        { 
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1. See your accounts and balance" +
                "\n2. Transfer between accounts" +
                "\n3. Withdrawal" +
                "\n4. Deposit" +
                "\n5. Open new account" +
                "\n6. Log out");
                Console.Write("What would you like to do: ");
                int userMenuChoice = int.Parse(Console.ReadLine());

                switch (userMenuChoice)
                {
                    case 1:
                        UserFunctions.ViewAccountBalance(context, username);
                        break;
                    case 2:
                        UserFunctions.TransferInternal(context, username); 
                        break;
                    case 3:
                        UserFunctions.Withdraw(context, username);
                        break;
                    case 4:
                        UserFunctions.Deposit(context, username);
                        break;
                    case 5:
                        UserFunctions.CreateNewAccount(context, username); 
                        break;
                    case 6:
                        LogInLogOut.LogOut();
                        return;
                    default:
                        InvalidInputHandling.InvalidInput();
                        break;
                }
            }
        }

        //Method for adminstrator menu after successful log in
        internal static void AdminMenu(BankContext context)
        {          
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1. View all users" +
                "\n2. Create user" +
                "\n3. Log out");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        AdminFunctions.ViewUsers(context);
                        break;
                    case "2":
                        AdminFunctions.CreateUser(context);
                        break;
                    case "3":
                        LogInLogOut.LogOut();
                        return;
                    default:
                        InvalidInputHandling.InvalidInput();
                        break;
                }
            }
        }

    }
}
