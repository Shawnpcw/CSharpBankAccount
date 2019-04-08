using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
namespace BankAccount.Models
{
    public class User
    {
        [Key]
        public int userid {get;set;}

        [Required]
        [MinLength(2)]
        public string firstName {get;set;}
        [Required]
        [MinLength(2)]
        public string lastName {get;set;}

        [EmailAddress]
        [Required]
        [MinLength(2)]
        public string email { get; set; }
        [Required]
        [MinLength(2, ErrorMessage="Password must be 8 characters or longer!")]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [NotMapped]
        [Compare("password")]
        [DataType(DataType.Password)]
        public string passwordConfirm { get; set; }

        [NotMapped]
        [Range(0, Double.PositiveInfinity, ErrorMessage="Cannot withdraw more than in account")]
        public decimal Sum 
        { 
            get
            {
                if (Transactions!=null){

                return Transactions.Sum(t=>t.amount);
                }
                return 0;
                
            
            }
        } 
        
        
        public List<Transaction> Transactions {get;set;}
    }
    public class LoginUser
    {
        [Required]
        public string email {get; set;}
        [Required]
        public string password { get; set; }
    }
}