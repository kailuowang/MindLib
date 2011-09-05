using System;

namespace MindHarbor.GenInterfaces {
	public interface ICreditCardInfo {
		long CCNum { get; }
		string HolderName { get; }

		DateTime ExpDate { get; }
		int CVV { get; }
		CreditCardType Type { get; }
	    bool CreditCardIsExpired();
	}

	/// <summary>
	/// mutable ICreditCardInfo
	/// </summary>
	public interface ICreditCardInfoM : ICreditCardInfo {
		new long CCNum { get; set; }
		new string HolderName { get; set; }
		new DateTime ExpDate { get; set; }
		new int CVV { get; set; }
		new CreditCardType Type { get; set; }
	}

	public enum CreditCardType {
		AmericanExpress = 1,
		Discover = 2,
		MasterCard = 4,
		Visa = 8
	}
}