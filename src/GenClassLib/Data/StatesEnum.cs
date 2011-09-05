using System;
using System.Collections.Generic;

namespace MindHarbor.GenClassLib.Data {
	/// <summary>
	/// Type safe enumberation of the states in the U.S.
	/// </summary>
	public sealed class StatesEnum {
		private static readonly object lockObj = new object();
		private static ICollection<StatesEnum> states = new List<StatesEnum>();
		private string abbr;
		private string name;

		private StatesEnum(string abbr, string name) {
			this.name = name;
			this.abbr = abbr;
		}

		#region states initialization

		public static readonly StatesEnum Alabama = new StatesEnum("AL", "Alabama");
		public static readonly StatesEnum Alaska = new StatesEnum("AK", "Alaska");
		public static readonly StatesEnum Arizona = new StatesEnum("AZ", "Arizona");
		public static readonly StatesEnum Arkansas = new StatesEnum("AR", "Arkansas");
		public static readonly StatesEnum California = new StatesEnum("CA", "California");
		public static readonly StatesEnum Colorado = new StatesEnum("CO", "Colorado");
		public static readonly StatesEnum Connecticut = new StatesEnum("CT", "Connecticut");
		public static readonly StatesEnum DC = new StatesEnum("DC", "D.C.");
		public static readonly StatesEnum Delaware = new StatesEnum("DE", "Delaware");
		public static readonly StatesEnum Florida = new StatesEnum("FL", "Florida");
		public static readonly StatesEnum Georgia = new StatesEnum("GA", "Georgia");
		public static readonly StatesEnum Hawaii = new StatesEnum("HI", "Hawaii");
		public static readonly StatesEnum Idaho = new StatesEnum("ID", "Idaho");
		public static readonly StatesEnum Illinois = new StatesEnum("IL", "Illinois");
		public static readonly StatesEnum Indiana = new StatesEnum("IN", "Indiana");
		public static readonly StatesEnum Iowa = new StatesEnum("IA", "Iowa");
		public static readonly StatesEnum Kansas = new StatesEnum("KS", "Kansas");
		public static readonly StatesEnum Kentucky = new StatesEnum("KY", "Kentucky");
		public static readonly StatesEnum Louisiana = new StatesEnum("LA", "Louisiana");
		public static readonly StatesEnum Maine = new StatesEnum("ME", "Maine");
		public static readonly StatesEnum Maryland = new StatesEnum("MD", "Maryland");
		public static readonly StatesEnum Massachusetts = new StatesEnum("MA", "Massachusetts");
		public static readonly StatesEnum Michigan = new StatesEnum("MI", "Michigan");
		public static readonly StatesEnum Minnesota = new StatesEnum("MN", "Minnesota");
		public static readonly StatesEnum Mississippi = new StatesEnum("MS", "Mississippi");
		public static readonly StatesEnum Missouri = new StatesEnum("MO", "Missouri");
		public static readonly StatesEnum Montana = new StatesEnum("MT", "Montana");
		public static readonly StatesEnum Nebraska = new StatesEnum("NE", "Nebraska");
		public static readonly StatesEnum Nevada = new StatesEnum("NV", "Nevada");
		public static readonly StatesEnum NewHampshire = new StatesEnum("NH", "New Hampshire");
		public static readonly StatesEnum NewJersey = new StatesEnum("NJ", "New Jersey");
		public static readonly StatesEnum NewMexico = new StatesEnum("NM", "New Mexico");
		public static readonly StatesEnum NewYork = new StatesEnum("NY", "New York");
		public static readonly StatesEnum NorthCarolina = new StatesEnum("NC", "North Carolina");
		public static readonly StatesEnum NorthDakota = new StatesEnum("ND", "North Dakota");
		public static readonly StatesEnum Ohio = new StatesEnum("OH", "Ohio");
		public static readonly StatesEnum Oklahoma = new StatesEnum("OK", "Oklahoma");
		public static readonly StatesEnum Oregon = new StatesEnum("OR", "Oregon");
		public static readonly StatesEnum Pennsylvania = new StatesEnum("PA", "Pennsylvania");
		public static readonly StatesEnum RhodeIsland = new StatesEnum("RI", "Rhode Island");
		public static readonly StatesEnum SouthCarolina = new StatesEnum("SC", "South Carolina");
		public static readonly StatesEnum SouthDakota = new StatesEnum("SD", "South Dakota");
		public static readonly StatesEnum Tennessee = new StatesEnum("TN", "Tennessee");
		public static readonly StatesEnum Texas = new StatesEnum("TX", "Texas");
		public static readonly StatesEnum Utah = new StatesEnum("UT", "Utah");
		public static readonly StatesEnum Vermont = new StatesEnum("VT", "Vermont");
		public static readonly StatesEnum Virginia = new StatesEnum("VA", "Virginia");
		public static readonly StatesEnum Washington = new StatesEnum("WA", "Washington");
		public static readonly StatesEnum WestVirginia = new StatesEnum("WV", "West Virginia");
		public static readonly StatesEnum Wisconsin = new StatesEnum("WI", "Wisconsin");
		public static readonly StatesEnum Wyoming = new StatesEnum("WY", "Wyoming");

		static StatesEnum() {
			InitializeStates();
		}

		private static void InitializeStates() {
			lock (lockObj) {
				if (states.Count > 0) return;
				states.Add(Alabama);
				states.Add(Alaska);
				states.Add(Arizona);
				states.Add(Arkansas);
				states.Add(California);
				states.Add(Colorado);
				states.Add(Connecticut);
				states.Add(DC);
				states.Add(Delaware);
				states.Add(Florida);
				states.Add(Georgia);
				states.Add(Hawaii);
				states.Add(Idaho);
				states.Add(Illinois);
				states.Add(Indiana);
				states.Add(Iowa);
				states.Add(Kansas);
				states.Add(Kentucky);
				states.Add(Louisiana);
				states.Add(Maine);
				states.Add(Maryland);
				states.Add(Massachusetts);
				states.Add(Michigan);
				states.Add(Minnesota);
				states.Add(Mississippi);
				states.Add(Missouri);
				states.Add(Montana);
				states.Add(Nebraska);
				states.Add(Nevada);
				states.Add(NewHampshire);
				states.Add(NewJersey);
				states.Add(NewMexico);
				states.Add(NewYork);
				states.Add(NorthCarolina);
				states.Add(NorthDakota);
				states.Add(Ohio);
				states.Add(Oklahoma);
				states.Add(Oregon);
				states.Add(Pennsylvania);
				states.Add(RhodeIsland);
				states.Add(SouthCarolina);
				states.Add(SouthDakota);
				states.Add(Tennessee);
				states.Add(Texas);
				states.Add(Utah);
				states.Add(Vermont);
				states.Add(Virginia);
				states.Add(Washington);
				states.Add(WestVirginia);
				states.Add(Wisconsin);
				states.Add(Wyoming);
			}
		}

		#endregion

		public string Name {
			get { return name; }
		}

		public string Abbr {
			get { return abbr; }
		}

		public static IEnumerable<StatesEnum> GetAllStates() {
			if (states.Count == 0) InitializeStates();
			return states;
		}

		/// <summary>
		/// parse the state by a string, could be either a name or an abbrievation
		/// </summary>
		/// <param name="nameOrAbbr"></param>
		/// <returns>the state found or null if no such state is found</returns>
		public static StatesEnum Parse(string nameOrAbbr) {
			if (nameOrAbbr.Length < 2) throw new ArgumentException("Invalid name or abbr");
			if (nameOrAbbr.Length == 2)
				return FindStateByAbbr(nameOrAbbr);
			else
				return FindStateByName(nameOrAbbr);
		}

		private static StatesEnum FindStateByName(string name) {
			foreach (StatesEnum state in GetAllStates())
				if (state.Name == name)
					return state;
			return null;
		}

		private static StatesEnum FindStateByAbbr(string abbr) {
			foreach (StatesEnum state in GetAllStates())
				if (state.Abbr == abbr)
					return state;
			return null;
		}
	}
}