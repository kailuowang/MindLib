using MindHarbor.GenInterfaces;

namespace MindHarbor.GenClassLib.Test.MiscUtilTests {
	public class Address : IAddressM {
		private string m_City;
		private string m_Country;
		private string m_Name;
		private string m_State;
		private string m_Street;
		private string m_Street2;
		private string m_Zip;

		#region IAddressM Members

		public string City {
			get { return m_City; }
			set { m_City = value; }
		}

		public string Country {
			get { return m_Country; }
			set { m_Country = value; }
		}

		public string Name {
			get { return m_Name; }
			set { m_Name = value; }
		}

		public string State {
			get { return m_State; }
			set { m_State = value; }
		}

		public string Street {
			get { return m_Street; }
			set { m_Street = value; }
		}

		public string Street2 {
			get { return m_Street2; }
			set { m_Street2 = value; }
		}

		public string Zip {
			get { return m_Zip; }
			set { m_Zip = value; }
		}

		#endregion
	}

	public class ContactInfo : IContactInfoM {
		private IAddressM address;
		private string company;
		private string email;
		private string fax;
		private string firstName;
		private string lastName;
		private string phone;
		private string phoneExt;
		private string title;

		public string Title {
			get { return title; }
			set { title = value; }
		}

		public string PhoneExt {
			get { return phoneExt; }
			set { phoneExt = value; }
		}

		#region IContactInfoM Members

		public IAddressM Address {
			get { return address; }
			set { address = value; }
		}

		IAddress IContactInfo.Address {
			get { return address; }
		}

		public string FirstName {
			get { return firstName; }
			set { firstName = value; }
		}

		public string LastName {
			get { return lastName; }
			set { lastName = value; }
		}

		public string Email {
			get { return email; }
			set { email = value; }
		}

		public string Phone {
			get { return phone; }
			set { phone = value; }
		}

		public string Fax {
			get { return fax; }
			set { fax = value; }
		}

		public string Company {
			get { return company; }
			set { company = value; }
		}

		#endregion
	}
}