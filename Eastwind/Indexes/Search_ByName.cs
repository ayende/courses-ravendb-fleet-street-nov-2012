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

    public class DogsAndCatsByCustomer : AbstractMultiMapIndexCreationTask<DogsAndCatsByCustomer.Result>
    {
        public class Result
        {
            public string Customer { get; set; }
            public int CatCount { get; set; }
            public int DogCount { get; set; }
        }

        public DogsAndCatsByCustomer()
        {
            AddMap<Dog>(dogs =>
                        from dog in dogs
                        from c in dog.Customers
                        select new { Customer = c, CatCount = 0, DogCount = 1 }
            );

            AddMap<Cat>(cats =>
                    from cat in cats
                    from c in cat.Customers
                    select new { Customer = c, CatCount = 1, DogCount = 0 }
        );

            Reduce = results =>
                from result in results
                group result by result.Customer
                    into g
                    select new
                    {
                        Customer = g.Key,
                        CatCount = g.Sum(x => x.CatCount),
                        DogCount = g.Sum(x => x.DogCount)
                    };
        }
    }
}