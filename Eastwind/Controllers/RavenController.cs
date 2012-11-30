using System;
using System.Web.Mvc;
using Eastwind.Indexes;
using Eastwind.Models;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client.Listeners;
using Raven.Imports.Newtonsoft.Json;
using Raven.Json.Linq;

namespace Eastwind.Controllers
{
	public class RavenController : Controller
	{
		private static readonly Lazy<IDocumentStore> lazyDocStore =
			new Lazy<IDocumentStore>(() =>
				{
					var docStore = new DocumentStore
						{
							Url = "http://localhost:8080",
							DefaultDatabase = "Voms"
						};
					var ordersHiLoGenerator = new HiLoKeyGenerator("orders", 32);
					docStore.Conventions.RegisterIdConvention<Order>((commands, order) =>
						{
							var generateDocumentKey = ordersHiLoGenerator
								.GenerateDocumentKey(commands, docStore.Conventions, order);

							return order.Customer + "/" + generateDocumentKey;
						});
					docStore.RegisterListener(new ValidationListener());
					docStore.Initialize();

					//docStore.Conventions.FindTypeTagName = type =>
					//    {
					//        if (typeof (Animal).IsAssignableFrom(type))
					//            return "Animals";
					//        return DocumentConvention.DefaultTypeTagName(type);
					//    };

					IndexCreation.CreateIndexes(typeof(Search_ByName).Assembly, docStore);

					return docStore;
				});

		public static IDocumentStore DocumentStore
		{
			get { return lazyDocStore.Value; }
		}

		public new IDocumentSession Session { get; set; }

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			Session = DocumentStore.OpenSession();
			Session.Advanced.UseOptimisticConcurrency = true;
		}

		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			using (Session)
			{
				if (Session == null)
					return;
				if (filterContext.Exception != null)
					return;
				Session.SaveChanges();
			}
		}

		protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
		{
			return base.Json(data, contentType, contentEncoding, JsonRequestBehavior.AllowGet);
		}
	}

	public class ValidationListener : IDocumentStoreListener
	{
		public bool BeforeStore(string key, object entityInstance, RavenJObject metadata, RavenJObject original)
		{
			// do validation
			return false;
		}

		public void AfterStore(string key, object entityInstance, RavenJObject metadata)
		{
		}
	}
}