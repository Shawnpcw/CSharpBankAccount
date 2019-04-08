using Microsoft.EntityFrameworkCore;
 
namespace BankAccount.Models
{
    public class bankContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public bankContext(DbContextOptions<bankContext> options) : base(options) { }
            public DbSet<User> users {get;set;}
            public DbSet<Transaction> transactions {get;set;}
     
        
    }
}
