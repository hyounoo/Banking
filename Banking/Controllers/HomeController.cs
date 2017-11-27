using Banking.Areas.Admin.Controllers;
using Banking.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Banking.Controllers
{
    public class HomeController : Controller
    {
        ClientsController clientsController = new ClientsController();

        public ActionResult Index()
        {
            ViewBag.Title = "Banking POC";

            List<string> clients = Bank.Instance.GetConnectionIds();

            ViewBag.ClientSelectList = new SelectList(clients, null);

            return View();
        }
    }
}
