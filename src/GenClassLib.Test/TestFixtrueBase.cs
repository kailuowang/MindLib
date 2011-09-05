using System;
using MindHarbor.GenClassLib.Data;

namespace MindHarbor.GenClassLib.Test {
	public abstract class TestFixtrueBase {
		protected static string RandomName() {
			return RandomStringGenerator.GenerateLetterStrings(4);
		}

		protected static int RandomInt() {
			return Math.Abs(RandomStringGenerator.GenerateLetterStrings(8).GetHashCode()/1000);
		}
	}
}