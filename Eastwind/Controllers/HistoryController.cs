using System.Web.Mvc;
using Eastwind.Models;
using Raven.Client.Bundles.Versioning;

namespace Eastwind.Controllers
{
	public class HistoryController : RavenController
	{
		 public ActionResult Reset(int id, int revision)
		 {
			 var jsonDocument = DocumentStore.DatabaseCommands
				 .Get("orders/" + id + "/revisions/" + revision);
			 jsonDocument.Metadata.Remove("Raven-Document-Revision-Status");
			 jsonDocument.Metadata.Remove("Raven-Document-Parent-Revision");
			 jsonDocument.Metadata.Remove("Raven-Read-Only");
			 DocumentStore.DatabaseCommands.Put("orders/" + id, null,
			                                    jsonDocument.DataAsJson,
			                                    jsonDocument.Metadata);

			 return Json("Done");
		 }
	}
}