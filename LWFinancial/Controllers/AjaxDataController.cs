using LWFinancial.Enumerations;
using LWFinancial.Helpers;
using LWFinancial.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LWFinancial.Controllers
{
    [Authorize]
    public class AjaxDataController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private HouseholdsHelper householdHelper = new HouseholdsHelper();

        public JsonResult GetSideBarChartData()
        {
            var dataChart = new List<DoubleChart>();
            Household house = db.Households.Find(householdHelper.ListUserHousehold(User.Identity.GetUserId()));

            foreach (var item in house.Accounts)
            {
                dataChart.Add(new DoubleChart
                {
                    y = item.Name,
                    a = item.Transactions.Where(t => t.Type == TransactionType.Withdrawl).Count(),
                    b = item.Transactions.Where(t => t.Type == TransactionType.Deposit).Count()
                });
            }

            return Json(dataChart);
        }
    }
}