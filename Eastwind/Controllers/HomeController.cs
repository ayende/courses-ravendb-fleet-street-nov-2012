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
	}
}