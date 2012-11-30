using System.Collections.Generic;
using Raven.Imports.Newtonsoft.Json;

namespace Eastwind.Models
{
	public class Order
	{
		public string Id { get; set; }
		public string Customer { get; set; }
		public List<OrderLine> Lines { get; set; }
		public int LastOrderLineId { get; set; }

		public Order()
		{
			Lines = new List<OrderLine>();
		}

		public int AllocateOrderLineId()
		{
			return ++LastOrderLineId;
		}

		public void AddLine(OrderLine orderLine)
		{
			orderLine.Id = AllocateOrderLineId();
			Lines.Add(orderLine);
		}
	}

	public class OrderLine
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
	}
}