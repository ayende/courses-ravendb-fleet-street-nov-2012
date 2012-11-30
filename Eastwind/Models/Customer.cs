using System;
using System.Collections.Generic;

namespace Eastwind.Models
{
	public class Customer
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }

		public List<string> Remarks { get; set; }

		public Customer()
		{
			Remarks = new List<string>();
		}
	}

	public class Animal
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public DateTime Birthday { get; set; }
		public List<string> Customers { get; set; }

		public Animal()
		{
			Customers = new List<string>();
		}
	}

	public class Cat : Animal
	{
		public int NumberOfSoulsLeft { get; set; }
	}

	public class Dog : Animal
	{
		public string Breed { get; set; }

		public List<Immunization> Immunizations { get; set; }
		public List<Treatment> Treatments { get; set; }

		public Dog()
		{
			Immunizations = new List<Immunization>();
			Treatments = new List<Treatment>();
		}
	}

	public class Treatment
	{
		public string Description { get; set; }
		public DateTime At { get; set; }
	}

	public class Immunization
	{
		public string Name { get; set; }
		public List<DateTime> At { get; set; }
	}
}