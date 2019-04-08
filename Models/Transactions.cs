using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankAccount.Models
{
    public class Transaction
    {
        [Key]
        public int transactionid {get;set;}
        [Range((double)decimal.MinValue,(double)decimal.MaxValue)]
        public decimal amount {get;set;}

        public DateTime created_at {get;set;} = DateTime.Now;
        public int users_id {get;set;}
        [ForeignKey("users_id")]
        public User user {get;set;}       
    }
}