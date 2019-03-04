using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LWFinancial.Enumerations;
using LWFinancial.Models;
using LWFinancial.Helpers;
using Microsoft.AspNet.Identity;

namespace LWFinancial.Controllers
{
    public class HouseholdInvitationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private HouseholdsHelper houseHoldHelper = new HouseholdsHelper();

        // GET: HouseholdInvitations
        public ActionResult Index()
        {
            var householdInvitations = db.HouseholdInvitations.Include(h => h.Household).Include(h => h.Recipient).Include(h => h.Sender);
            return View(householdInvitations.ToList());
        }

        // GET: HouseholdInvitations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HouseholdInvitation householdInvitation = db.HouseholdInvitations.Find(id);
            if (householdInvitation == null)
            {
                return HttpNotFound();
            }
            return View(householdInvitation);
        }

        // GET: HouseholdInvitations/Create
        public ActionResult Create()
        {
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name");
            ViewBag.RecipientId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.SenderId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: HouseholdInvitations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Title,Decscription,UniqueKey,Email,Created,Accepted,Expires,Read,HouseholdId,RecipientId,SenderId")] HouseholdInvitation householdInvitation)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.HouseholdInvitations.Add(householdInvitation);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", householdInvitation.HouseholdId);
        //    ViewBag.RecipientId = new SelectList(db.Users, "Id", "FirstName", householdInvitation.RecipientId);
        //    ViewBag.SenderId = new SelectList(db.Users, "Id", "FirstName", householdInvitation.SenderId);
        //    return View(householdInvitation);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string email, string subject, string body)
        {
            var userId = db.Users.Find(User.Identity.GetUserId());

            var invitation = new HouseholdInvitation
            {
                UniqueKey = Guid.NewGuid(),
                Email = email,
                Title = subject,
                Decscription = body,
                Created = DateTime.Now,
                Expires = DateTime.Now.AddDays(7),
                SenderId = userId.Id,
                HouseholdId = userId.HouseholdId.GetValueOrDefault()
            };

            db.HouseholdInvitations.Add(invitation);
            db.SaveChanges();

            var callbackUrl = Url.Action("AcceptInvite", "Households", new { email = invitation.Email, keycode = invitation.UniqueKey, householdId = invitation.HouseholdId }, protocol: Request.Url.Scheme);
            var acceptLink = "You can accept your invitation by clicking <a href=\"" + callbackUrl + "\">here</a>";

            var from = "LWFinancial <LWFinancial@mailinator.com>";
            var emailMessage = new MailMessage(from, email)
            {
                Subject = invitation.Title,
                Body = $"{invitation.Decscription} <hr /><br /> {acceptLink}",
                IsBodyHtml = true
            };

            var svc = new PersonalEmail();
            await svc.SendAsync(emailMessage);

            return RedirectToAction("Details", "Households", new { id = invitation.HouseholdId});
        }

        [Authorize]
        public ActionResult AcceptInvite(string email, string keycode, int householdId)
        {
            if (houseHoldHelper.IsUserInHousehold(User.Identity.GetUserId(), householdId))
            {
                return RedirectToAction("InvalidAttempt", "Home");
            } 

            HouseholdInvitation householdInvitation = db.HouseholdInvitations.Find(householdId);
            var test = householdInvitation.Household;
            return View(householdInvitation.Household);
        }

        public ActionResult AcceptRegister(string houseHoldId)
        {
            return RedirectToAction("Details","Households", new { id = houseHoldId});
        }

        // GET: HouseholdInvitations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HouseholdInvitation householdInvitation = db.HouseholdInvitations.Find(id);
            if (householdInvitation == null)
            {
                return HttpNotFound();
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", householdInvitation.HouseholdId);
            ViewBag.RecipientId = new SelectList(db.Users, "Id", "FirstName", householdInvitation.RecipientId);
            ViewBag.SenderId = new SelectList(db.Users, "Id", "FirstName", householdInvitation.SenderId);
            return View(householdInvitation);
        }

        // POST: HouseholdInvitations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Decscription,UniqueKey,Email,Created,Accepted,Expires,Read,HouseholdId,RecipientId,SenderId")] HouseholdInvitation householdInvitation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(householdInvitation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", householdInvitation.HouseholdId);
            ViewBag.RecipientId = new SelectList(db.Users, "Id", "FirstName", householdInvitation.RecipientId);
            ViewBag.SenderId = new SelectList(db.Users, "Id", "FirstName", householdInvitation.SenderId);
            return View(householdInvitation);
        }

        // GET: HouseholdInvitations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HouseholdInvitation householdInvitation = db.HouseholdInvitations.Find(id);
            if (householdInvitation == null)
            {
                return HttpNotFound();
            }
            return View(householdInvitation);
        }

        // POST: HouseholdInvitations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HouseholdInvitation householdInvitation = db.HouseholdInvitations.Find(id);
            db.HouseholdInvitations.Remove(householdInvitation);
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
