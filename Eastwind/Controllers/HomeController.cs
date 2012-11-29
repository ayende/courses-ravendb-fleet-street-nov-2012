using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using Eastwind.Models;
using System;
using Raven.Abstractions.Data;
using Raven.Client;

namespace Eastwind.Controllers
{
    public class HomeController : RavenController
    {
		public ActionResult Measure()
		{
			Session.Store(new User
				{
					ZipCode = 13
				});
			Session.SaveChanges();

			var sp = Stopwatch.StartNew();
			while (DocumentStore.DatabaseCommands.GetStatistics().StaleIndexes.Length > 0)
			{
			}

			return Json(sp.ElapsedMilliseconds);

		}

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

		public ActionResult Create()
		{
			for (int i = 0; i < 10; i++)
			{
				var entity = new User
				{
					GivenName = DateTime.Now.ToString()
				};
				Session.Store(entity);
			}

			return Json("ok");
		}

		public ActionResult Show(string DocID)
		{
			var load = Session.Load<User>(DocID);
			load.NationalID = DateTime.Now.ToString();
			return Json(load);
		}

		public ActionResult Search2(string surname)
		{
			return Json(
				Queryable.Where(Session.Query<User>(), x => x.Surname == surname)
				       .ToList()
				);
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

        public ActionResult Show(object usr)
        {
            return Json(usr);
        }

        public ActionResult Query(string usr, string sur, string city, int? zip, string state)
        {
	        IQueryable<User> q = Session.Query<User>();

	        if (string.IsNullOrEmpty(usr) == false)
		        q = q.Where(x => x.GivenName == usr);
			if (string.IsNullOrEmpty(sur) == false)
				q = q.Where(x => x.Surname == sur);
			if (string.IsNullOrEmpty(city) == false)
				q = q.Where(x => x.City == city);
			if (string.IsNullOrEmpty(state) == false)
				q = q.Where(x => x.State == state);
	        if (zip != null)
		        q = q.Where(x => x.ZipCode == zip.Value);

	        var result = q.ToList();

            return Json(new
	            {
					Query = q.ToString(),
					result.Count, 
					Results = result,
	            });
        }

		public ActionResult AwesomeQuery(string s)
		{
			var q = Session.Query<User>()
			               .Where(x => x.Surname == s);

			var result = q.ToList();

			if (result.Count == 0)
			{
				var suggestions = q.Suggest();

				if (suggestions.Suggestions.Length == 1)
					return AwesomeQuery(suggestions.Suggestions[0]);

				return Json(new
					{
						DidYouMean = suggestions.Suggestions
					});
			}

			return Json(new
			{
				Query = q.ToString(),
				result.Count,
				Results = result,
			});
		}
    }
}