using LWFinancial.Models;
using LWFinancial.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LWFinancial.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult InvalidAttempt()
        {
            return View();
        }

        public ActionResult TheLobby()
        {
            var guests = db.Users.Where(u => u.HouseholdId == null).
                                  Select(guestUser => new GuestUser
                                  {
                                      Name = guestUser.FirstName + " " + guestUser.LastName,
                                      Email = guestUser.Email,
                                      PhoneNumber = guestUser.PhoneNumber,
                                      AvatarPath = ""
                                  }).ToList();

            return View(guests);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            EmailModel model = new EmailModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var body = "<p>Email from: <bold>{0}</bold>({1})</p><p>Message:</p><p>{2}</p>";
                    var emailTo = ConfigurationManager.AppSettings["emailTo"];
                    var from = string.Format("LWFinancial<{0}>", emailTo);
                    model.Body = model.Body;
                    var htmlBody = new MvcHtmlString(model.Body);
                    var email = new MailMessage(from, ConfigurationManager.AppSettings["emailto"])
                    {
                        Subject = model.Subject,
                        Body = string.Format(body, model.FromEmail, model.FromEmail, htmlBody),
                        IsBodyHtml = true
                    };

                    var svc = new PersonalEmail();
                    await svc.SendAsync(email);

                    return View(new EmailModel());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }
            }
            return View(model);
        }
    }
}