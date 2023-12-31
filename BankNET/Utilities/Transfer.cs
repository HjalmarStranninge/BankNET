﻿using BankNET.Data;
using BankNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace BankNET.Utilities
{
    // Class for transfer money options
    internal static class Transfer
    {
        internal static int numberOfTries = 1;
        // Method for internal transfers, ie transfers within one user's accounts
        internal static void TransferInternal(BankContext context, string username)
        {
            User? user = context.Users
                .Where(u => u.UserName == username)
                .Include(u => u.Accounts)
                .SingleOrDefault();

            int selectedOption = 0;
            
            Account sendingAccount = new Account();
            Account receivingAccount = new Account();

            // Creates a list of accounts and adds all accounts of the current user to it.
            List<Account> menuOptions = new List<Account>();

            foreach (var account in user.Accounts)
            {
                menuOptions.Add(account);
            }

            ConsoleKeyInfo key;

            // Displays the accounts available and lets the user choose account using arrow keys.
            do
            {
                MenuUI.ClearAndPrintFooter();
                Console.WriteLine($"   Which account would you like to transfer from?\n");

                for (int i = 0; i < menuOptions.Count; i++)
                {
                    if (i == selectedOption)
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Black;

                        Console.WriteLine($"{menuOptions[i].AccountNumber} {menuOptions[i].AccountName}\nBalance: {menuOptions[i].Balance,2} SEK");
                        Console.ResetColor();
                    }

                    else
                    {
                        Console.WriteLine($"{menuOptions[i].AccountNumber} {menuOptions[i].AccountName}\nBalance: {menuOptions[i].Balance,2} SEK");
                    }
                }
                
                key = Console.ReadKey();
                Console.Beep();

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:

                        if (selectedOption > 0)
                        {
                            selectedOption = (selectedOption - 1 + menuOptions.Count) % menuOptions.Count;
                        }
                        break;

                    case ConsoleKey.DownArrow:

                        if (selectedOption < menuOptions.Count - 1)
                        {
                            selectedOption = (selectedOption + 1) % menuOptions.Count;
                        }

                        break;

                }
            } while (key.Key != ConsoleKey.Enter);

            // Save chosen account.
            if (key.Key == ConsoleKey.Enter)
            {
                sendingAccount = menuOptions[selectedOption];               
            }

            do
            {
                MenuUI.ClearAndPrintFooter();
                Console.WriteLine($"   Which account would you like to transfer to?\n");

                for (int i = 0; i < menuOptions.Count; i++)
                {
                    if (i == selectedOption)
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Black;

                        Console.WriteLine($"{menuOptions[i].AccountNumber} {menuOptions[i].AccountName}\nBalance: {menuOptions[i].Balance,2} SEK");
                        Console.ResetColor();
                    }

                    else
                    {
                        Console.WriteLine($"{menuOptions[i].AccountNumber} {menuOptions[i].AccountName}\nBalance: {menuOptions[i].Balance,2} SEK");
                    }
                }
              
                key = Console.ReadKey();
                Console.Beep();

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:

                        if (selectedOption > 0)
                        {
                            selectedOption = (selectedOption - 1 + menuOptions.Count) % menuOptions.Count;
                        }
                        break;

                    case ConsoleKey.DownArrow:

                        if (selectedOption < menuOptions.Count - 1)
                        {
                            selectedOption = (selectedOption + 1) % menuOptions.Count;
                        }

                        break;

                }
            } while (key.Key != ConsoleKey.Enter);

            // Save chosen account.
            if (key.Key == ConsoleKey.Enter)
            {
                receivingAccount = menuOptions[selectedOption];
            }

            if (receivingAccount.Id == sendingAccount.Id)
            {
                MenuUI.ClearAndPrintFooter();
                Console.WriteLine("\nError: Cannot transfer money between the same account");
                Thread.Sleep(2000);
            }
            else
            {
                decimal transferAmount = 0;
                bool validInput = false;

                // Keeps the user stuck in a loop until they enter a valid numeric amount.
                do
                {
                    MenuUI.ClearAndPrintFooter();

                    Console.Write("\n\tEnter amount you wish to transfer: ");
                    try
                    {
                        Console.CursorVisible = true;
                        transferAmount = decimal.Parse(Console.ReadLine());
                        validInput = true;
                        Console.CursorVisible = false;
                    }
                    catch (Exception ex)
                    {
                        MenuUI.ClearAndPrintFooter();
                        InvalidInputHandling.InvalidInputAmount();
                    }

                } while (!validInput);

                if (BankHelpers.IsThereBalance(sendingAccount, transferAmount, username))
                {
                    sendingAccount.Balance -= transferAmount;
                    receivingAccount.Balance += transferAmount;
                    context.SaveChanges();

                    Sounds.PlaySuccessSound();
                    MenuUI.ClearAndPrintFooter();

                    Console.WriteLine($"Transfer successful! Updated account balances: \n" +
                        $"{sendingAccount.AccountName}: {sendingAccount.Balance,2} SEK\n" +
                        $"{receivingAccount.AccountName}: {receivingAccount.Balance,2} SEK");
                    
                    Thread.Sleep(1000);
                    Console.WriteLine("\n\t\tPress ENTER to continue");

                    Console.ReadLine();
                    Console.Beep();
                }

                // Error message if there isn't enough funds.
                else
                {
                    MenuUI.ClearAndPrintFooter();
                    Console.WriteLine("\n\t  Transaction failed. Not enough funds in the account.");

                    Thread.Sleep(2000);
                }
            }
        }

        // Method for external transfers, ie transfers between two users' accounts
        internal static void TransferExternal(BankContext context, string senderUsername)
        {
            User? user = context.Users
                .Where(u => u.UserName == senderUsername)
                .Include(u => u.Accounts)
                .SingleOrDefault();

            int selectedOption = 0;

            // List of all bankaccounts in the database.
            List<Account> allAccounts = context.Accounts.ToList();

            Account sendingAccount = new Account();
            Account receivingAccount = new Account();

            // Creates a list of accounts and adds all accounts of the current user to it.
            List<Account> menuOptions = new List<Account>();

            foreach (var account in user.Accounts)
            {
                menuOptions.Add(account);
            }

            ConsoleKeyInfo key;

            // Displays the accounts available and lets the user choose account using arrow keys.
            do
            {
                MenuUI.ClearAndPrintFooter();
                Console.WriteLine($"   Which account would you like to transfer from?\n");

                for (int i = 0; i < menuOptions.Count; i++)
                {
                    if (i == selectedOption)
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Black;

                        Console.WriteLine($"{menuOptions[i].AccountNumber} {menuOptions[i].AccountName}\nBalance: {menuOptions[i].Balance,2} SEK");
                        Console.ResetColor();
                    }

                    else
                    {
                        Console.WriteLine($"{menuOptions[i].AccountNumber} {menuOptions[i].AccountName}\nBalance: {menuOptions[i].Balance,2} SEK");
                    }
                }
               
                key = Console.ReadKey();
                Console.Beep();

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:

                        if (selectedOption > 0)
                        {
                            selectedOption = (selectedOption - 1 + menuOptions.Count) % menuOptions.Count;
                        }
                        break;

                    case ConsoleKey.DownArrow:

                        if (selectedOption < menuOptions.Count - 1)
                        {
                            selectedOption = (selectedOption + 1) % menuOptions.Count;
                        }

                        break;

                }
            } while (key.Key != ConsoleKey.Enter);

            // Save chosen account.
            if (key.Key == ConsoleKey.Enter)
            {
                sendingAccount = menuOptions[selectedOption];
            }

            MenuUI.ClearAndPrintFooter();
            Console.WriteLine($"\n  Enter recipient account number. Format XXXX-XXXX");
            Console.Write("\n\t  Receiving account: ");

            bool accountFound = false;
            
            Console.CursorVisible = true;

            string recipient = Console.ReadLine();
            Console.Beep();

            Console.CursorVisible = false;

            foreach (var account in allAccounts)
            {
                if (account.AccountNumber == recipient)
                {
                    receivingAccount = account;
                    accountFound = true;
                }
            }

            if (accountFound)
            {
                decimal transferAmount = 0;
                bool validInput = false;

                // Keeps the user stuck in a loop until they enter a valid numeric amount.
                do
                {
                    MenuUI.ClearAndPrintFooter();

                    Console.Write("\n\tEnter amount you wish to transfer: ");
                    try
                    {
                        Console.CursorVisible = true;

                        transferAmount = decimal.Parse(Console.ReadLine());
                        Console.Beep();

                        validInput = true;
                        Console.CursorVisible = false;
                    }
                    catch (Exception ex)
                    {
                        MenuUI.ClearAndPrintFooter();
                        InvalidInputHandling.InvalidInputAmount();
                    }

                } while (!validInput);

                if (BankHelpers.IsThereBalance(sendingAccount, transferAmount, senderUsername))
                {
                    MenuUI.ClearAndPrintFooter();

                    // User PIN confirmation needed after accepted transferAmount + sendingAccount found
                    Console.Write("\n\tPlease enter PIN to confirm: ");
                    if (BankHelpers.PinCheck(context, senderUsername))
                    {
                        sendingAccount.Balance -= transferAmount;
                        receivingAccount.Balance += transferAmount;
                        context.SaveChanges();

                        MenuUI.ClearAndPrintFooter();
                        Sounds.PlaySuccessSound();
                        Console.WriteLine($"\n\t\tTransaction successful!");
                        Console.WriteLine($"\n\tAmount: {transferAmount:F2} SEK");
                        Console.WriteLine($"\tSent from: {sendingAccount.AccountName} {sendingAccount.AccountNumber}");
                        Console.WriteLine($"\tRecipient: {receivingAccount.AccountName} {receivingAccount.AccountNumber}");
                        
                        Thread.Sleep(1000);
                        Console.WriteLine("\n\t\tPress ENTER to continue");

                        Console.ReadLine();
                        Console.Beep();
                    }
                    else
                    {
                        MenuUI.ClearAndPrintFooter();
                        InvalidInputHandling.AttemptsTracker(senderUsername, numberOfTries);
                        Console.Beep();
                        Thread.Sleep(1000);
                    }
                }
                // Error message if there isn't enough balance.
                else
                {
                    MenuUI.ClearAndPrintFooter();
                    Console.WriteLine("\n\t  Transaction failed. Not enough funds in the account.");

                    Thread.Sleep(2000);
                }
            }
            else
            {
                MenuUI.ClearAndPrintFooter();
                Console.WriteLine("\n\t  Transaction failed. Invalid account number.");

                Thread.Sleep(2000);
            }
        }
    }
}
