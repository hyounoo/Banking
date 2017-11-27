namespace Banking.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Banking.Models.BankingContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Banking.Models.BankingContext context)
        {
            //  Code First Migrations in EF to seed the database with test data.
            //  From the Tools menu, select Library Package Manager, then select Package Manager Console. In the Package Manager Console window, enter the following command:
            //  Enable-Migrations
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.

            //  In the Package Manager Console window, type the following commands after changing seed data :
            //  Add-Migration Initial
            //  Update-Database

            //  On Database update error: refer below URL
            //  https://stackoverflow.com/a/15832184

            var createDate = DateTime.Now;

            context.Clients.AddOrUpdate(
              p => p.Id,
              new Client { Name = "Andrew Peters", CreatedDate = createDate, ModifiedDate = createDate },
              new Client { Name = "Brice Lambson", CreatedDate = createDate, ModifiedDate = createDate },
              new Client { Name = "Rowan Miller", CreatedDate = createDate, ModifiedDate = createDate }
            );

            context.Currencies.AddOrUpdate(
              p => p.Id,
              new Currency { Id = 1, CurrencyCode = "AED", CurrencyVSUSD = 0.27225701 },
              new Currency { Id = 2, CurrencyCode = "ARS", CurrencyVSUSD = 0.0631313131313131 },
              new Currency { Id = 3, CurrencyCode = "AUD", CurrencyVSUSD = 0.74 },
              new Currency { Id = 4, CurrencyCode = "AZN", CurrencyVSUSD = 0.57323015 },
              new Currency { Id = 5, CurrencyCode = "BBD", CurrencyVSUSD = 0.5 },
              new Currency { Id = 6, CurrencyCode = "BGL", CurrencyVSUSD = 0.54338966 },
              new Currency { Id = 7, CurrencyCode = "BGN", CurrencyVSUSD = 0.54338966 },
              new Currency { Id = 8, CurrencyCode = "BHD", CurrencyVSUSD = 2.65237918 },
              new Currency { Id = 9, CurrencyCode = "BHT", CurrencyVSUSD = 0.02814523 },
              new Currency { Id = 0, CurrencyCode = "BMD", CurrencyVSUSD = 1 },
              new Currency { Id = 10, CurrencyCode = "BND", CurrencyVSUSD = 0.70209928 },
              new Currency { Id = 11, CurrencyCode = "BRD", CurrencyVSUSD = 0.70209928 },
              new Currency { Id = 12, CurrencyCode = "BRL", CurrencyVSUSD = 0.288184438040346 },
              new Currency { Id = 13, CurrencyCode = "BWP", CurrencyVSUSD = 0.0919 },
              new Currency { Id = 14, CurrencyCode = "CAD", CurrencyVSUSD = 0.75 },
              new Currency { Id = 15, CurrencyCode = "CHF", CurrencyVSUSD = 0.99127676 },
              new Currency { Id = 16, CurrencyCode = "CLP", CurrencyVSUSD = 0.00148240386610928 },
              new Currency { Id = 17, CurrencyCode = "CNY", CurrencyVSUSD = 0.14507892 },
              new Currency { Id = 18, CurrencyCode = "COP", CurrencyVSUSD = 0.000325000325000325 },
              new Currency { Id = 19, CurrencyCode = "CZK", CurrencyVSUSD = 0.03932595 },
              new Currency { Id = 20, CurrencyCode = "DKK", CurrencyVSUSD = 0.14285102 },
              new Currency { Id = 21, CurrencyCode = "DOP", CurrencyVSUSD = 0.021505376344086 },
              new Currency { Id = 22, CurrencyCode = "DZD", CurrencyVSUSD = 0.00898572 },
              new Currency { Id = 23, CurrencyCode = "EGP", CurrencyVSUSD = 0.05691358 },
              new Currency { Id = 24, CurrencyCode = "EUR", CurrencyVSUSD = 1.06 },
              new Currency { Id = 25, CurrencyCode = "FJD", CurrencyVSUSD = 0.4769 },
              new Currency { Id = 26, CurrencyCode = "GBP", CurrencyVSUSD = 1.25 },
              new Currency { Id = 27, CurrencyCode = "GTQ", CurrencyVSUSD = 0.13321788 },
              new Currency { Id = 28, CurrencyCode = "HKD", CurrencyVSUSD = 0.12893244 },
              new Currency { Id = 29, CurrencyCode = "HRK", CurrencyVSUSD = 0.14101588 },
              new Currency { Id = 30, CurrencyCode = "HUF", CurrencyVSUSD = 0.00343891 },
              new Currency { Id = 31, CurrencyCode = "IDR", CurrencyVSUSD = 0.00007459 },
              new Currency { Id = 32, CurrencyCode = "ILS", CurrencyVSUSD = 0.25880587 },
              new Currency { Id = 33, CurrencyCode = "INR", CurrencyVSUSD = 0.01467029 },
              new Currency { Id = 34, CurrencyCode = "JMD", CurrencyVSUSD = 0.00773994 },
              new Currency { Id = 35, CurrencyCode = "JOD", CurrencyVSUSD = 1.4120305 },
              new Currency { Id = 36, CurrencyCode = "JPY", CurrencyVSUSD = 0.00902364 },
              new Currency { Id = 37, CurrencyCode = "KES", CurrencyVSUSD = 0.00981836 },
              new Currency { Id = 38, CurrencyCode = "KRW", CurrencyVSUSD = 0.00084271 },
              new Currency { Id = 39, CurrencyCode = "KWD", CurrencyVSUSD = 3.28051701 },
              new Currency { Id = 40, CurrencyCode = "KZT", CurrencyVSUSD = 0.00295404 },
              new Currency { Id = 41, CurrencyCode = "MOP", CurrencyVSUSD = 0.12517838 },
              new Currency { Id = 42, CurrencyCode = "MSD", CurrencyVSUSD = 0.22625458 },
              new Currency { Id = 43, CurrencyCode = "MXN", CurrencyVSUSD = 0.0480769230769231 },
              new Currency { Id = 44, CurrencyCode = "MYR", CurrencyVSUSD = 0.22625458 },
              new Currency { Id = 45, CurrencyCode = "NAD", CurrencyVSUSD = 0.07018233 },
              new Currency { Id = 46, CurrencyCode = "NGN", CurrencyVSUSD = 0.00316987 },
              new Currency { Id = 47, CurrencyCode = "NOK", CurrencyVSUSD = 0.11703435 },
              new Currency { Id = 48, CurrencyCode = "NTD", CurrencyVSUSD = 0.03127346 },
              new Currency { Id = 49, CurrencyCode = "NZD", CurrencyVSUSD = 0.7066 },
              new Currency { Id = 50, CurrencyCode = "OMR", CurrencyVSUSD = 2.5974026 },
              new Currency { Id = 51, CurrencyCode = "PEN", CurrencyVSUSD = 0.292397660818713 },
              new Currency { Id = 52, CurrencyCode = "PGK", CurrencyVSUSD = 0.3155 },
              new Currency { Id = 53, CurrencyCode = "PHP", CurrencyVSUSD = 0.02007065 },
              new Currency { Id = 54, CurrencyCode = "PKR", CurrencyVSUSD = 0.00948699 },
              new Currency { Id = 55, CurrencyCode = "PLN", CurrencyVSUSD = 0.24003841 },
              new Currency { Id = 56, CurrencyCode = "QAR", CurrencyVSUSD = 0.27464228 },
              new Currency { Id = 57, CurrencyCode = "RMB", CurrencyVSUSD = 0.14507892 },
              new Currency { Id = 58, CurrencyCode = "RON", CurrencyVSUSD = 0.23563787 },
              new Currency { Id = 59, CurrencyCode = "RSD", CurrencyVSUSD = 0.00863059 },
              new Currency { Id = 60, CurrencyCode = "RUB", CurrencyVSUSD = 0.01563081 },
              new Currency { Id = 61, CurrencyCode = "SAR", CurrencyVSUSD = 0.2666169 },
              new Currency { Id = 62, CurrencyCode = "SEK", CurrencyVSUSD = 0.10845281 },
              new Currency { Id = 63, CurrencyCode = "SGD", CurrencyVSUSD = 0.70219788 },
              new Currency { Id = 64, CurrencyCode = "THB", CurrencyVSUSD = 0.02814523 },
              new Currency { Id = 65, CurrencyCode = "TND", CurrencyVSUSD = 0.43654778 },
              new Currency { Id = 66, CurrencyCode = "TOP", CurrencyVSUSD = 0.4632 },
              new Currency { Id = 67, CurrencyCode = "TRY", CurrencyVSUSD = 0.29750394 },
              new Currency { Id = 68, CurrencyCode = "TTD", CurrencyVSUSD = 0.14825577 },
              new Currency { Id = 69, CurrencyCode = "TWD", CurrencyVSUSD = 0.03127346 },
              new Currency { Id = 70, CurrencyCode = "UAH", CurrencyVSUSD = 0.03886136 },
              new Currency { Id = 71, CurrencyCode = "USD", CurrencyVSUSD = 1 },
              new Currency { Id = 72, CurrencyCode = "UYU", CurrencyVSUSD = 0.0345423143350604 },
              new Currency { Id = 73, CurrencyCode = "VEF", CurrencyVSUSD = 0.0005 },
              new Currency { Id = 74, CurrencyCode = "VND", CurrencyVSUSD = 0.00004446 },
              new Currency { Id = 75, CurrencyCode = "VUV", CurrencyVSUSD = 0.00906372 },
              new Currency { Id = 76, CurrencyCode = "WST", CurrencyVSUSD = 0.4027 },
              new Currency { Id = 77, CurrencyCode = "XAF", CurrencyVSUSD = 0.00161396 },
              new Currency { Id = 78, CurrencyCode = "ZAR", CurrencyVSUSD = 0.07018233 },
              new Currency { Id = 79, CurrencyCode = "ZWD", CurrencyVSUSD = 0 },
              new Currency { Id = 80, CurrencyCode = "ZWL", CurrencyVSUSD = 0 }
              );

            context.Accounts.AddOrUpdate(
              p => p.AccountNumber,
              new Account { AccountNumber = 10000001, ClientId = 1, Balance = 10000, CurrencyId = 71, CreatedDate = createDate, ModifiedDate = createDate, Title = "Andrew's first account" },
              new Account { AccountNumber = 10000002, ClientId = 1, Balance = 20000, CurrencyId = 71, CreatedDate = createDate, ModifiedDate = createDate, Title = "Andrew's second account" },
              new Account { AccountNumber = 10000003, ClientId = 1, Balance = 30000, CurrencyId = 71, CreatedDate = createDate, ModifiedDate = createDate, Title = "Andrew's third account" },
              new Account { AccountNumber = 10000004, ClientId = 2, Balance = 40000, CurrencyId = 71, CreatedDate = createDate, ModifiedDate = createDate, Title = "Brice's first account" },
              new Account { AccountNumber = 10000005, ClientId = 2, Balance = 50000, CurrencyId = 71, CreatedDate = createDate, ModifiedDate = createDate, Title = "Brice's second account" },
              new Account { AccountNumber = 10000006, ClientId = 3, Balance = 60000, CurrencyId = 71, CreatedDate = createDate, ModifiedDate = createDate, Title = "Rowan's first account" },
              new Account { AccountNumber = 10000007, ClientId = 3, Balance = 70000, CurrencyId = 71, CreatedDate = createDate, ModifiedDate = createDate, Title = "Rowan's second account" },
              new Account { AccountNumber = 10000008, ClientId = 3, Balance = 80000, CurrencyId = 71, CreatedDate = createDate, ModifiedDate = createDate, Title = "Rowan's third account" },
              new Account { AccountNumber = 10000009, ClientId = 3, Balance = 900000, CurrencyId = 71, CreatedDate = createDate, ModifiedDate = createDate, Title = "Rowan is rich!!!" }
            );
        }
    }
}
