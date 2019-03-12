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
    [Authorize]
    public class BudgetItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private BudgetHelper budgetHelper = new BudgetHelper();
        private HouseholdsHelper householdHelper = new HouseholdsHelper();
        private Budget budget = new Budget();

        // GET: BudgetItems
        public ActionResult Index()
        {
            var budgetItems = db.BudgetItems.Include(b => b.Budget);
            return View(budgetItems.ToList());
        }

        // GET: BudgetItems
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

                ViewBag.BudgetId = new SelectList(db.Budgets.Where(h => h.HouseholdId == householdId).ToList(), "Id", "Name");
                return View();
            }

            return RedirectToAction("InvalidAttempt", "Home");
        }

        // GET: BudgetItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BudgetItem budgetItem = db.BudgetItems.Find(id);
            if (budgetItem == null)
            {
                return HttpNotFound();
            }
            return View(budgetItem);
        }

        // GET: BudgetItems/Create
        public ActionResult Create()
        {
            ViewBag.BudgetId = new SelectList(db.Budgets, "Id", "Name");
            return View();
        }

        // POST: BudgetItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BudgetId,Name,DesiredAmount")] BudgetItem budgetItem)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                
                budgetHelper.UpdateBudgetIncome(budgetItem.BudgetId, budgetItem.DesiredAmount);
                db.BudgetItems.Add(budgetItem);
                db.SaveChanges();
                return RedirectToAction("IndexMy");
            }

            ViewBag.BudgetId = new SelectList(db.Budgets, "Id", "Name", budgetItem.BudgetId);
            return View(budgetItem);
        }

        // GET: BudgetItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BudgetItem budgetItem = db.BudgetItems.Find(id);
            if (budgetItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.BudgetId = new SelectList(db.Budgets, "Id", "Name", budgetItem.BudgetId);
            return View(budgetItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int BudgetId, string budgetItemName, decimal desiredAmount)
        {
            var budgetItem = db.BudgetItems.Find(BudgetId);
            decimal newBudgetItem = 0;
            budgetItem.Name = budgetItemName;
            if (budgetItem.DesiredAmount == desiredAmount)
            {
                budgetItem.DesiredAmount = desiredAmount;
            }
            else
            {
                newBudgetItem = desiredAmount - budgetItem.DesiredAmount;
                budgetHelper.UpdateBudgetIncome(budgetItem.BudgetId, newBudgetItem);
                budgetItem.DesiredAmount = desiredAmount;
            }

            //etc...
            db.BudgetItems.Attach(budgetItem);
            db.Entry(budgetItem).Property(a => a.Name).IsModified = true;
            db.Entry(budgetItem).Property(a => a.DesiredAmount).IsModified = true;
            //etc... for each property that you want to be able to change in the edit.
            db.SaveChanges();
            return RedirectToAction("IndexMy"); //or whatever.
        }

        // POST: BudgetItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOld([Bind(Include = "Id,Name,DesiredAmount,CurrentAmount,BudgetId")] BudgetItem budgetItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(budgetItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BudgetId = new SelectList(db.Budgets, "Id", "Name", budgetItem.BudgetId);
            return View(budgetItem);
        }

        // GET: BudgetItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BudgetItem budgetItem = db.BudgetItems.Find(id);
            if (budgetItem == null)
            {
                return HttpNotFound();
            }
            return View(budgetItem);
        }

        // POST: BudgetItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BudgetItem budgetItem = db.BudgetItems.Find(id);

            budgetHelper.UpdateBudgetDeleteIncome(budgetItem.BudgetId, budgetItem.DesiredAmount);

            db.BudgetItems.Remove(budgetItem);
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
