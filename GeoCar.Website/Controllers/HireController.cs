using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using GeoCar.Model;
using GeoCar.Database;

namespace GeoCar.Website.Controllers
{
    public class HireController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(string id)
        {
            var hire = HireRepository.RetrieveSingleHire(id);
            hire.HireUser = UserRepository.RetrieveUser(hire.UserId);

            return View("SingleHire", hire);
        }

        public ActionResult NewHireAction(string bookingReference, string customerEmailAddress, string firstName, string surname, int startMileage)
        {
            var user = UserRepository.RetrieveUser(customerEmailAddress);
            if (user == null)
            {
                user = UserRepository.CreateUser(customerEmailAddress, firstName, surname);
            }

            var hire = HireRepository.RetrieveSingleHire(bookingReference);
            if (hire != null)
            {
                return Redirect("/manage/Hire");
            }
            else
            {
                hire = HireRepository.CreateHire(user.UserId, bookingReference, DateTime.MinValue, startMileage);
                return Redirect($"/manage/Hire/Edit/{hire.BookingReference}");
            }
        }
    }
}