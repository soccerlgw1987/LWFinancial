using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
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
    public class HouseholdsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();

        // GET: Households
        public ActionResult Index()
        {
            return View(db.Households.ToList());
        }

        public ActionResult Dashboard()
        {
            var userId = User.Identity.GetUserId();
            var householdId = db.Users.Find(userId).HouseholdId;
            var house = db.Households.Find(householdId);

            return View(house);
        }

        public ActionResult PrepareInvitation(int houseId)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GenerateInvitation(int houseId)
        {
            return RedirectToAction("Details", "Households", new { id = houseId });
        }


        // GET: Households/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // GET: Households/Create
        public ActionResult Create()
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            if(user.HouseholdId == null)
            {
                return View();
            }

            return RedirectToAction("InvalidAttempt", "Home");
        }

        // POST: Households/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Decscription,AvatarPath,IncomeAmount")] Household household, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (FileUploadValidator.IsWebFriendlyImage(image))
                {
                    var fileName = Path.GetFileName(image.FileName);
                    image.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), fileName));
                    household.AvatarPath = "/Uploads/" + fileName;
                }
                household.Created = DateTime.Now;
                db.Households.Add(household);
                db.SaveChanges();

                var userId = User.Identity.GetUserId();
                var user = db.Users.Find(userId);
                user.HouseholdId = household.Id;
                roleHelper.AddUserToRole(userId, RoleNames.HOH);

                db.SaveChanges();

                return RedirectToAction("Details", "Households", new { id = user.HouseholdId });
            }

            return View(household);
        }

        // GET: Households/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Decscription,AvatarPath,Created,Updated,IncomeAmount")] Household household, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (FileUploadValidator.IsWebFriendlyImage(image))
                {
                    var fileName = Path.GetFileName(image.FileName);
                    image.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), fileName));
                    household.AvatarPath = "/Uploads/" + fileName;
                }

                household.Updated = DateTime.Now;
                db.Entry(household).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = household.Id});
            }
            return View(household);
        }

        // GET: Households/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Household household = db.Households.Find(id);
            db.Households.Remove(household);
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
