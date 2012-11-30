using System.Web.Mvc;
using Eastwind.Indexes;
using Eastwind.Models;
using System.Linq;
using Raven.Client;

namespace Eastwind.Controllers
{
	public class PolyanaController : RavenController
	{
		public ActionResult Item(string cust)
		{
			var stats = Session.Query<DogsAndCatsByCustomer.Result, DogsAndCatsByCustomer>()
					.Include(x => x.Customer)
					.First(x => x.Customer == cust);

			var customer = Session.Load<Customer>(stats.Customer);

			return Json(new
				{
					customer.Name,
					stats.CatCount,
					stats.DogCount,
				});
		}

		public ActionResult Dog()
		{
			Session.Store(new Dog
				{
					Name = "a"
				});
			return Json(null);
		}

		public ActionResult Cat()
		{
			Session.Store(new Cat
			{
				Name = "a"
			});
			return Json(null);
		}

		public ActionResult LoadDog(int id)
		{
			return Json(Session.Load<Dog>(id));
		}

		public ActionResult LoadCat(int id)
		{
			return Json(Session.Load<Cat>(id));
		}

		public ActionResult LoadAnimal(int id)
		{
			return Json(Session.Load<Animal>(id));
		}

		public ActionResult QueryAnimals(int id)
		{
			return Json(Session.Query<Animal>().ToList());
		}

	}
}