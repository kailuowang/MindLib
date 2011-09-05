namespace MindHarbor.GenInterfaces {
	public interface IContactInfo {
		IAddress Address { get; }
		string FirstName { get; }
		string LastName { get; }
		string Company { get; }
		string Phone { get; }
		string Fax { get; }
		string Email { get; }
	}

	public interface IContactInfoM : IContactInfo {
		new IAddressM Address { get; set; }
		new string FirstName { get; set; }
		new string LastName { get; set; }
		new string Company { get; set; }
		new string Phone { get; set; }
		new string Fax { get; set; }
		new string Email { get; set; }
	}
}