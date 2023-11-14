using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankNET.Models;
using Microsoft.EntityFrameworkCore;

namespace BankNET.Data
{
    internal class BankContext : DbContext
    {
        public DbSet <User> Users { get; set; }
        public DbSet <Account> Accounts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\.;Initial Catalog=BankNET;Integrated Security=True;Pooling=False");
        }       
    }
}
