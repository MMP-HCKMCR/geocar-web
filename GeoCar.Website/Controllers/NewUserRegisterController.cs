using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using GeoCar.Model;
using GeoCar.Database;

namespace GeoCar.Website.Controllers
{
    public class NewUserRegisterController : Controller
    {
        public class Model
        {
            public bool NoBookingFound;
        }

        public ActionResult Index(bool notFound = false)
        {
            return View("Index", model: new Model { NoBookingFound = notFound });
        }

        public ActionResult Action(string bookingReference, string emailAddress, string userPassword)
        {
            var hire = HireRepository.RetrieveSingleHire(bookingReference, emailAddress);
            if (hire == null)
            {
                return Redirect("/manage/NewUserRegister?notFound=true");
            }

            var user = UserRepository.UpdatePassword(hire.UserId, userPassword);
            return Redirect("/manage/NewUserRegister/Success");
        }

        public ActionResult Success()
        {
            return View();
        }
    }
}