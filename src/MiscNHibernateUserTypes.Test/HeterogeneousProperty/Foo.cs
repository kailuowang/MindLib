using System;
using System.Collections.Generic;
using System.Text;

namespace MindHarbor.MiscNHibernateUserTypes.Test.HeterogeneousProperty {
	using HeterogeneousProperty = MindHarbor.MiscNHibernateUserTypes.HeterogeneousProperty;
	public class Foo
	{
		private int id;

		private string password;

		public int Id
		{
			get { return id; }
			set { id = value; }
		}

		public string Password
		{
			get { return password; }
			set { password = value; }
		}

		private IDictionary<string, HeterogeneousProperty> props = new Dictionary<string, HeterogeneousProperty>();

		public HeterogeneousPropertyDict Props
		{
			get { return new HeterogeneousPropertyDict(props); }
		}
	}
}