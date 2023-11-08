using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BankNET.Models
{
    internal class Account
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string AccountName { get; set; }

        public decimal Balance { get; set; }

        public virtual User User { get; set; }
    }
}
