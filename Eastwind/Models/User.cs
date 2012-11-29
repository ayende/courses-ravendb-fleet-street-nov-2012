namespace Eastwind.Models
{
	public class User
	{
		public string Id { get; set; }
		public string Gender { get; set; }
		public string GivenName { get; set; }
		public string Surname { get; set; }
		public string StreetAddress { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public int ZipCode { get; set; }
		public string EmailAddress { get; set; }
		public string Username { get; set; }
		public string NationalID { get; set; }
		public string Occupation { get; set; }
		public string Company { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
	}
}