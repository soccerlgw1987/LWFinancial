using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LWFinancial.Helpers;
using LWFinancial.Models;
using Microsoft.AspNet.Identity;

namespace LWFinancial.Controllers
{
    [RequireHttps]
    public class AccountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private AccountHelper accountHelper = new AccountHelper();
        private HouseholdsHelper householdHelper = new HouseholdsHelper();

        // GET: Accounts
        public ActionResult Index()
        {
            var accounts = db.Accounts.Include(a => a.Household);
            return View(accounts.ToList());
        }

        public ActionResult IndexMy()
        {
            return View();
        }

        // GET: Accounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // GET: Accounts/Create
        public ActionResult Create()
        {
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name");
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,InitialBalance,LowBalanceWarning")] Account account, int householdId)
        {
            account.CurrentBalance = account.InitialBalance;
            account.HouseholdId = householdId;
            account.Created = DateTime.Now;
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {

                db.Accounts.Add(account);
                db.SaveChanges();
                return RedirectToAction("IndexMy");
            }

            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", account.HouseholdId);
            return View(account);
        }

        // GET: Accounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", account.HouseholdId);
            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int accountId, string accountName, decimal? reconciledAmount, decimal lowBalance)
        {
            var account = db.Accounts.Find(accountId);
            account.Name = accountName;
            account.ReconciledBalance = reconciledAmount;
            account.LowBalanceWarning = lowBalance;
            //etc...
            db.Accounts.Attach(account);
            db.Entry(account).Property(a => a.Name).IsModified = true;
            db.Entry(account).Property(a => a.ReconciledBalance).IsModified = true;
            db.Entry(account).Property(a => a.LowBalanceWarning).IsModified = true;
            //etc... for each property that you want to be able to change in the edit.
            db.SaveChanges();
            return RedirectToAction("IndexMy"); //or whatever.
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOld([Bind(Include = "Id,Name,InitialBalance,CurrentBalance,ReconciledBalance,LowBalanceWarning,Created,HouseholdId")] Account account, int Id, int HouseholdId, DateTime Created, decimal CurrentBalance, decimal InitialBalance)
        {
            account.Id = Id;
            account.HouseholdId = HouseholdId;
            account.Created = Created;
            account.CurrentBalance = CurrentBalance;
            account.InitialBalance = InitialBalance;
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexMy");
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", account.HouseholdId);
            return View(account);
        }

        // GET: Accounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int accountId)
        //{
        //    var account = db.Accounts.Find(accountId);
        //    db.Accounts.Remove(account);
        //    db.SaveChanges();
        //    return RedirectToAction("IndexMy");
        //}

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int accountId)
        {
            Account account = db.Accounts.Find(accountId);
            db.Accounts.Remove(account);
            db.SaveChanges();
            return RedirectToAction("IndexMy");
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
