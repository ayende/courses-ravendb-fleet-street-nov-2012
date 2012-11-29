using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eastwind.Models;

namespace Eastwind.Controllers
{
    public class HomeController : RavenController
    {
        public ActionResult Index()
        {
            var users = Session.Query<User>()
                               .Take(3)
                               .ToList();

            return Json(new
                {
                    users.Count,
                    Results = users
                });
        }

        public ActionResult Add()
        {
            var dude = new User
                {
                    Id = string.Format("users/{0}", new Random().NextDouble()),
                    City = "Los Angeles",
                    Company = "ummm....",
                    EmailAddress = "thedude@lebowski.com",
                    Gender = "dude",
                    GivenName = "Jeff",
                    Surname = "Lebowski",
                    Username = "thedude"
                };

            Session.Store(dude);
            return RedirectToAction("Show", new { docId = dude.Id });
        }

        public ActionResult Show(User usr)
        {
            return Json(usr);
        }

        public ActionResult Query(string p, string f)
        {
            var result = new List<User>();
            switch (f)
            {
                case "usr":
                    result = Session.Query<User>().Where(x => x.GivenName == p).ToList();
                    break;
                case "sur":
                    result = Session.Query<User>().Where(x => x.Surname == p).ToList();
                    break;
                case "city":
                    result = Session.Query<User>().Where(x => x.City == p).ToList();
                    break;
                case "zip":
                    int zzip;
                    if (Int32.TryParse(p, out zzip))
                    {
                        result = Session.Query<User>().Where(x => x.ZipCode == zzip).ToList();
                    }
                    break;

                case "state":
                    result = Session.Query<User>().Where(x => x.State== p).ToList();
                    break;

            }
            return Json(new { result.Count, Results = result });
        }


        public ActionResult City(string p)
        {
            var result = Session.Query<User>().Where(x => x.City == p);
            return RedirectToAction("Show", result);
        }

    }
}