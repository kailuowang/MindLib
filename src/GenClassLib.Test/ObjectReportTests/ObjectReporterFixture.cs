using System;
using System.Collections.Generic;
using System.Data;
using MindHarbor.GenClassLib.ObjectReport;
using NUnit.Framework;

namespace MindHarbor.GenClassLib.Test.ObjectReportTests {
	[TestFixture]
	public class ObjectReporterFixture : TestFixtrueBase {
		private static void printTable(DataTable dt) {
			foreach (DataColumn column in dt.Columns) Console.Write(column.ColumnName + " | ");
			Console.Write("\n");
			foreach (DataRow row in dt.Rows) {
				foreach (DataColumn column in dt.Columns)
					Console.Write(row[column] + " | ");
				Console.Write("\n");
			}
		}

		[Test]
		public void Test() {
			IList<MockCalssA> toReport = new List<MockCalssA>();
			for (int i = 0; i < 20; i++)
				toReport.Add(new MockCalssA(RandomName(), RandomInt(), new MockClassB(RandomName(), RandomInt())));

			ObjectReporter or = new ObjectReporter();
			or.AddDeepCopyMapping("AssoicationToB");
			DataTable dt = or.ObjectsToTable(toReport);
			// printTable(dt);
			Assert.IsTrue(dt.Columns.Contains("Number"));
			Assert.IsTrue(dt.Columns.Contains("Name"));
			Assert.IsTrue(dt.Columns.Contains("Quantity"));
			Assert.IsTrue(dt.Columns.Contains("Code"));
			Assert.AreEqual(20, dt.Rows.Count);
			for (int i = 0; i < 20; i++) {
				Assert.AreEqual(toReport[i].Name.ToString(), dt.Rows[i]["Name"]);
				Assert.AreEqual(toReport[i].Number.ToString(), dt.Rows[i]["Number"]);
				Assert.AreEqual(toReport[i].AssoicationToB.Quantity.ToString(), dt.Rows[i]["Quantity"]);
				Assert.AreEqual(toReport[i].AssoicationToB.Code.ToString(), dt.Rows[i]["Code"]);
			}
			or = new ObjectReporter();
			dt = or.ObjectsToTable(toReport);
			//    printTable(dt);
			Assert.IsTrue(dt.Columns.Contains("Number"));
			Assert.IsTrue(dt.Columns.Contains("Name"));
			Assert.IsTrue(dt.Columns.Contains("AssoicationToB"));
			for (int i = 0; i < 20; i++) {
				Assert.AreEqual(toReport[i].Name.ToString(), dt.Rows[i]["Name"]);
				Assert.AreEqual(toReport[i].Number.ToString(), dt.Rows[i]["Number"]);
				Assert.AreEqual(toReport[i].AssoicationToB.ToString(), dt.Rows[i]["AssoicationToB"]);
			}

			Assert.AreEqual(20, dt.Rows.Count);
		}
	}

	public class MockCalssA {
		private MockClassB assoicationToB;
		private string name;
		private int number;

		public MockCalssA(string name, int number, MockClassB mcb) {
			Name = name;
			Number = number;
			AssoicationToB = mcb;
		}

		public MockClassB AssoicationToB {
			get { return assoicationToB; }
			set { assoicationToB = value; }
		}

		public int Number {
			get { return number; }
			set { number = value; }
		}

		public string Name {
			get { return name; }
			set { name = value; }
		}
	}

	public class MockClassB {
		private string code;
		private int quantity;

		public MockClassB(string code, int qty) {
			Code = code;
			Quantity = qty;
		}

		public int Quantity {
			get { return quantity; }
			set { quantity = value; }
		}

		public string Code {
			get { return code; }
			set { code = value; }
		}

		public override string ToString() {
			return "MOckBToString " + code;
		}
	}
}