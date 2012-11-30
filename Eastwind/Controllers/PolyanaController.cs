using System.Web.Mvc;
using Eastwind.Models;
using System.Linq;

namespace Eastwind.Controllers
{
	public class PolyanaController : RavenController
	{
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