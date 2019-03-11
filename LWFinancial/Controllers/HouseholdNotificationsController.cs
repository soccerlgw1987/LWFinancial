using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LWFinancial.Models;

namespace LWFinancial.Controllers
{
    [RequireHttps]
    [Authorize]
    public class HouseholdNotificationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: HouseholdNotifications
        public ActionResult Index()
        {
            var householdNotifications = db.HouseholdNotifications.Include(h => h.Household).Include(h => h.Recipient);
            return View(householdNotifications.ToList());
        }

        // GET: HouseholdNotifications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HouseholdNotification householdNotification = db.HouseholdNotifications.Find(id);
            if (householdNotification == null)
            {
                return HttpNotFound();
            }
            return View(householdNotification);
        }

        // GET: HouseholdNotifications/Create
        public ActionResult Create()
        {
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name");
            ViewBag.RecipientId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: HouseholdNotifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Decscription,Created,Read,HouseholdId,RecipientId")] HouseholdNotification householdNotification)
        {
            if (ModelState.IsValid)
            {
                db.HouseholdNotifications.Add(householdNotification);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", householdNotification.HouseholdId);
            ViewBag.RecipientId = new SelectList(db.Users, "Id", "FirstName", householdNotification.RecipientId);
            return View(householdNotification);
        }

        // GET: HouseholdNotifications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HouseholdNotification householdNotification = db.HouseholdNotifications.Find(id);
            if (householdNotification == null)
            {
                return HttpNotFound();
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", householdNotification.HouseholdId);
            ViewBag.RecipientId = new SelectList(db.Users, "Id", "FirstName", householdNotification.RecipientId);
            return View(householdNotification);
        }

        // POST: HouseholdNotifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Decscription,Created,Read,HouseholdId,RecipientId")] HouseholdNotification householdNotification, bool notificationRead)
        {
            if (ModelState.IsValid)
            {
                householdNotification.Read = notificationRead;
                db.Entry(householdNotification).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", householdNotification.HouseholdId);
            ViewBag.RecipientId = new SelectList(db.Users, "Id", "FirstName", householdNotification.RecipientId);
            return View(householdNotification);
        }

        // GET: HouseholdNotifications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HouseholdNotification householdNotification = db.HouseholdNotifications.Find(id);
            if (householdNotification == null)
            {
                return HttpNotFound();
            }
            return View(householdNotification);
        }

        // POST: HouseholdNotifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HouseholdNotification householdNotification = db.HouseholdNotifications.Find(id);
            db.HouseholdNotifications.Remove(householdNotification);
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
