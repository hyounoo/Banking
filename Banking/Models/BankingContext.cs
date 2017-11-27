using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Banking.Models
{
    public class BankingContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public BankingContext() : base("name=BankingContext")
        {
            Database.SetInitializer<BankingContext>(new DropCreateDatabaseIfModelChanges<BankingContext>());
            //  To trace the SQL, uncomment the following line of code 
            //this.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }

        public System.Data.Entity.DbSet<Banking.Models.Client> Clients { get; set; }

        public System.Data.Entity.DbSet<Banking.Models.Account> Accounts { get; set; }

        public System.Data.Entity.DbSet<Banking.Models.Currency> Currencies { get; set; }
    }
}
