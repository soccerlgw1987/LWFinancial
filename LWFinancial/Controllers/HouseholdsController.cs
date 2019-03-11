using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LWFinancial.Enumerations;
using LWFinancial.Helpers;
using LWFinancial.Models;
using Microsoft.AspNet.Identity;

namespace LWFinancial.Controllers
{
    [RequireHttps]
    public class HouseholdsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private HouseholdsHelper houseHelper = new HouseholdsHelper();

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

        [Authorize]
        public ActionResult AcceptInvite(string email, string keycode, int householdId)
        {
            if (houseHelper.IsUserInHousehold(User.Identity.GetUserId(), householdId))
            {
                return RedirectToAction("InvalidAttempt", "Home");
            }

            Household household = db.Households.Find(householdId);
            return View(household);
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

        // Post: Households/Leave/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LeaveAsync(string userId, int householdId)
        {
            Household household = db.Households.Find(householdId);

            if (User.IsInRole(RoleNames.HOH.ToString()))
            {
                foreach (var item in household.ApplicationUsers)
                {
                    houseHelper.RemoveUserFromHousehold(item.Id, householdId);
                    if(roleHelper.IsUserInRole(item.Id, RoleNames.HOH))
                    {
                        roleHelper.RemoveUserFromRole(item.Id, RoleNames.HOH.ToString());
                    }
                    else
                    {
                        roleHelper.RemoveUserFromRole(item.Id, RoleNames.Member.ToString());
                    }
                    roleHelper.AddUserToRole(item.Id, RoleNames.Guest);
                }

                await AuthorizationHelper.ReauthorizeUserAsync(userId);

                db.Households.Remove(household);
                db.SaveChanges();
            }
            else
            {
                houseHelper.RemoveUserFromHousehold(userId, householdId);
                roleHelper.RemoveUserFromRole(userId, RoleNames.Member.ToString());
                roleHelper.AddUserToRole(userId, RoleNames.Guest);

                await AuthorizationHelper.ReauthorizeUserAsync(userId);
                db.SaveChanges();
            }

            return RedirectToAction("Index", "Households");
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
        public async Task<ActionResult> CreateAsnyc([Bind(Include = "Id,Name,Decscription,AvatarPath,IncomeAmount")] Household household, HttpPostedFileBase image)
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
                roleHelper.RemoveUserFromRole(userId, RoleNames.Guest.ToString());
                roleHelper.AddUserToRole(userId, RoleNames.HOH);

                await AuthorizationHelper.ReauthorizeUserAsync(userId);

                db.SaveChanges();

                return RedirectToAction("Details", "Households", new { id = user.HouseholdId });
            }

            return View(household);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AcceptAsync([Bind(Include = "Id,Name,Decscription,AvatarPath,IncomeAmount,Created,Updated")] Household household)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var user = db.Users.Find(userId);
                user.HouseholdId = household.Id;
                db.SaveChanges();

                roleHelper.RemoveUserFromRole(userId, RoleNames.Guest.ToString());
                roleHelper.AddUserToRole(userId, RoleNames.Member);

                await AuthorizationHelper.ReauthorizeUserAsync(userId);

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
        public ActionResult Edit([Bind(Include = "Id,Name,Decscription,AvatarPath,Created,Updated,IncomeAmount,CurrentBudgetAmount")] Household household, HttpPostedFileBase image)
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
        public ActionResult DeleteConfirmed(string userId, int householdId)
        {
            if (User.IsInRole(RoleNames.HOH.ToString()))
            {
                foreach (var item in houseHelper.UsersInHousehold(householdId))
                {
                    houseHelper.RemoveUserFromHousehold(item.Id, householdId);
                    roleHelper.RemoveUserFromRole(item.Id, RoleNames.Member.ToString());
                    roleHelper.AddUserToRole(item.Id, RoleNames.Guest);
                }
            }
            else
            {
                houseHelper.RemoveUserFromHousehold(userId, householdId);
                roleHelper.RemoveUserFromRole(userId, RoleNames.Member.ToString());
                roleHelper.AddUserToRole(userId, RoleNames.Guest);
            }

            Household household = db.Households.Find(householdId);
            db.Households.Remove(household);
            db.SaveChanges();
            return RedirectToAction("Index", "Households");
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
