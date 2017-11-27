using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Banking.Models;
using Banking.Areas.Admin.ViewModels;
using X.PagedList;
using Banking.Helper;

namespace Banking.Areas.Admin.Controllers
{
    public class ClientsController : Controller
    {
        private BankingContext db = new BankingContext();

        #region Private methods
        private IQueryable<Client> SearchClientsByCriteria(ClientSearchViewModel viewModel)
        {
            var query = from c in db.Clients
                        select c;

            if (!string.IsNullOrEmpty(viewModel.ClientName))
            {
                query = from c in query
                        where c.Name.ToLower().Contains(viewModel.ClientName.ToLower())
                        select c;
            }

            query = ApplySortOrder(viewModel, query);

            return query;
        }

        internal ClientViewModel SearchClientById(int? id)
        {
            var query = from c in db.Clients
                        where c.Id == id
                        select new ClientViewModel
                        {
                            ClientId = c.Id,
                            ClientName = c.Name,
                            CreatedDate = c.CreatedDate,
                            ModifiedDate = c.ModifiedDate,
                            AccountList = c.Accounts.Select(a => new AccountViewModel
                            {
                                Id = a.Id,
                                ClientId = a.ClientId,
                                Title = a.Title,
                                AccountNumber = a.AccountNumber,
                                Balance = a.Balance,
                                CurrencyId = a.CurrencyId,
                                CreatedDate = a.CreatedDate,
                                ModifiedDate = a.ModifiedDate
                            }).ToList()
                        };

            return id != null ? query.FirstOrDefault() : new ClientViewModel { AccountList = new List<AccountViewModel>() };
        }

        private IQueryable<Client> ApplySortOrder(ClientSearchViewModel viewModel, IQueryable<Client> query)
        {
            #region Apply sort order

            switch (viewModel.SortParameter)
            {
                case "NameSortParameter":
                    query = query.OrderBy(q => q.Name);
                    viewModel.NameSortParameter = MVCStringHelper.ConvertSortDirection(viewModel.SortParameter);
                    break;
                case "NameSortParameter_desc":
                    query = query.OrderByDescending(q => q.Name);
                    viewModel.NameSortParameter = MVCStringHelper.ConvertSortDirection(viewModel.SortParameter);
                    break;
                case "AccountsSortParameter":
                    query = query.OrderBy(q => q.Accounts.Count());
                    viewModel.AccountsSortParameter = MVCStringHelper.ConvertSortDirection(viewModel.SortParameter);
                    break;
                case "AccountsSortParameter_desc":
                    query = query.OrderByDescending(q => q.Accounts.Count());
                    viewModel.AccountsSortParameter = MVCStringHelper.ConvertSortDirection(viewModel.SortParameter);
                    break;
                case "AccountTotalSortParameter":
                    query = query.OrderBy(q => q.Accounts.Sum(a => a.Balance));
                    viewModel.AccountTotalSortParameter = MVCStringHelper.ConvertSortDirection(viewModel.SortParameter);
                    break;
                case "AccountTotalSortParameter_desc":
                    query = query.OrderByDescending(q => q.Accounts.Sum(a => a.Balance));
                    viewModel.AccountTotalSortParameter = MVCStringHelper.ConvertSortDirection(viewModel.SortParameter);
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
                    query = query.OrderBy(q => q.CreatedDate);
                    viewModel.CreatedDateSortParameter = MVCStringHelper.ConvertSortDirection(viewModel.SortParameter);
                    break;
                case "ModifiedDateSortParameter_desc":
                    query = query.OrderByDescending(q => q.CreatedDate);
                    viewModel.CreatedDateSortParameter = MVCStringHelper.ConvertSortDirection(viewModel.SortParameter);
                    break;

                default:
                    query = query.OrderBy(q => q.Name);
                    break;
            }

            #endregion

            return query;
        }

        internal void UpsertClient(ClientViewModel viewModel)
        {
            using (var tran = db.Database.BeginTransaction())
            {
                try
                {
                    var exist = viewModel.ClientId != 0;

                    var now = DateTime.Now;

                    var client = exist ? db.Clients.Find(viewModel.ClientId) : new Client();

                    client.Name = viewModel.ClientName;
                    client.ModifiedDate = now;

                    if (!exist)
                    {
                        client.CreatedDate = now;
                        db.Clients.Add(client);
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
        #endregion

        // GET: Client
        public ActionResult Index(ClientSearchViewModel viewModel)
        {
            IQueryable<Client> query = null;

            if (ModelState.IsValid)
            {
                query = SearchClientsByCriteria(viewModel);
            }

            var result = from c in query
                         select new ClientViewModel
                         {
                             ClientId = c.Id,
                             ClientName = c.Name,
                             CreatedDate = c.CreatedDate,
                             ModifiedDate = c.ModifiedDate,
                             AccountList = c.Accounts.Select(a => new AccountViewModel
                             {
                                 Id = a.Id,
                                 AccountNumber = a.AccountNumber,
                                 Title = a.Title,
                                 Balance = a.Balance,
                                 CurrencyId = a.CurrencyId
                             }).ToList()
                         };

            viewModel.ClientList = result.ToPagedList(viewModel.page ?? 1, viewModel.pageSize ?? 10);

            return View(viewModel);
        }

        // GET: Client/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.ClientId = id;
            return View();
        }

        // GET: Client/Edit/5
        public ActionResult Edit(int? id)
        {
            var viewModel = SearchClientById(id);

            if (viewModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.CurrencyId = new SelectList(db.Currencies, "Id", "CurrencyCode", null);
            return View(viewModel);
        }

        // POST: Client/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClientViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                UpsertClient(viewModel);
                return RedirectToAction("Index");
            }
            ViewBag.CurrencyId = new SelectList(db.Currencies, "Id", "CurrencyCode", null);
            return View(viewModel);
        }

        // GET: Client/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var viewModel = SearchClientById(id);

            if (viewModel == null)
            {
                return HttpNotFound();
            }

            ViewBag.CurrencyId = new SelectList(db.Currencies, "Id", "CurrencyCode", null);
            return View(viewModel);
        }

        // POST: Client/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Client client = db.Clients.Find(id);
            db.Clients.Remove(client);
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
