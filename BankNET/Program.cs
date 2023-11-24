using BankNET.Utilities;
using System.Net.Http.Headers;
using BankNET.Data;
using BankNET.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;


namespace BankNET
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RunBankNET.Start();
        }
    }
}