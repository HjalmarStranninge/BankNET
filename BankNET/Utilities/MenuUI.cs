using BankNET.Data;
using BankNET.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BankNET.Utilities
{
    // Class that handles the UI elements of the application. 
    internal static class MenuUI
    {

        // Prints a header with logo.
        public static void PrintHeader()
        {
            Console.Write($"------------------- <> Bank");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("NET");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"\u2122 <> ------------------\n\n\n");
        }

        // Clears console and prints footer at the bottom before resetting cursor position.
        public static void ClearAndPrintFooter()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 10);
            Console.WriteLine("\n\n\n======================================================");
            Console.SetCursorPosition(0, 0);
            PrintHeader();

        }

        public static void OverwriteLine(string content)
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, currentLineCursor);
            Console.Write(new string(' ', Console.WindowWidth)); // Clear the line
            Console.SetCursorPosition(0, currentLineCursor);
            Console.Write(content);
        }

        // Lets you enter your pin code while hiding it, showing star symbols instead.
        public static string EnterPinHidden()
        {
            ConsoleKeyInfo keyInfo;
            string pin = "";
            do
            {
                // Read a key without displaying it.
                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key != ConsoleKey.Backspace && keyInfo.Key != ConsoleKey.Enter)
                {
                    // Display a star for each character.
                    pin += keyInfo.KeyChar;
                    Console.Write("*");
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && pin.Length > 0)
                {
                    // Clear the last character and move the cursor back.
                    pin = pin.Substring(0, pin.Length - 1);
                    Console.Write("\b \b");
                }
            }
            while (keyInfo.Key != ConsoleKey.Enter);

            return pin;
        }
    }
}
