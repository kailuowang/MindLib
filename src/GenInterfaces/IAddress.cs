namespace MindHarbor.GenInterfaces {
	public interface IAddress {
		string Name { get; }

		string Street { get; }

		string Street2 { get; }

		string City { get; }

		string State { get; }

		string Zip { get; }

		string Country { get; }
	}

	/// <summary>
	/// Mutable IAddress
	/// </summary>
	public interface IAddressM : IAddress {
		new string Name { get; set; }

		new string Street { get; set; }

		new string Street2 { get; set; }

		new string City { get; set; }

		new string State { get; set; }

		new string Zip { get; set; }

		new string Country { get; set; }
	}
}