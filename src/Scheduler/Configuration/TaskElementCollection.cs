using System.Configuration;

namespace MindHarbor.Scheduler.Configuration {
	/// <summary>
	/// ConfigurationElementCollection for DomainLayer Assembly Configuration Element
	/// </summary>
	public class TaskElementCollection : ConfigurationElementCollection {
		protected override string ElementName {
			get { return "task"; }
		}

		///<summary>
		///When overridden in a derived class, creates a new <see cref="T:System.Configuration.ConfigurationElement"></see>.
		///</summary>
		///
		///<returns>
		///A new <see cref="T:System.Configuration.ConfigurationElement"></see>.
		///</returns>
		///
		protected override ConfigurationElement CreateNewElement() {
			return new TaskElement();
		}

		protected override bool IsElementName(string elementName) {
			return elementName == ElementName;
		}

		///<summary>
		///Gets the element key for a specified configuration element when overridden in a derived class.
		///</summary>
		///
		///<returns>
		///An <see cref="T:System.Object"></see> that acts as the key for the specified <see cref="T:System.Configuration.ConfigurationElement"></see>.
		///</returns>
		///
		///<param name="element">The <see cref="T:System.Configuration.ConfigurationElement"></see> to return the key for. </param>
		protected override object GetElementKey(ConfigurationElement element) {
			return ((TaskElement) element).Name;
		}
	}
}