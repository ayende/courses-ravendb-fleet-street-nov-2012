using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using Eastwind.Models;
using System;

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

		public ActionResult Search(string surname)
		{
			return Json(
				Session.Query<User>()
				       .Where(x => x.Surname == surname)
				       .ToList()
				);
		}
	}
}