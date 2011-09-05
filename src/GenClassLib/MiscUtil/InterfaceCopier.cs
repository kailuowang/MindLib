using System;
using MindHarbor.GenInterfaces;

namespace MindHarbor.GenClassLib.MiscUtil {
	public class InterfaceCopier {
		public static void CopyIAddress(IAddress from, IAddressM to) {
			to.Name = from.Name;
			to.Street = from.Street;
			to.Street2 = from.Street2;
			to.State = from.State;
			to.City = from.City;
			to.Zip = from.Zip;
			to.Country = from.Country;
		}

		public static void CopyICreditCardInfo(ICreditCardInfo from, ICreditCardInfoM to) {
			to.CCNum = from.CCNum;
			to.CVV = from.CVV;
			to.ExpDate = from.ExpDate;
			to.HolderName = from.HolderName;
			to.Type = from.Type;
		}

		/// <summary>
		/// this is a deep copy
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		public static void CopyIContactInfo(IContactInfo from, IContactInfoM to) {
			if (from == null)
				throw new ArgumentNullException("the from is null");
			if (to == null)
				throw new ArgumentNullException("the to is null");
			if (from.Address == null)
				to.Address = null;
			else if (to.Address == null)
				throw new ArgumentNullException("the to.Address is null");
			else
				CopyIAddress(from.Address, to.Address);

			to.Company = from.Company;
			to.Email = from.Email;
			to.Fax = from.Fax;
			to.FirstName = from.Email;
			to.LastName = from.LastName;
			to.Phone = from.Phone;
		}
	}
}