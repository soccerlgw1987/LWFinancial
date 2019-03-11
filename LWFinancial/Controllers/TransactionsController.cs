using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LWFinancial.Enumerations;
using LWFinancial.Helpers;
using LWFinancial.Models;
using Microsoft.AspNet.Identity;

namespace LWFinancial.Controllers
{
    [RequireHttps]
    [Authorize]
    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private HouseholdsHelper householdHelper = new HouseholdsHelper();
        private AccountHelper accountHelper = new AccountHelper();
        private BudgetItemHelper budgetItemHelper = new BudgetItemHelper();
        private NotificationHelper notificationHelper = new NotificationHelper();
        private TransactionHelper transactionHelper = new TransactionHelper();

        // GET: Transactions
        public ActionResult Index()
        {
            var transactions = db.Transactions.Include(t => t.Account).Include(t => t.BudgetItem).Include(t => t.EnteredBy);
            return View(transactions.ToList());
        }

        // GET: Transactions
        public ActionResult IndexMy()
        {
            var userId = User.Identity.GetUserId();
            int userHousehold = householdHelper.ListUserHousehold(userId);

            if (userHousehold == 0)
            {
                return RedirectToAction("InvalidAttempt", "Home");
            }
            else if (householdHelper.IsUserInHousehold(userId, userHousehold))
            {
                var householdId = householdHelper.ListUserHousehold(User.Identity.GetUserId());
                var accounts = db.Accounts.Where(h => h.HouseholdId == householdId).ToList();
                var budgetItems = db.Households.Find(householdId).Budgets.SelectMany(b => b.BudgetItems);

                ViewBag.AccountId = new SelectList(accounts, "Id", "Name");
                ViewBag.BudgetItemId = new SelectList(budgetItems, "Id", "Name");

                return View();
            }

            return RedirectToAction("InvalidAttempt", "Home");
        }

        // GET: Transactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: Transactions/Create
        public ActionResult Create()
        {
            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Name");
            ViewBag.BudgetItemId = new SelectList(db.BudgetItems, "Id", "Name");
            ViewBag.EnteredById = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Type,Amount,AccountId,BudgetItemId")] Transaction transaction)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);

            if(transaction.Type == TransactionType.Deposit && transaction.BudgetItemId > 0)
            {
                ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Name", transaction.AccountId);
                ViewBag.BudgetItemId = new SelectList(db.BudgetItems, "Id", "Name", transaction.BudgetItemId);
                ViewBag.EnteredById = new SelectList(db.Users, "Id", "FirstName", transaction.EnteredById);
                return View(transaction);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    transaction.Created = DateTime.Now;
                    transaction.EnteredById = User.Identity.GetUserId();

                    if(transaction.Type == TransactionType.Withdrawl)
                    {
                        budgetItemHelper.UpdateBudgetItemIncome(transaction.BudgetItemId, transaction.Amount);
                        accountHelper.UpdateAccountWithdrawlIncome(transaction.AccountId, transaction.Amount);
                    }
                    else
                    {
                        accountHelper.UpdateAccountIncome(transaction.AccountId, transaction.Amount);
                    }

                    db.Transactions.Add(transaction);
                    db.SaveChanges();

                    Account account = db.Accounts.Find(transaction.AccountId);
                    if(account.CurrentBalance <= 0)
                    {
                        notificationHelper.GetOverdraftNotification(householdHelper.ListUserHousehold(User.Identity.GetUserId()), transaction.Account.Name);
                    }
                    else if(account.CurrentBalance <= account.LowBalanceWarning)
                    {
                        notificationHelper.GetLowBalanceNotification(householdHelper.ListUserHousehold(User.Identity.GetUserId()), transaction.Account.Name);
                    }

                    return RedirectToAction("IndexMy");
                }
            }

            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Name", transaction.AccountId);
            ViewBag.BudgetItemId = new SelectList(db.BudgetItems, "Id", "Name", transaction.BudgetItemId);
            ViewBag.EnteredById = new SelectList(db.Users, "Id", "FirstName", transaction.EnteredById);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Name", transaction.AccountId);
            ViewBag.BudgetItemId = new SelectList(db.BudgetItems, "Id", "Name", transaction.BudgetItemId);
            ViewBag.EnteredById = new SelectList(db.Users, "Id", "FirstName", transaction.EnteredById);
            return View(transaction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int transactionId, int AccountId, int? BudgetItemId, string transactionName, string transactionType, decimal transactionAmount, decimal? transactionReconciledAmount)
        {
            var transaction = db.Transactions.Find(transactionId);

            if (transaction.Type == TransactionType.Withdrawl)
            {
                budgetItemHelper.UpdateBudgetItemWithdrawlIncome(transaction.BudgetItemId, transaction.Amount);
                accountHelper.UpdateAccountIncome(transaction.AccountId, transaction.Amount);
                if(transaction.ReconciledAmount > 0)
                {
                    transactionHelper.UpdateReconciledWithdrawlIncome(transaction.Id, transaction.ReconciledAmount);
                    accountHelper.UpdateReconciledWithdrawlIncome(transaction.AccountId, transaction.ReconciledAmount);
                }
            }
            else
            {
                accountHelper.UpdateAccountWithdrawlIncome(transaction.AccountId, transaction.Amount);
            }

            db.SaveChanges();

            transaction.AccountId = AccountId;
            transaction.BudgetItemId = BudgetItemId;
            transaction.Name = transactionName;
            if(transactionType == "Deposit")
            {
                transaction.Type = TransactionType.Deposit;
            }
            else
            {
                transaction.Type = TransactionType.Withdrawl;
            }
            transaction.EnteredById = User.Identity.GetUserId();
            transaction.Updated = DateTime.Now;
            transaction.Amount = transactionAmount;
            db.SaveChanges();

            if (transaction.Type == TransactionType.Withdrawl)
            {
                budgetItemHelper.UpdateBudgetItemIncome(transaction.BudgetItemId, transaction.Amount);
                accountHelper.UpdateAccountWithdrawlIncome(transaction.AccountId, transaction.Amount);
            }
            else
            {
                accountHelper.UpdateAccountIncome(transaction.AccountId, transaction.Amount);
            }
            
            if(transactionReconciledAmount > 0)
            {
                transactionHelper.UpdateReconciledIncome(transaction.Id, transactionReconciledAmount);
                accountHelper.UpdateReconciledIncome(transaction.AccountId, transactionReconciledAmount);

                transaction.ReconciledAmount = transactionReconciledAmount;
            }

            //etc...
            db.Transactions.Attach(transaction);
            db.Entry(transaction).Property(a => a.AccountId).IsModified = true;
            db.Entry(transaction).Property(a => a.BudgetItemId).IsModified = true;
            db.Entry(transaction).Property(a => a.Name).IsModified = true;
            db.Entry(transaction).Property(a => a.EnteredById).IsModified = true;
            db.Entry(transaction).Property(a => a.Amount).IsModified = true;
            db.Entry(transaction).Property(a => a.ReconciledAmount).IsModified = true;
            db.Entry(transaction).Property(a => a.Updated).IsModified = true;
            //etc... for each property that you want to be able to change in the edit.
            db.SaveChanges();
            return RedirectToAction("IndexMy"); //or whatever.
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOld([Bind(Include = "Id,Name,Type,Created,Updated,Amount,ReconciledAmount,Reconciled,AccountId,BudgetItemId,EnteredById")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Name", transaction.AccountId);
            ViewBag.BudgetItemId = new SelectList(db.BudgetItems, "Id", "Name", transaction.BudgetItemId);
            ViewBag.EnteredById = new SelectList(db.Users, "Id", "FirstName", transaction.EnteredById);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int transactionId)
        {
            Transaction transaction = db.Transactions.Find(transactionId);

            if (transaction.Type == TransactionType.Withdrawl)
            {
                budgetItemHelper.UpdateBudgetItemWithdrawlIncome(transaction.BudgetItemId, transaction.Amount);
                accountHelper.UpdateAccountIncome(transaction.AccountId, transaction.Amount);
                if (transaction.ReconciledAmount > 0)
                {
                    transactionHelper.UpdateReconciledWithdrawlIncome(transaction.Id, transaction.ReconciledAmount);
                    accountHelper.UpdateReconciledWithdrawlIncome(transaction.AccountId, transaction.ReconciledAmount);
                }
            }
            else
            {
                accountHelper.UpdateAccountWithdrawlIncome(transaction.AccountId, transaction.Amount);
            }

            db.SaveChanges();

            db.Transactions.Remove(transaction);
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
