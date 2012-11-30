using System;
using System.Web.Mvc;
using Eastwind.Models;
using Raven.Abstractions.Data;
using Raven.Json.Linq;
using System.Linq;

namespace Eastwind.Controllers
{
	public class VetController : RavenController
	{

		public ActionResult Update(int id)
		{
			var customer = Session.Load<Customer>(id);
			customer.Remarks.Add(null);
			return Json("");
		}

		public ActionResult Name(int id)
		{
			var customer = Session.Load<Customer>(id);
			return Json(new
				{
					Name = customer.Name,
					Etag = Session.Advanced.GetEtagFor(customer)
				});
		}

		public ActionResult Update2(int id, string name, Guid etag)
		{
			var customer = Session.Load<Customer>(id);
			Session.Store(customer, etag);
			customer.Name = name;
			return Json("");
		}

		public ActionResult Register()
		{
			var customer = new Customer
				{
					Email = "ayende@ayende.com",
					Name = "Oren",
					Phone = "0123",
				};

			Session.Store(customer);

            var daphne = new Cat
            {
                Customers = { customer.Id },
                Name = "Daphne",
                Birthday = new DateTime(2010, 1, 1),
            };

			var oscar = new Dog
				{
					Customers = {customer.Id},
					Name = "Oscar",
					Birthday = new DateTime(2006, 1, 1),
					Breed = "Mixed - Small"
				};

			var arava = new Dog
			{
				Customers = {Session.Advanced.GetDocumentId(customer)},
				Name = "Arava",
				Birthday = new DateTime(2009, 8, 2),
				Breed = "German Shepherd"
			};

            Session.Store(daphne);
			Session.Store(oscar);
			Session.Store(arava);

			return Json("OK");
		}

		public ActionResult Strange(int id)
		{
			var dog1 = Session.Load<Dog>(id); 
			var dog2 = Session.Load<Dog>(id);


			dog1.Breed = "Foo";
			dog2.Immunizations.Add(new Immunization
				{
					Name = "bar"
				});

			return Json("What is going on ?");
		}

		public ActionResult Dog(int id)
		{
			var dog = Session
				.Include<Dog>(x=>x.Customers)
				.Load(id);

			if (dog == null)
				return Json(new {NoSuchBeast = true});

			return Json(new
				{
					Owners = dog.Customers.Select(cust=>Session.Load<Customer>(cust)).ToArray(),
					Dog = dog
				});
		}

		//public ActionResult Customer(int id)
		//{
			
		//}

		public ActionResult CreateOrder(string customer)
		{
			var order = new Order
				{
					Customer = customer
				};

			order.AddLine(
				new OrderLine {Name = "Shot", Quantity = 1, Price = 293}
				);

			Session.Store(order);

			return Json("OK");
		}

		public ActionResult UpdateQty(int id, int qty, int lineId)
		{
			var order = Session.Load<Order>(id);

			order.Lines.First(x => x.Id == lineId).Quantity = qty;

			return Json("OK");
		}
	}
}