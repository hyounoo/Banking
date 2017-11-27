using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Banking.Models;
using System.Web.Http.Results;

namespace Banking.Controllers
{
    public class AccountsController : ApiController
    {
        private BankingContext db = new BankingContext();

        #region Custom APIs
        #region ConvertingCurrency
        private decimal ConvertToUSD(string currencyCode, decimal amount)
        {
            var currencyVSUSD = from c in db.Currencies
                                where c.CurrencyCode == currencyCode
                                select c.CurrencyVSUSD;

            return (decimal)currencyVSUSD.FirstOrDefault() * amount;
        }

        private decimal ConvertBackFromUSD(string currencyCode, decimal amount)
        {
            var currencyVSUSD = from c in db.Currencies
                                where c.CurrencyCode == currencyCode
                                select c.CurrencyVSUSD;

            return amount / (decimal)currencyVSUSD.FirstOrDefault();
        }
        #endregion

        /// <summary>
        /// A web service method used to retrive funds in an account.
        /// </summary>
        /// <param name="accountNumber" type="Integer">The account number of which to retrieve the balance.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/accounts/Balance")]
        [ResponseType(typeof(BankResponseViewModel))]
        public async Task<IHttpActionResult> Balance(int accountNumber)
        {
            var account = await db.Accounts.Where(a => a.AccountNumber == accountNumber).FirstOrDefaultAsync();

            var result = new BankResponseViewModel()
            {
                AccountNumber = accountNumber,
                Successful = false
            };

            if (account == null)
            {
                result.Message = "Account not found.";
            }
            else
            {
                result.Successful = true;
                result.Balance = account.Balance;
                result.Currency = account.Currency.CurrencyCode;
            }

            return Ok(result);
        }

        /// <summary>
        /// A web service method used to deposit funds in an account.
        /// </summary>
        /// <param name="accountNumber">The account number to deposit the funds to.</param>
        /// <param name="amount">The value of funds being deposited.</param>
        /// <param name="currency">The currency of the funds being deposited.</param>
        /// <returns></returns>
        [Route("api/accounts/Deposit")]
        [ResponseType(typeof(BankResponseViewModel))]
        public async Task<IHttpActionResult> Deposit(int accountNumber, decimal amount, string currency)
        {
            var account = await db.Accounts.Where(a => a.AccountNumber == accountNumber).FirstOrDefaultAsync();

            var result = new BankResponseViewModel()
            {
                AccountNumber = accountNumber,
                Successful = false
            };

            if (account == null)
            {
                result.Message = "Account not found.";
            }
            else if (amount <= 0)
            {
                result.Message = "Invalid ammount.";
            }
            else
            {
                try
                {
                    var defaultCurrency = await db.Currencies.Where(c => c.CurrencyCode == "USD").Select(r => r.CurrencyCode).FirstOrDefaultAsync();
                    var accountCurrency = account.Currency.CurrencyCode;
                    var targetCurrency = currency;

                    var balance = account.Currency.CurrencyCode == defaultCurrency ? account.Balance :
                        ConvertToUSD(account.Currency.CurrencyCode, account.Balance);

                    var targetAmount = currency == defaultCurrency ? amount :
                        ConvertToUSD(currency, amount);

                    balance += targetAmount;

                    account.Balance = account.Currency.CurrencyCode == defaultCurrency ? balance :
                        ConvertBackFromUSD(account.Currency.CurrencyCode, balance);

                    db.SaveChanges();
                    result.Successful = true;
                    result.Balance = account.Balance;
                    result.Currency = account.Currency.CurrencyCode;
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }

            return Ok(result);
        }

        /// <summary>
        /// A web service method used to withdraw funds in an account.
        /// </summary>
        /// <param name="accountNumber">The account number to withdraw the funds from.</param>
        /// <param name="amount">The value of funds being withdrawn.</param>
        /// <param name="currency">The currency of the funds being withdrawn.</param>
        /// <returns></returns>
        [Route("api/accounts/Withdraw")]
        [ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> Withdraw(int accountNumber, decimal amount, string currency)
        {
            var result = new BankResponseViewModel()
            {
                AccountNumber = accountNumber,
                Successful = false
            };

            var account = await db.Accounts.Where(a => a.AccountNumber == accountNumber).FirstOrDefaultAsync();

            var defaultCurrency = await db.Currencies.Where(c => c.CurrencyCode == "USD").Select(r => r.CurrencyCode).FirstOrDefaultAsync();
            var accountCurrency = account.Currency.CurrencyCode;
            var targetCurrency = currency;

            var balance = account.Currency.CurrencyCode == defaultCurrency ? account.Balance :
                ConvertToUSD(account.Currency.CurrencyCode, account.Balance);

            var targetAmount = currency == defaultCurrency ? amount :
                ConvertToUSD(currency, amount);

            if (account == null)
            {
                result.Message = "Account not found.";
            }
            else if (amount <= 0)
            {
                result.Message = "Invalid ammount.";
            }
            else if (balance < targetAmount)
            {
                result.Message = "Insufficient balance.";
            }
            else
            {
                try
                {
                    balance -= targetAmount;

                    account.Balance = account.Currency.CurrencyCode == defaultCurrency ? balance :
                        ConvertBackFromUSD(account.Currency.CurrencyCode, balance);

                    db.SaveChanges();
                    result.Successful = true;
                    result.Balance = account.Balance;
                    result.Currency = account.Currency.CurrencyCode;
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }

            return Ok(result);
        }
        #endregion

        // !!Caution These APIs return POCO classes!!
        // Consider using DTO instead. ex) POCO => ViewModel.
        // Please refer Admin area of this project for the sample codes.

        #region Scaffolded APIs
        // GET: api/Accounts
        public IQueryable<Account> GetAccounts()
        {
            return db.Accounts;
        }

        // GET: api/Accounts1/5
        [ResponseType(typeof(Account))]
        public async Task<IHttpActionResult> GetAccount(int id)
        {
            Account account = await db.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }

        // PUT: api/Accounts1/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAccount(int id, Account account)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != account.Id)
            {
                return BadRequest();
            }

            db.Entry(account).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Accounts1
        [ResponseType(typeof(Account))]
        public async Task<IHttpActionResult> PostAccount(Account account)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Accounts.Add(account);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = account.Id }, account);
        }

        // DELETE: api/Accounts1/5
        [ResponseType(typeof(Account))]
        public async Task<IHttpActionResult> DeleteAccount(int id)
        {
            Account account = await db.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            db.Accounts.Remove(account);
            await db.SaveChangesAsync();

            return Ok(account);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AccountExists(int id)
        {
            return db.Accounts.Count(e => e.AccountNumber == id) > 0;
        }
        #endregion
    }
}