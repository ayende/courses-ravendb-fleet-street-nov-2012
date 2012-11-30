using System.Linq;
using Eastwind.Models;
using Raven.Client.Indexes;

namespace Eastwind.Indexes
{
	public class DogsAndCatsByCustomer : AbstractMultiMapIndexCreationTask<DogsAndCatsByCustomer.FinalResult>
	{
		public class Result
		{
			public string Customer { get; set; }
			public int CatCount { get; set; }
			public int DogCount { get; set; }
		}

		public class FinalResult
		{
			public string CustomerId { get; set; }
			public string CustomerName { get; set; }
			public int CatCount { get; set; }
			public int DogCount { get; set; }
		
		}

		public DogsAndCatsByCustomer()
		{
			AddMap<Dog>(dogs =>
			            from dog in dogs
			            from c in dog.Customers
			            select new
				            {
					            CustomerId = c,
					            CatCount = 0,
					            DogCount = 1,
					            CustomerName = (string) null
				            });

			AddMap<Cat>(cats =>
			            from cat in cats
			            from c in cat.Customers
			            select new
				            {
					            CustomerId = c, 
								CatCount = 1, 
								DogCount = 0, 
								CustomerName = (string) null
				            });

			AddMap<Customer>(customers =>
			                 from customer in customers
			                 select new
				                 {
									 CustomerName = customer.Name,
									 CustomerId = customer.Id,
									 CatCount = 0,
									 DogCount = 0
				                 });

			Reduce = results =>
			         from result in results
			         group result by result.CustomerId
			         into g
			         select new
				         {
					         CustomerId = g.Key,
					         CatCount = g.Sum(x => x.CatCount),
					         DogCount = g.Sum(x => x.DogCount),
							 g.FirstOrDefault(x=>x.CustomerName != null).CustomerName
				         };


		}
	}
}