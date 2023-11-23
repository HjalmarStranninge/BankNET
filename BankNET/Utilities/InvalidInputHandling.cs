using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankNET.Utilities
{
    // Class containing methods for handling invalid inputs
    internal class InvalidInputHandling
    {
        private static bool isLockedOut { get; set; }
        private static DateTime lockoutTime { get; set; }

        internal static void IncorrectLogin(int i)
        {
            MenuUI.ClearAndPrintFooter();
            if (isLockedOut && DateTime.Now < lockoutTime)
            {
                Console.WriteLine("Multiple incorrect tries have been made. Please try again in a couple of minutes");
                Thread.Sleep(2000);
                return;
            }
            else if (i == 0)
            {
                Console.WriteLine("\n\t    Invalid username and/or pin.");
                Console.WriteLine("\n\t     You have 2 attempts left.");
                Thread.Sleep(2000);
            }
            else if (i == 1)
            {
                Console.WriteLine("\n\t    Invalid username and/or pin.");
                Console.WriteLine("\n\t     You have 1 attempt left.");
                Thread.Sleep(2000);
            }
            else
            {
                LockOutUser();
                //Console.WriteLine("   Too many incorrect tries, program shutting down.");
                //Thread.Sleep(3000);
                //Environment.Exit(0);
            }
        }
        internal static void InvalidWithdrawal(string message)
        {
            Console.WriteLine($"\n{message}");
            Console.Write("Returning to main menu...");
            Thread.Sleep(2000);
        }
        internal static void LockOutUser()
        {
            isLockedOut = true;
            lockoutTime = DateTime.Now.AddMinutes(3);
            Console.WriteLine("Multiple incorrect tries have been made. Temporarily locking user out. Please try again in a couple of minutes");
            Thread.Sleep(2000);
        }
        //internal static bool GoBackShortcut(ConsoleKey key)
        //{
        //    while (true)
        //    {
        //        //if (Console.KeyAvailable)
        //        //{
        //        //    ConsoleKeyInfo pressedKey = Console.ReadKey(intercept: true);
        //        //    if (pressedKey.Key == key)
        //        //    {
        //        //        return Console.ReadKey(intercept: true).Key == key;
        //        //    }
        //        //}
        //        if (Console.KeyAvailable && Console.ReadKey(intercept: true).Key == key)
        //        {
        //            return true; // User pressed the specified key
        //        }
        //    }
            
        //}
    }
}
