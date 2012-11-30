using System.Linq;
using Eastwind.Models;
using Raven.Client.Indexes;

namespace Eastwind.Indexes
{
	public class Search_ByName : AbstractMultiMapIndexCreationTask
	{
		public Search_ByName()
		{
			AddMap<Dog>(dogs =>
						from dog in dogs
						select new { dog.Name }
			);

			AddMap<Customer>(customers =>
							 from customer in customers
							 select new { customer.Name }
			);

		}
	}

	public class Dogs_ByCustomer : AbstractIndexCreationTask<Dog, Dogs_ByCustomer.Result>
	{
		public class Result
		{
			public string Customer { get; set; }
			public int Count { get; set; }
		}
		public Dogs_ByCustomer()
		{
			Map = dogs =>
			      from dog in dogs
				  from c in dog.Customers
				  select new { Customer = c, Count = 1 };

			Reduce = results =>
			         from result in results
			         group result by result.Customer
			         into g
			         select new
				         {
							 Customer = g.Key,
							 Count = g.Sum(x=>x.Count)
				         };
		}
	}
}