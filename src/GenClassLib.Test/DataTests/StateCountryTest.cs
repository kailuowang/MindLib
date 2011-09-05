using System;
using MindHarbor.GenClassLib.Data;
using NUnit.Framework;

namespace MindHarbor.GenClassLib.Test.DataTests {
	[TestFixture]
	public class StateCountryTest {
		[Test]
		public void StatesTest() {
			Assert.AreEqual(StatesEnum.Wisconsin.Abbr, "WI");
			Assert.AreEqual(StatesEnum.Parse("North Carolina"), StatesEnum.NorthCarolina);
			Assert.AreEqual(StatesEnum.Parse("NY"), StatesEnum.NewYork);
			foreach (StatesEnum state in StatesEnum.GetAllStates())
				Console.WriteLine(state.Name + "   " + state.Abbr);
		}
	}
}