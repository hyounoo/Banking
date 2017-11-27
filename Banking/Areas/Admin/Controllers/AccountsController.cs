using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Banking.Models;
using Banking.Helper;
using Banking.Areas.Admin.ViewModels;
using X.PagedList;
using System.Collections.Generic;

namespace Banking.Areas.Admin.Controllers
{
    public class AccountsController : Controller
    {
        private BankingContext db = new BankingContext();

        #region Private methods
        private IQueryable<Account> SearchAccountsByCriteria(AccountSearchViewModel viewModel)
        {
            var query = from a in db.Accounts
                        select a;

            if (viewModel.AccountNumber != null)
            {
                query = from a in query
                        where a.AccountNumber == viewModel.AccountNumber
                        select a;
            }

            if (!string.IsNullOrEmpty(viewModel.Title))
            {
                query = from a in query
                        where a.Title.ToLower().Contains(viewModel.Title.ToLower())
                        select a;
            }

            if (!string.IsNullOrEmpty(viewModel.ClientName))
            {
                query = from a in query
                        where a.Client.Name.ToLower().Contains(viewModel.ClientName.ToLower())
                        select a;
            }

            if (viewModel.MinBalance != null)
            {
                query = from a in query
                        where a.Balance >= viewModel.MinBalance
                        select a;
            }

            if (viewModel.MaxBalance != null)
            {
                query = from a in query
                        where a.Balance <= viewModel.MaxBalance
                        select a;
            }

            if (viewModel.CurrencyId != null)
            {
                query = from a in query
                        where a.CurrencyId == viewModel.CurrencyId
                        select a;
            }

            if (!string.IsNullOrEmpty(viewModel.ClientName))
            {
                query = from a in query
                        join c in db.Clients
                        on a.ClientId equals c.Id
                        where c.Name.ToLower().Contains(viewModel.ClientName.ToLower())
                        select a;
            }

            query = ApplySortOrder(viewModel, query);

            return query;
        }

        private IQueryable<Account> ApplySortOrder(AccountSearchViewModel viewModel, IQueryable<Account> query)
        {
            #region Apply sort order

            switch (viewModel.SortParameter)
            {
                case "AccountNumberSortParameter":
                    query = query.OrderBy(q => q.AccountNumber);
                    viewModel.AccountNumberSortParameter = MVCStringHelper.ConvertSortDirection(viewModel.SortParameter);
                    break;
                case "AccountNumberSortParameter_desc":
                    query = query.OrderByDescending(q => q.AccountNumber);
                    viewModel.AccountNumberSortParameter = MVCStringHelper.ConvertSortDirection(viewModel.SortParameter);
                    break;
                case "TitleSortParameter":
                    query = query.OrderBy(q => q.Title);
                    viewModel.TitleSortParameter = MVCStringHelper.ConvertSortDirection(viewModel.SortParameter);
                    break;
                case "TitleSortParameter_desc":
                    query = query.OrderByDescending(q => q.Title);
                    viewModel.TitleSortParameter = MVCStringHelper.ConvertSortDirection(viewModel.SortParameter);
                    break;
                case "ClientNameSortParameter":
                    query = query.OrderBy(q => q.Client.Name);
                    viewModel.ClientNameSortParameter = MVCStringHelper.ConvertSortDirection(viewModel.SortParameter);
                    break;
                case "ClientNameSortParameter_desc":
                    query = query.OrderByDescending(q => q.Client.Name);
                    viewModel.ClientNameSortParameter = MVCStringHelper.ConvertSortDirection(viewModel.SortParameter);
                    break;
                case "BalanceSortParameter":
                    query = query.OrderBy(q => q.Balance);
                    viewModel.BalanceSortParameter = MVCStringHelper.ConvertSortDirection(viewModel.SortParameter);
                    break;
                case "BalanceSortParameter_desc":
                    query = query.OrderByDescending(q => q.Balance);
                    viewModel.BalanceSortParameter = MVCStringHelper.ConvertSortDirection(viewModel.SortParameter);
                    break;
                case "CreatedDateSortParameter":
                    query = query.OrderBy(q => q.CreatedDate);
                    viewModel.CreatedDateSortParameter = MVCStringHelper.ConvertSortDirection(viewModel.SortParameter);
                    break;
                case "CreatedDateSortParameter_desc":
                    query = query.OrderByDescending(q => q.CreatedDate);
                    viewModel.CreatedDateSortParameter = MVCStringHelper.ConvertSortDirection(viewModel.SortParameter);
                    break;
                case "ModifiedDateSortParameter":
                    query = query.OrderBy(q => q.ModifiedDate);
                    viewModel.ModifiedDateSortParameter = MVCStringHelper.ConvertSortDirection(viewModel.SortParameter);
                    break;
                case "ModifiedDateSortParameter_desc":
                    query = query.OrderByDescending(q => q.ModifiedDate);
                    viewModel.ModifiedDateSortParameter = MVCStringHelper.ConvertSortDirection(viewModel.SortParameter);
                    break;

                default:
                    query = query.OrderBy(q => q.AccountNumber);
                    break;
            }

            #endregion

            return query;
        }

        internal AccountViewModel SearchAccountById(int? id)
        {
            var query = from a in db.Accounts
                        where a.Id == id
                        select new AccountViewModel
                        {
                            Id = a.Id,
                            ClientId = a.ClientId,
                            Title = a.Title,
                            AccountNumber = a.AccountNumber,
                            Balance = a.Balance,
                            CurrencyId = a.CurrencyId,
                            CreatedDate = a.CreatedDate,
                            ModifiedDate = a.ModifiedDate
                        };

            return id != null ? query.FirstOrDefault() : new AccountViewModel();
        }

        internal AccountViewModel SearchAccountByAccountNumber(int? accountNumber)
        {
            var query = from a in db.Accounts
                        where a.AccountNumber == accountNumber
                        select new AccountViewModel
                        {
                            Id = a.Id,
                            ClientId = a.ClientId,
                            Title = a.Title,
                            AccountNumber = a.AccountNumber,
                            Balance = a.Balance,
                            CurrencyId = a.CurrencyId,
                            CreatedDate = a.CreatedDate,
                            ModifiedDate = a.ModifiedDate
                        };

            return accountNumber != null ? query.FirstOrDefault() : new AccountViewModel();
        }

        private void UpsertAccount(AccountViewModel viewModel)
        {
            using (var tran = db.Database.BeginTransaction())
            {
                try
                {
                    var exist = viewModel.Id != 0;

                    var now = DateTime.Now;

                    var account = exist ? db.Accounts.Find(viewModel.Id) : new Account();

                    account.Title = viewModel.Title;
                    account.ModifiedDate = now;
                    account.ClientId = viewModel.ClientId;
                    account.Balance = viewModel.Balance;
                    account.CurrencyId = viewModel.CurrencyId;
                    if (!exist)
                    {
                        account.AccountNumber = GetNextAccountNumber() + 1;
                        account.CreatedDate = now;
                        db.Accounts.Add(account);
                    }
                    db.SaveChanges();
                    tran.Commit();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    tran.Rollback();
                }
            }
        }

        private int GetNextAccountNumber()
        {
            return db.Accounts.Max(a => a.AccountNumber);
        }
        #endregion

        // GET: Account
        public ActionResult Index(AccountSearchViewModel viewModel)
        {
            IQueryable<AccountViewModel> result = null;

            if (ModelState.IsValid)
            {
                var query = SearchAccountsByCriteria(viewModel);

                result = from a in query
                         select new AccountViewModel
                         {
                             Id = a.Id,
                             AccountNumber = a.AccountNumber,
                             Title = a.Title,
                             Balance = a.Balance,
                             CurrencyId = a.CurrencyId,
                             ClientId = a.ClientId,
                             ClientName = a.Client.Name,
                             CreatedDate = a.CreatedDate,
                             ModifiedDate = a.ModifiedDate
                         };
            }
            else
            {
                var emptyList = new List<AccountViewModel>();
                result = emptyList.AsQueryable();
            }

            viewModel.AccountList = result.ToPagedList(viewModel.page ?? 1, viewModel.pageSize ?? 10);

            ViewBag.CurrencyId = new SelectList(db.Currencies, "Id", "CurrencyCode", null);

            return View(viewModel);
        }

        // GET: Account/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var viewModel = SearchAccountById(id);

            if (viewModel == null)
            {
                return HttpNotFound();
            }

            ViewBag.ClientId = new SelectList(db.Clients, "Id", "Name", viewModel.ClientId);
            ViewBag.CurrencyId = new SelectList(db.Currencies, "Id", "CurrencyCode", viewModel.CurrencyId);

            return View(viewModel);
        }

        // GET: Account/Edit/5
        public ActionResult Edit(int? id)
        {
            var viewModel = SearchAccountById(id);

            if (viewModel == null)
            {
                return HttpNotFound();
            }

            ViewBag.ClientId = new SelectList(db.Clients, "Id", "Name", viewModel.ClientId);
            ViewBag.CurrencyId = new SelectList(db.Currencies, "Id", "CurrencyCode", viewModel.CurrencyId);

            return View(viewModel);
        }

        // POST: Account/Edit/viewModel
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AccountViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                UpsertAccount(viewModel);
                return RedirectToAction("Index");
            }

            ViewBag.ClientId = new SelectList(db.Clients, "Id", "Name", viewModel.ClientId);
            ViewBag.CurrencyId = new SelectList(db.Currencies, "Id", "CurrencyCode", viewModel.CurrencyId);
            return View(viewModel);
        }

        // GET: Account/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var viewModel = SearchAccountById(id);

            if (viewModel == null)
            {
                return HttpNotFound();
            }

            ViewBag.ClientId = new SelectList(db.Clients, "Id", "Name", viewModel.ClientId);
            ViewBag.CurrencyId = new SelectList(db.Currencies, "Id", "CurrencyCode", viewModel.CurrencyId);

            return View(viewModel);
        }

        // POST: Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Account account = db.Accounts.Find(id);
            db.Accounts.Remove(account);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
