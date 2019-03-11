using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LWFinancial.Models;
using LWFinancial.Helpers;
using Microsoft.AspNet.Identity;

namespace LWFinancial.Controllers
{
    [RequireHttps]
    [Authorize]
    public class BudgetsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private HouseholdsHelper householdHelper = new HouseholdsHelper();

        // GET: Budgets
        public ActionResult Index()
        {
            var budgets = db.Budgets.Include(b => b.Household);
            return View(budgets.ToList());
        }

        // GET: Budgets
        public ActionResult IndexMy()
        {
            var userId = User.Identity.GetUserId();
            int userHousehold = householdHelper.ListUserHousehold(userId);

            if (userHousehold == 0)
            {
                return RedirectToAction("InvalidAttempt", "Home");
            }
            else if(householdHelper.IsUserInHousehold(userId, userHousehold))
            {
                return View();
            }

            return RedirectToAction("InvalidAttempt", "Home");
        }

        // GET: Budgets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            return View(budget);
        }

        // GET: Budgets/Create
        public ActionResult Create()
        {
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name");
            return View();
        }

        // POST: Budgets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Decscription,DesiredAmount")] Budget budget, int householdId)
        {
            budget.HouseholdId = householdId;
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                householdHelper.UpdateHouseholdIncome(budget.HouseholdId, budget.DesiredAmount);
                db.Budgets.Add(budget);
                db.SaveChanges();
                return RedirectToAction("IndexMy");
            }

            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", budget.HouseholdId);
            return View(budget);
        }

        // GET: Budgets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", budget.HouseholdId);
            return View(budget);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int budgetId, string budgetName, string description, decimal desiredAmount)
        {
            var budget = db.Budgets.Find(budgetId);
            decimal newBudget = 0;
            budget.Name = budgetName;
            budget.Decscription = description;
            if(budget.DesiredAmount == desiredAmount)
            {
                budget.DesiredAmount = desiredAmount;
            }
            else
            {
                newBudget = desiredAmount - budget.DesiredAmount;
                householdHelper.UpdateHouseholdIncome(budget.HouseholdId, newBudget);
                budget.DesiredAmount = desiredAmount;
            }

            //etc...
            db.Budgets.Attach(budget);
            db.Entry(budget).Property(a => a.Name).IsModified = true;
            db.Entry(budget).Property(a => a.Decscription).IsModified = true;
            db.Entry(budget).Property(a => a.DesiredAmount).IsModified = true;
            //etc... for each property that you want to be able to change in the edit.
            db.SaveChanges();
            return RedirectToAction("IndexMy"); //or whatever.
        }

        // POST: Budgets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOld([Bind(Include = "Id,Name,Decscription,DesiredAmount,CurrentAmount,HouseholdId")] Budget budget)
        {
            if (ModelState.IsValid)
            {
                db.Entry(budget).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", budget.HouseholdId);
            return View(budget);
        }

        // GET: Budgets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            return View(budget);
        }

        // POST: Budgets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int budgetId)
        {
            Budget budget = db.Budgets.Find(budgetId);

            householdHelper.UpdateHouseholdDeleteIncome(budget.HouseholdId, budget.DesiredAmount);

            db.Budgets.Remove(budget);
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
